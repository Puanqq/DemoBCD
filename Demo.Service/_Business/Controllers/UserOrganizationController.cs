using AutoMapper;
using Demo.EntityFramework.Entities;
using Demo.Service.Base;
using Demo.Service.Business.Managers;
using Demo.Service.Dtos;
using Demo.UnitOfWork.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Business.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UserOrganizationController : BaseCrudAsyncController<UserOrganization, UserOrganizationInputDto, UserOrganizationOutputDto, Guid>
    {
        private readonly UserOrganizationManager _userOrganizationManager;
        private readonly IRepository<UserOrganization, Guid> _repository;

        public UserOrganizationController(
            IRepository<UserOrganization, Guid> repository,
            IMapper mapper,
            UserOrganizationManager userOrganizationManager) : base(repository, mapper)
        {
            _userOrganizationManager = userOrganizationManager;
            _repository = repository;
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public override async Task<ActionResult<UserOrganizationOutputDto>> CreateAsync([FromBody] UserOrganizationInputDto input)
        {
            if (_repository.Query.Any(a => a.OrganizationId == input.OrganizationId && a.UserId == input.UserId))
            {
                return BadRequest("user already exists in the department");
            }
            return await base.CreateAsync(input);
        }

        [HttpGet]
        public Task<List<UserOutputDto>> GetListUserNotDependencyUserOrganizationAsync(Guid organizationId)
        {
            return _userOrganizationManager.GetListUserNotDependencyUserOrganizationAsync(organizationId);
        }

        [HttpGet]
        public Task<List<UserOrganizationOutputDto>> GetListUserOrganizationByOrganizationIdAsync(Guid organizationId)
        {
            return _userOrganizationManager.GetListUserOrganizationByOrganizationIdAsync(organizationId);
        }

        [HttpPut]
        public Task UpdateTitleUserOrganizationByIdAsync(UpdateTitleUserOrganizationDto input)
        {
            return _userOrganizationManager.UpdateTitleUserOrganizationByIdAsync(input);
        }
    }
}
