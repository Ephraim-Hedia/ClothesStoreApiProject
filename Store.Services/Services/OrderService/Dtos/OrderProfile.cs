using AutoMapper;
using Store.Data.Entities.IdentityEntities;
using Store.Data.Entities.OrderEntities;

namespace Store.Services.Services.OrderService.Dtos
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderItem, OrderItemResultDto>()
                .ForMember(dest => dest.ProductItemId, opt => opt.MapFrom(src => src.ItemOrdered.ProductItemId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ItemOrdered.ProductName))
                .ForMember(dest => dest.ProductColor, opt => opt.MapFrom(src => src.ItemOrdered.ProductColor))
                .ForMember(dest => dest.ProductSize, opt => opt.MapFrom(src => src.ItemOrdered.ProductSize));

            CreateMap<Delivery, DeliveryDto>()
                .ForMember(dest=> dest.ShippingAddress , opt => opt.MapFrom(src => src.ShippingAddress))
                .ReverseMap();

            CreateMap<ShippingAddress, ShippingAddressDto>()
                .ForMember(dest => dest.City , opt => opt.MapFrom(src => src.City)).ReverseMap();

            CreateMap<City, CityResultDto>().ReverseMap();
            CreateMap<ShippingAddress, ShippingAddressCreateDto>().ReverseMap();


            CreateMap<Order, OrderResultDto>()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.GetTotal()))
                .ForMember(dest => dest.Delivery, opt => opt.MapFrom(src => src.DeliveryMethod));
        }
        
    }
}
