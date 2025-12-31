using AutoMapper;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.DTOs;

namespace Mango.Services.OrderAPI
{
    public class MappingProfile
    {

        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeaderDto, CartHeaderDto>().
                ForMember(dest => dest.CartTotal, u => u.MapFrom(src => src.OrderTotal)).
                ReverseMap();

                config.CreateMap<CartDetailsDto, OrderDetailsDto>().
                ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.Product.Name)).
                ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product.Price));

                config.CreateMap<OrderDetailsDto, CartDetailsDto>();

                config.CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();
                config.CreateMap<OrderHeaderDto, OrderHeader>().ReverseMap();

            });

            return mappingConfig;
        }

    }
}
