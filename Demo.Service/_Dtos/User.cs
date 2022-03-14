using Demo.Service.Base.Dtos;
using System;

namespace Demo.Service.Dtos
{
    public class UserDto : EntityDto<Guid>
    {
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Avatar { get; set; }
    }

    public class UserOutputDto : UserDto { }


    public class UpdateUserInfomationInputDto
    {
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
