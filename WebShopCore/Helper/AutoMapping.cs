using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;
using WebShopCore.ViewModel;
using WebShopCore.ViewModel.Auth;
using WebShopCore.ViewModel.Cart;
using WebShopCore.ViewModel.Category;
using WebShopCore.ViewModel.Coupon;
using WebShopCore.ViewModel.Feedback;
using WebShopCore.ViewModel.Image;
using WebShopCore.ViewModel.Order;
using WebShopCore.ViewModel.Product;
using WebShopCore.ViewModel.ProductCategory;
using WebShopCore.ViewModel.ProductFeedBack;
using WebShopCore.ViewModel.User;
using WebShopCore.ViewModel.UserRole;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebShopCore.Helper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            // product
            CreateMap<ProductViewModel, Product>().ReverseMap();
            CreateMap<ProductCrudModel, Product>().ReverseMap();

            CreateMap<ProductImageCrudModel, ProductImage>().ReverseMap();
            CreateMap<ProductImageViewModel, ProductImage>().ReverseMap();
            CreateMap<ProductCategoryViewModel, ProductCategory>().ReverseMap();

            CreateMap<CategoryViewModel, Category>().ReverseMap();
            CreateMap<CategoryCrudModel, Category>().ReverseMap();

            CreateMap<ImageCrudModel, Image>().ReverseMap();
            CreateMap<ImageViewModel, Image>().ReverseMap();

            // coupon
            CreateMap<CouponViewModel, Coupon>().ReverseMap();
            CreateMap<CouponCrudModel, Coupon>().ReverseMap();
            
            // role
            CreateMap<ActionCtrlViewModel, ActionRole>().ReverseMap();

            CreateMap<RoleCrudModel, Role>().ReverseMap();
            CreateMap<RoleViewModel, Role>().ReverseMap();

            // cart
            CreateMap<CartViewModel, Cart>().ReverseMap();
            CreateMap<CartItemViewModel, CartItem>().ReverseMap();

            // order
            CreateMap<OrderViewModel, Order>().ReverseMap();
            CreateMap<OrderCrudModel, Order>().ReverseMap();
            CreateMap<OrderItemViewModel, OrderItem>().ReverseMap();

            // user
            CreateMap<AuthenticationModel, User>().ReverseMap();
            CreateMap<UserCrudModel, User>().ReverseMap();
            CreateMap<UserViewModel, User>().ReverseMap();

            // feedback
            CreateMap<FeedbackCrudModel, Feedback>().ReverseMap();
            CreateMap<FeedbackViewModel, Feedback>().ReverseMap();

        }
    }
}
