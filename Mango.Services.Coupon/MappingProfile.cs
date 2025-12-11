using AutoMapper;
using Mango.Services.Coupon.Models.DTOs;

namespace Mango.Services.Coupon
{
    public class MappingProfile
    {

        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<CouponDto, Models.Coupon>().ReverseMap();
            });

            return mappingConfig;
        }

    }
}
