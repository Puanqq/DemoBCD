using AutoMapper;
using Demo.EntityFramework.Entities;
using Demo.Service.Base;
using Demo.Service.Business.Managers;
using Demo.Service.Dtos;
using Demo.Service.Filters;
using Demo.UnitOfWork.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Demo.Service.Business.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class OrganizationController : BaseCrudAsyncController<Organization, OrganizationInputDto, OrganizationOutputDto, Guid>
    {
        private readonly OrganizationManager _organizationManager;

        public OrganizationController(
            IRepository<Organization, Guid> repository,
            IMapper mapper,
            OrganizationManager organizationManager) : base(repository, mapper)
        {
            _organizationManager = organizationManager;
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
    }
}
