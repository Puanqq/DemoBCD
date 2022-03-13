using Demo.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Dtos
{
    public class CriteriaRequestDto
    {
        public string Property { get; set; }

        public OptionCriteriaRequest Option { get; set; }

        public string Value { get; set; }
    }
}
