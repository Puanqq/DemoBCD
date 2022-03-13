using Demo.EntityFramework.Entities;
using Demo.Service.Base.Dtos;
using System;
using System.Collections.Generic;

namespace Demo.Service.Dtos
{
    public class OrganizationDto : EntityDto<Guid>
    {
        public string CodeValue { get; set; }

        public string Name { get; set; }
    }

    public class OrganizationInputDto : OrganizationDto
    {
    }

    public class OrganizationOutputDto : OrganizationDto
    {
        public string Titles { get; set; }

        public virtual IEnumerable<UserOrganization> UserOrganizations { get; set; }
    }

    public class TitleOrganizationInputDto : EntityDto<Guid>
    {
        public List<TitleInputDto> ListTitle { get; set; }
    }
}
