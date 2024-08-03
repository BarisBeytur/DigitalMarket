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

            CreateMap<Order, OrderResponse>();
            CreateMap<OrderRequest, Order>();

            CreateMap<OrderDetail, OrderDetailResponse>();
            CreateMap<OrderDetailRequest, OrderDetail>();

            CreateMap<Product, ProductResponse>();
            CreateMap<ProductRequest, Product>();

            CreateMap<ProductCategory, ProductCategoryResponse>();
            CreateMap<ProductCategoryRequest, ProductCategory>();

            CreateMap<User, UserResponse>();
            CreateMap<UserRequest, User>();

        }
    }
}
