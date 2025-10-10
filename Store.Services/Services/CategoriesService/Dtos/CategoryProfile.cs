using AutoMapper;
using Store.Data.Entities;

namespace Store.Services.Services.CategoriesService.Dtos
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryCreateDto, Category>().ReverseMap();
            CreateMap<CategoryResultDto , Category>().ReverseMap();
        }
    }
}
