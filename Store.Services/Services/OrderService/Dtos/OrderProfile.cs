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

            CreateMap<ShippingAddress, ShippingAddressDto>().ReverseMap();

            CreateMap<Order, OrderResultDto>()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.GetTotal()));

            CreateMap<OrderCreateDto, Order>()
                .ForMember(dest => dest.Delivery.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress));
        }
    }
}
