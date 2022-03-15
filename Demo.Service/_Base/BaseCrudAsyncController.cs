using AutoMapper;
using Demo.EntityFramework.Entities;
using Demo.Service.Base.Dtos;
using Demo.Service.Base.Enums;
using Demo.Service.Base.Interfaces;
using Demo.UnitOfWork.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Base
{
   
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseCrudAsyncController<TEntity, TEntityInputDto, TEntityOutputDto, TPrimaryKey, TPaginationInputDto> : Controller 
        where TEntity : Entity<TPrimaryKey>
        where TEntityInputDto : EntityDto<TPrimaryKey>
        where TEntityOutputDto : EntityDto<TPrimaryKey>
        where TPaginationInputDto : PaginationInputDto
        where TPrimaryKey : struct
    {
        private readonly IRepository<TEntity, TPrimaryKey> _repository;
        private readonly IMapper _mapper;
        private readonly IExcelManager _excelManager;

        protected List<ExcelHeader> _excelHeader = new List<ExcelHeader>();

        public BaseCrudAsyncController(
            IRepository<TEntity, TPrimaryKey> repository,
            IMapper mapper,
            IExcelManager excelManager
        )
        {
            _mapper = mapper;
            _repository = repository;
            _excelManager = excelManager;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TEntityOutputDto>> GetAsync(TPrimaryKey id)
        {            
            var entity = await _repository.GetAsync(id);

            if (entity == null)
            {
                return NotFound($"Can't find Id { id }");
            }
            Log.Information("GET method {GetAsync} in BaseCrudAsyncController");
            return Ok(entity.JsonMapTo<TEntityOutputDto>());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<ActionResult<FileOutputDto>> ExportExcelDefaultAsync([FromBody] TPaginationInputDto input)
        {
            
            var pagination = await GetAllAsync(input);

            var result = _excelManager.ExportExcelDefault<TEntityOutputDto>(_excelHeader, ((PaginationOutputDto<TEntityOutputDto>)((OkObjectResult)pagination.Result).Value).Items);
            Log.Information("Post method {ExportExcelDefaultAsync} in BaseCrudAsyncController");
            return result;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<ActionResult<PaginationOutputDto<TEntityOutputDto>>> GetAllAsync([FromBody] TPaginationInputDto input)
        {

            if(input.MaxCountResult == 0)
            {
                return new PaginationOutputDto<TEntityOutputDto>()
                {
                    Items = new List<TEntityOutputDto>(),
                    TotalCount = 0
                };
            }

            var query = _repository.Query;

            if (input.ListCriteria != null && input.ListCriteria.Count() > 0)
            {

                foreach (var item in input.ListCriteria)
                {

                    switch (item.Option)
                    {
                        case OptionCriteriaRequest.Equals:
                            query = query.Where(a => EF.Property<string>(a, item.Property) == item.Value);
                            break;
                        case OptionCriteriaRequest.NotEquals:
                            query = query.Where(a => EF.Property<string>(a, item.Property) != item.Value);
                            break;
                        case OptionCriteriaRequest.Contains:
                            query = query.Where(a => EF.Property<string>(a, item.Property).Contains(item.Value));
                            break;
                        case OptionCriteriaRequest.StartsWith:
                            query = query.Where(a => EF.Property<string>(a, item.Property).StartsWith(item.Value));
                            break;
                        default:
                            break;
                    }

                }
            }
            else
            {
                Log.Warning("Warning: Input is null");
            }

            if (input.Sorting != null)
            {
                var arr = input.Sorting.Split(" ");

                var property = arr[0];
                var typeSorting = arr[1];

                switch (typeSorting)
                {
                    case "ASC":
                        query = query.OrderBy(a => EF.Property<string>(a, property));
                        break;
                    case "DESC":
                        query = query.OrderByDescending(a => EF.Property<string>(a, property));
                        break;
                    default:
                        break;
                }
            }

            var items = await query.Skip(input.SkipCount).Take(input.MaxCountResult).ToListAsync();


            var result = new PaginationOutputDto<TEntityOutputDto>()
            {
                Items = items.JsonMapTo<List<TEntityOutputDto>>(),
                TotalCount = await query.CountAsync()
            };

            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<ActionResult<List<TEntityOutputDto>>> GetListAsync()
        {
            var query = await _repository.GetListAsync();
            if (query == null)
            {
                Log.Warning("This is no data. From GET {GetListAsync} method");
                return NotFound("Query data: none");
            }

            return Ok(query.JsonMapTo<List<TEntityOutputDto>>());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<ActionResult<TEntityOutputDto>> CreateAsync([FromBody] TEntityInputDto input)
        {
            var query = await _repository.InsertAsync(input.JsonMapTo<TEntity>());

            Log.Information("Post method {CreateAsync} in BaseCrudAsyncController");
            return Ok(query.JsonMapTo<TEntityOutputDto>());
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<ActionResult> UpdateAsync([FromBody] TEntityInputDto input)
        {
            var entity = await _repository.GetAsync(input.Id);

            if (entity == null)
            {
                Log.Warning($"[Update Action]: Can't find Id { input.Id.ToString() }");
                return NotFound($"Can't find Id { input }");
            }

            entity = _mapper.Map<TEntityInputDto, TEntity>(input, entity);

            await _repository.UpdateAsync(entity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<ActionResult> DeleteAsync(TPrimaryKey id)
        {
            var entity = await _repository.GetAsync(id);

            if (entity == null)
            {
                Log.Warning($"[Delete Action]: Can't find Id { id }");
                return NotFound($"Can't find Id { id }");
            }

            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}
