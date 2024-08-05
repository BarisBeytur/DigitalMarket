using AutoMapper;
using DigitalMarket.Data.Domain;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Business.Infrastructure.Mapping.AutoMapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {

            CreateMap<Category, CategoryResponse>();
            CreateMap<CategoryRequest, Category>();

            CreateMap<Coupon, CouponResponse>();
            CreateMap<CouponRequest, Coupon>();

            CreateMap<DigitalWallet, DigitalWalletResponse>();
            CreateMap<DigitalWalletRequest, DigitalWallet>();
            CreateMap<DigitalWallet, PointBalanceResponse>().ReverseMap();

            CreateMap<Order, OrderResponse>();
            CreateMap<OrderRequest, Order>();

            CreateMap<OrderDetail, OrderDetailResponse>()
                .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.Product.ProductCategories.Select(pc => pc.CategoryId).ToArray()));
            CreateMap<OrderDetailRequest, OrderDetail>();

            CreateMap<Product, ProductResponse>();
            CreateMap<ProductRequest, Product>();
            CreateMap<ProductRequest, ProductResponse>().ReverseMap();


            CreateMap<ProductCategory, ProductCategoryResponse>();
            CreateMap<ProductCategoryRequest, ProductCategory>();
            CreateMap<ProductCategory, ProductWithCategoryResponse>();


            CreateMap<User, UserResponse>();
            CreateMap<UserRequest, User>();


        }
    }
}
