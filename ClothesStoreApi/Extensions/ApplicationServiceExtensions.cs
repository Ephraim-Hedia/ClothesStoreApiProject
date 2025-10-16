using Store.Repositories.Interfaces;
using Store.Repositories.Repositories;
using Store.Services.Helper.Validation;
using Store.Services.Services.CategoriesService;
using Store.Services.Services.CategoriesService.Dtos;
using Store.Services.Services.DiscountService;
using Store.Services.Services.DiscountService.Dtos;
using Store.Services.Services.EmailService;
using Store.Services.Services.ProductColorService;
using Store.Services.Services.ProductColorService.Dtos;
using Store.Services.Services.ProductService;
using Store.Services.Services.ProductService.Dtos;
using Store.Services.Services.ProductSizeService;
using Store.Services.Services.ProductSizeService.Dtos;
using Store.Services.Services.RoleService;
using Store.Services.Services.RoleService.Dtos;
using Store.Services.Services.SubcategoryService;
using Store.Services.Services.SubcategoryService.Dtos;
using Store.Services.Services.TokenService;
using Store.Services.Services.AccountService;
using Store.Services.Services.UserService;
using Store.Services.Services.AccountService.Dtos;
using Store.Services.Services.UserService.Dtos;
using Store.Services.Services.AddressService;
using Store.Services.Services.BasketService;
using Store.Services.Services.BasketService.Dtos;
using Store.Services.Services.OrderService;
using Store.Services.Services.OrderService.Dtos;

namespace Store.Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork , UnitOfWork> ();
            services.AddScoped<IProductColorService, ProductColorService>();
            services.AddScoped<IProductSizeService, ProductSizeService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISubcategoryService, SubcategoryService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IOrderService, OrderService>();





            services.AddScoped<EntityListValidator>();
            services.AddScoped<EntityValidator>();




            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(ColorProfile));
            services.AddAutoMapper(typeof(SizeProfile));
            services.AddAutoMapper(typeof(CategoryProfile));
            services.AddAutoMapper(typeof(SubcategoryProfile));
            services.AddAutoMapper(typeof(DiscountProfile));
            services.AddAutoMapper(typeof(RoleProfile));
            services.AddAutoMapper(typeof(AccountProfile));
            services.AddAutoMapper(typeof(UserProfile));
            services.AddAutoMapper(typeof(BasketProfile));
            services.AddAutoMapper(typeof(OrderProfile));




            return services;
        }
    }
}
