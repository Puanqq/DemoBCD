using Demo.Service.Base.Dtos;
using System;

namespace Demo.Service.Dtos
{
    public class TitleDto : EntityDto<Guid>
    {
        public string CodeValue { get; set; }

        public string Name { get; set; }
    }

    public class TitleInputDto : TitleDto
    {

    }

    public class TitleOutputDto : TitleDto
    {

    }
}
