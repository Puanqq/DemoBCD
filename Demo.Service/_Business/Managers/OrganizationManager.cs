using Demo.Service.Base;
using Demo.Service.Dtos;
using System;
using System.Threading.Tasks;
using Demo.EntityFramework.Entities;
using Demo.UnitOfWork.interfaces;
using Demo.Service.Interfaces;

namespace Demo.Service.Business.Managers
{
    public class OrganizationManager : IOrganizationManager
    {
        private readonly IRepository<Organization, Guid> _organizationRepository;

        public OrganizationManager(
            IRepository<Organization, Guid> organizationRepository
        )
        {
            _organizationRepository = organizationRepository;
        }

        public async Task UpdateTitleOrganizationAsync(TitleOrganizationInputDto input)
        {
            var entity = await _organizationRepository.GetAsync(input.Id);

            if(entity != null)
            {
                entity.Titles = input.ListTitle.ConvertToJson();
                await _organizationRepository.UpdateAsync(entity);
            }
        
        }
    }
}
