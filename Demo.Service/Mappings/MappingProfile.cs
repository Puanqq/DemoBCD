
using AutoMapper;
using Demo.EntityFramework.Entities;
using Demo.Service.Dtos;

namespace Demo.Service.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrganizationInputDto, Organization>().ForMember(f => f.Titles, a => a.Ignore());
            CreateMap<TitleInputDto, Title>();
            CreateMap<UserOrganizationInputDto, UserOrganization>();
        }
    }
}
