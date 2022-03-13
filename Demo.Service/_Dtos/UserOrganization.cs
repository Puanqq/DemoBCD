using Demo.Service.Base.Dtos;
using System;

namespace Demo.Service.Dtos
{
    public class UpdateTitleUserOrganizationDto : EntityDto<Guid>
    {
        public Guid TitleId { get; set; }
    }

    public class UserOrganizationDto : EntityDto<Guid>
    {
        public Guid OrganizationId { get; set; }

        public Guid TitleId { get; set; }

        public Guid UserId { get; set; }
    }

    public class UserOrganizationInputDto : UserOrganizationDto
    {
    }

    public class UserOrganizationOutputDto : UserOrganizationDto
    {
        public UserInfoUserOrganizationDto User { get; set; }

        public InfoOrganizationInUserOrganizationDto Organization { get; set; }

        public InfoTitleInUserOrganizationDto Title { get; set; }
    }

    public class InfoOrganizationInUserOrganizationDto
    {
        public string Name { get; set; }

        public string CodeValue { get; set; }
    }

    public class UserInfoUserOrganizationDto
    {
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }

    public class InfoTitleInUserOrganizationDto
    {
        public string CodeValue { get; set; }

        public string Name { get; set; }
    }

    public class ExportExcelUserOrganizationDto
    {
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string OrganizationName { get; set; }

        public string TitleName { get; set; }
    }
}
