using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.IServices;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.DTOs;
using Mango.Services.ShoppingCartAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ResponseDto _response;
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        public CartAPIController(IMapper mapper, AppDbContext context, IProductService productService, ICouponService couponService)
        {
            _mapper = mapper;
            _response = new ResponseDto();
            _context = context;
            _productService = productService;
            _couponService = couponService;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            ResponseDto result = new();

            try
            {
                var cartDetails = await _context.CartDetails.FirstOrDefaultAsync(u => u.CartDetailsId == cartDetailsId);
                var totalCountOfCartItem = _context.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();

                _context.CartDetails.Remove(cartDetails);

                if (totalCountOfCartItem == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _context.SaveChangesAsync();

                _response.Result = true;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message.ToString();
            }

            return result;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            ResponseDto result = new();

            try
            {
                var cartHeaderFromDb = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == cartDto.CartHeader.UserId);

                if (cartHeaderFromDb == null)
                {
                    //Create header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _context.CartHeaders.Add(cartHeader);
                    await _context.SaveChangesAsync();

                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _context.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _context.SaveChangesAsync();

                }
                else
                {
                    //if header is not null then it is existsed
                    //Check if details has same product
                    var cartDetailsFromDb = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(c =>
                    c.ProductId == cartDto.CartDetails.First().ProductId &&
                    c.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                    if (cartDetailsFromDb == null)
                    {
                        //Create Cart Details
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _context.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        //Update Count in Cart Details
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;

                        _context.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _context.SaveChangesAsync();
                    }
                }
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message.ToString();
            }

            return result;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCartByUserID(string userId)
        {

            ResponseDto response = new();

            try
            {
                var userCartHeader = _context.CartHeaders.AsNoTracking().FirstOrDefault(ch => ch.UserId == userId);

                if (userCartHeader != null)
                {
                    CartDto cart = new CartDto() { CartHeader = _mapper.Map<CartHeaderDto>(userCartHeader) };
                    var cartDetails = _context.CartDetails.AsNoTracking().Where(cd => cd.CartHeaderId == userCartHeader.CartHeaderId).ToList();
                    cart.CartDetails = _mapper.Map<List<CartDetailsDto>>(cartDetails);
                    IEnumerable<ProductDto> products = await _productService.GetProductsAsync();

                    foreach (var detail in cart.CartDetails)
                    {
                        cart.CartHeader.CartTotal = cart.CartHeader.CartTotal ?? 0;
                        cart.CartHeader.CartTotal += (detail.Count * products.FirstOrDefault(p => p.ProductId == detail.ProductId).Price);
                    }

                    if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                    {
                        var coupon = await _couponService.GetCouponsByCodeAsync(cart.CartHeader.CouponCode);
                        if (coupon != null)
                        {
                            cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                            cart.CartHeader.Dicsount = coupon.DiscountAmount;
                        }
                    }

                    response.Result = cart;
                    response.IsSuccess = true;
                    response.Message = "Success";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Cart Header Not Found!";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;

        }

        [HttpPost("ApplyCoupon")]
        public async Task<Object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _context.CartHeaders.FirstAsync(c => c.UserId == cartDto.CartHeader.UserId);
                cartFromDb.CouponCode = cartDto.CartHeader.CouponCode;
                _context.CartHeaders.Update(cartFromDb);
                await _context.SaveChangesAsync();
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
            }

            return _response;
        }

        //[HttpPost("RemoveCoupon")]
        //public async Task<Object> RemoveCoupon([FromBody] CartDto cartDto)
        //{
        //    try
        //    {
        //        var cartFromDb = await _context.CartHeaders.FirstAsync(c => c.UserId == cartDto.CartHeader.UserId);
        //        cartFromDb.CouponCode = "";
        //        _context.CartHeaders.Update(cartFromDb);
        //        await _context.SaveChangesAsync();
        //        _response.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = ex.Message.ToString();
        //    }

        //    return _response;
        //}

    }
}
