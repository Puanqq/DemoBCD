using Demo.EntityFramework.Entities;
using Demo.Service.Dtos;
using Demo.UnitOfWork.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Base
{
   
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseCrudAsyncController<TEntity, TEntityInputDto, TEntityOutputDto, TPrimaryKey> : Controller 
        where TEntity : Entity<TPrimaryKey>
        where TEntityInputDto : EntityDto<TPrimaryKey>
        where TEntityOutputDto : EntityDto<TPrimaryKey>
        where TPrimaryKey : struct
    {
        private readonly IRepository<TEntity, TPrimaryKey> _repository;
        
        public BaseCrudAsyncController(
            IRepository<TEntity, TPrimaryKey> repository
<<<<<<< Updated upstream
        )
=======
, AutoMapper.IMapper mapper)
>>>>>>> Stashed changes
        {
            _repository = repository;
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

            return Ok(entity.JsonMapTo<TEntityOutputDto>());
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<ActionResult<List<TEntityOutputDto>>> GetListAsync()
        {
            var query = await _repository.GetListAsync();

            return Ok(query.JsonMapTo<List<TEntityOutputDto>>());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<ActionResult<TEntityOutputDto>> CreateAsync([FromBody] TEntityInputDto input)
        {
            var query = await _repository.InsertAsync(input.JsonMapTo<TEntity>());

            return Ok(query.JsonMapTo<TEntityOutputDto>());
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<ActionResult> UpdateAsync([FromBody] TEntityInputDto input)
        {
            var entity = await _repository.GetAsync(input.Id);

            if (entity == null)
            {
                return NotFound($"Can't find Id { input }");
            }

            await _repository.UpdateAsync(input.JsonMapTo<TEntity>());

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<ActionResult> DeleteAsync(TPrimaryKey id)
        {
            var entity = await _repository.GetAsync(id);

            if (entity == null)
            {
                return NotFound($"Can't find Id { id }");
            }

            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}
