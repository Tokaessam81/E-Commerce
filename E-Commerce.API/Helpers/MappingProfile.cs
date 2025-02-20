using AutoMapper;
using Ecommerce.Core.DTOS;
using Ecommerce.Core.Entities;
using Ecommerce.DTO;

namespace E_Commerce.API.Helpers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {

            // Department mappings
            CreateMap<Department, DepartmentDTO>()
                .ForMember(D => D.PictureUrl, o => o.MapFrom<PictureUrlDepartmentMappingProfile>())
                .ReverseMap()
                ;
            CreateMap<DepartmentDTO, Department>()
             .ForMember(dest => dest.PictureUrl, opt => opt.Ignore());
            // Category mappings
            CreateMap<Category, CategoryDTO>().ReverseMap();

            // Product mappings
            CreateMap<ProductDTO, Product>()
             .ForMember(dest => dest.Images, opt => opt.MapFrom(src =>
                 src.ImagesUrl.Select(url => new ProductImage { ImageUrl = url }).ToList()));

            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ImagesUrl, opt => opt.MapFrom(src =>
                    src.Images.Select(img => img.ImageUrl).ToList()));
            CreateMap<CustomerBasket, CustomerBasketDTO>()
                .ReverseMap();

            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Coupon, CouponDto>().ReverseMap();
            
            
            // Map BasketItemDTO → BasketItem
            CreateMap<BasketItemDTO, BasketItem>().ReverseMap();
        }
    }
}
