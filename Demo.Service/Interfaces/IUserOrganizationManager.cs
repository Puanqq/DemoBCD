using Demo.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Interfaces
{
    public interface IUserOrganizationManager
    {
        Task<List<UserOutputDto>> GetListUserNotDependencyUserOrganizationAsync(Guid organizationId);

        Task<List<UserOrganizationOutputDto>> GetListUserOrganizationByOrganizationIdAsync(Guid organizationId);

        Task UpdateTitleUserOrganizationByIdAsync(UpdateTitleUserOrganizationDto input);
    }
}
