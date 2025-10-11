using Store.Repositories.Interfaces;
using Store.Repositories.Repositories;
using Store.Services.Services.CategoriesService;
using Store.Services.Services.CategoriesService.Dtos;
using Store.Services.Services.DiscountService;
using Store.Services.Services.DiscountService.Dtos;
using Store.Services.Services.ProductColorService;
using Store.Services.Services.ProductColorService.Dtos;
using Store.Services.Services.ProductService;
using Store.Services.Services.ProductService.Dtos;
using Store.Services.Services.ProductSizeService;
using Store.Services.Services.ProductSizeService.Dtos;
using Store.Services.Services.SubcategoryService;
using Store.Services.Services.SubcategoryService.Dtos;

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



            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(ColorProfile));
            services.AddAutoMapper(typeof(SizeProfile));
            services.AddAutoMapper(typeof(CategoryProfile));
            services.AddAutoMapper(typeof(SubcategoryProfile));
            services.AddAutoMapper(typeof(DiscountProfile));


            return services;
        }
    }
}
