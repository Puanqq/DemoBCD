using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Base.Dtos
{
    public class EntityDto<TPrimaryKey> : IEntityDto<TPrimaryKey> where TPrimaryKey : struct
    {
        public TPrimaryKey Id { get; set; }
    }

    public interface IEntityDto<TPrimaryKey> where TPrimaryKey : struct
    {
        public TPrimaryKey Id { get; set; }
    }
}
