using Demo.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Base.Dtos
{
    public class PaginationInputDto
    {
        public int MaxCountResult { get; set; } = 999;

        public int SkipCount { get; set; } = 0;

        public string Sorting { get; set; }

        public List<CriteriaRequestDto> ListCriteria { get; set; }
    }

    public class PaginationOutputDto<TEntityOutputDto> where TEntityOutputDto : class
    {
        public List<TEntityOutputDto> Items { get; set; }

        public int TotalCount { get; set; }
    }
}
