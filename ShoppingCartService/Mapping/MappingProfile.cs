using System;
using AutoMapper;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace ShoppingCartService.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ItemDto, Item>();
            CreateMap<CreateCartDto, Cart>()
                .ForMember(dest => dest.CustomerId,
                    opt => opt.MapFrom(src => src.Customer.Id))
                .ForMember(dest => dest.CustomerType,
                    opt => opt.MapFrom(src => src.Customer.CustomerType))
                .ForMember(dest => dest.ShippingAddress,
                    opt => opt.MapFrom(src => src.Customer.Address));

            CreateMap<Item, ItemDto>();
            CreateMap<Cart, ShoppingCartDto>();

            CreateMap<CouponBase, CouponDto>()
                .Include<FreeShippingCoupon, FreeShippingCouponDto>()
                .Include<TypeAbsoluteCoupon, TypeAbsoluteCouponDto>()
                .Include<TypePercentageCoupon, TypePercentageCouponDto>()
                .ReverseMap();
            CreateMap<FreeShippingCoupon, FreeShippingCouponDto>().ReverseMap();
            CreateMap<TypeAbsoluteCoupon, TypeAbsoluteCouponDto>().ReverseMap();
            CreateMap<TypePercentageCoupon, TypePercentageCouponDto>().ReverseMap();
        }
    }
}