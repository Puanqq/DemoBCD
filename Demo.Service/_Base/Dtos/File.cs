using Demo.Service.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Base.Dtos
{

    public class ExcelHeader
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public ExcelType Type { get; set; }
    }

    public class FileDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Data { get; set; }

        public string Type { get; set; }
    }

    public class FileInputDto : FileDto
    {

    }

    public class FileOutputDto : FileDto
    {

    }
}
