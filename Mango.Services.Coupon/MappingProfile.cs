using AutoMapper;
using Mango.Services.CouponAPI.Models.DTOs;

namespace Mango.Services.CouponAPI
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
