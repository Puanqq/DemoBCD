using AutoMapper;
using Demo.EntityFramework.Entities;
using Demo.Service.Base;
using Demo.Service.Business.Managers;
using Demo.Service.Dtos;
using Demo.Service.Filters;
using Demo.Service.Interfaces;
using Demo.UnitOfWork.interfaces;
using Microsoft.AspNetCore.Authorization;
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
    public class TitleController : BaseCrudAsyncController<Title, TitleInputDto, TitleOutputDto, Guid, PaginationInputDto>
    {
        private readonly ITitleManager _titleManager;

        public TitleController(IRepository<Title, Guid> repository, IMapper mapper, ITitleManager titleManager) : base(repository, mapper)
        {
            _titleManager = titleManager;
        }

        [NotAllowSpecialCharacters("CodeValue")]
        public override Task<ActionResult<TitleOutputDto>> CreateAsync([FromBody] TitleInputDto input)
        {
            return base.CreateAsync(input);
        }

        [NotAllowSpecialCharacters("CodeValue")]
        public override Task<ActionResult> UpdateAsync([FromBody] TitleInputDto input)
        {
            _titleManager.UpdateTitleOrganizationAsync(input.JsonMapTo<Title>()).Wait();
            return base.UpdateAsync(input);
        }

        public override Task<ActionResult> DeleteAsync(Guid id)
        {
            var result = base.DeleteAsync(id);

            _titleManager.DeleteTitleOrganizationAsync(id).Wait();

            return result;
        }
    }
}
