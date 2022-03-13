using AutoMapper;
using Demo.EntityFramework.Entities;
using Demo.Service.Base;
using Demo.Service.Base.Dtos;
using Demo.Service.Base.Enums;
using Demo.Service.Base.Interfaces;
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

        public TitleController(
            IRepository<Title, Guid> repository, 
            IMapper mapper, 
            IExcelManager excelManager,
            ITitleManager titleManager) : base(repository, mapper, excelManager)
        {
            _titleManager = titleManager;
            SetConfigHeaderExportExcel();
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

        private void SetConfigHeaderExportExcel()
        {
            var obj = new TitleOutputDto();

            _excelHeader = new List<ExcelHeader>()
            {
                new ExcelHeader()
                {
                    Key = nameof(obj.CodeValue),
                    Value = "Title code",
                    Type = ExcelType.Default
                },
                new ExcelHeader()
                {
                    Key = nameof(obj.Name),
                    Value = "Title Name",
                    Type = ExcelType.Default
                }
            };
        }
    }
}
