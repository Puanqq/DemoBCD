using Demo.Service.Base.Dtos;
using Demo.Service.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Interfaces
{
    public interface IOrganizationManager
    {
        Task UpdateTitleOrganizationAsync(TitleOrganizationInputDto input);

        ActionResult<FileOutputDto> ExportExcelOrganizationTitleAsync(List<OrganizationOutputDto> list);
    }
}
