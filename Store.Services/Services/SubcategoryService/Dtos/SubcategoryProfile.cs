using AutoMapper;
using Store.Data.Entities;

namespace Store.Services.Services.SubcategoryService.Dtos
{
    public class SubcategoryProfile : Profile
    {
        public SubcategoryProfile()
        {
            CreateMap<Subcategory , SubcategoryCreateDto>().ReverseMap();
            CreateMap<Subcategory, SubcategoryResultDto>().ReverseMap();
        }
    }
}
