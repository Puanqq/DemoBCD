using Demo.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Interfaces
{
    public interface IOrganizationManager
    {
        Task UpdateTitleOrganizationAsync(TitleOrganizationInputDto input);
    }
}
