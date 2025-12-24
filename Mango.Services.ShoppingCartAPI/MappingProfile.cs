using AutoMapper;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.DTOs;

namespace Mango.Services.ProductAPI
{
    public class MappingProfile
    {

        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<CartDetailsDto, CartDetails>().ReverseMap();
            });

            return mappingConfig;
        }

    }
}
