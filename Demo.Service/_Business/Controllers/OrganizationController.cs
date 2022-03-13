using AutoMapper;
using Demo.EntityFramework.Entities;
using Demo.Service.Base;
using Demo.Service.Base.Dtos;
using Demo.Service.Base.Enums;
using Demo.Service.Base.Interfaces;
using Demo.Service.Dtos;
using Demo.Service.Filters;
using Demo.Service.Interfaces;
using Demo.UnitOfWork.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Service.Business.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Authorize]
    public class OrganizationController : BaseCrudAsyncController<Organization, OrganizationInputDto, OrganizationOutputDto, Guid, PaginationInputDto>
    {
        private readonly IOrganizationManager _organizationManager;

        public OrganizationController(
            IRepository<Organization, Guid> repository,
            IMapper mapper, 
            IExcelManager excelManager,
            IOrganizationManager organizationManager) : base(repository, mapper, excelManager)
        {
            _organizationManager = organizationManager;
            SetConfigHeaderExportExcel();
        }


        [NotAllowSpecialCharacters("CodeValue")]
        public override Task<ActionResult<OrganizationOutputDto>> CreateAsync([FromBody] OrganizationInputDto input)
        {
            return base.CreateAsync(input);
        }

        [NotAllowSpecialCharacters("CodeValue")]
        public override Task<ActionResult> UpdateAsync([FromBody] OrganizationInputDto input)
        {
            return base.UpdateAsync(input);
        }

        [HttpPut]
        public Task UpdateTitleOrganizationAsync([FromBody] TitleOrganizationInputDto input)
        {
            return _organizationManager.UpdateTitleOrganizationAsync(input);
        }

        private void SetConfigHeaderExportExcel()
        {
            var obj = new OrganizationOutputDto();

            _excelHeader = new List<ExcelHeader>()
            {
                new ExcelHeader()
                {
                    Key = nameof(obj.CodeValue),
                    Value = "Department code",
                    Type = ExcelType.Default
                },
                new ExcelHeader()
                {
                    Key = nameof(obj.Name),
                    Value = "Department Name",
                    Type = ExcelType.Default
                }
            };
        }

    }
}
