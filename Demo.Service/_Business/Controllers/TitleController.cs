using AutoMapper;
using Demo.EntityFramework.Entities;
using Demo.Service.Base;
using Demo.Service.Dtos;
using Demo.Service.Filters;
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
    public class TitleController : BaseCrudAsyncController<Title, TitleInputDto, TitleOutputDto, Guid>
    {
        public TitleController(IRepository<Title, Guid> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        [NotAllowSpecialCharacters("CodeValue")]
        public override Task<ActionResult<TitleOutputDto>> CreateAsync([FromBody] TitleInputDto input)
        {
            return base.CreateAsync(input);
        }

        [NotAllowSpecialCharacters("CodeValue")]
        public override Task<ActionResult> UpdateAsync([FromBody] TitleInputDto input)
        {
            return base.UpdateAsync(input);
        }
    }
}
