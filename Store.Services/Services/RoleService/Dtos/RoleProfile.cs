using AutoMapper;
using Store.Data.Entities.IdentityEntities;

namespace Store.Services.Services.RoleService.Dtos
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<ApplicationRole, RoleResultDto>().ReverseMap();
            CreateMap<ApplicationRole, RoleCreateDto>().ReverseMap();
            CreateMap<RoleCreateDto, RoleResultDto>().ReverseMap();

        }
    }
}
