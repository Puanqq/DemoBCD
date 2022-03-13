using Demo.Service.Base;
using Demo.Service.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Demo.EntityFramework.Entities;
using Demo.UnitOfWork.interfaces;
using Demo.Service.Interfaces;
using Demo.Service.Base.Dtos;
using Demo.Service.Base.Interfaces;
using Demo.Service.Base.Enums;

namespace Demo.Service.Business.Managers
{
    public class UserOrganizationManager : IUserOrganizationManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<UserOrganization, Guid> _userOrganizationRepository;
        private readonly IExcelManager _excelManager;

        public UserOrganizationManager(
             UserManager<User> userManager,
            IRepository<UserOrganization, Guid> userOrganizationRepository,
            IExcelManager excelManager
        )
        {
            _userOrganizationRepository = userOrganizationRepository;
            _userManager = userManager;
            _excelManager = excelManager;
        }

        public async Task<FileOutputDto> ReportExcelAsync()
        {
            var itemsSourch = await _userOrganizationRepository.Query.Select(s => new ExportExcelUserOrganizationDto()
            {
                Surname = s.User.Surname,
                Name = s.User.Name,
                UserName = s.User.UserName,
                TitleName = s.Title.Name,
                OrganizationName = s.Organization.Name
            }).ToListAsync();

            var obj = new ExportExcelUserOrganizationDto();

            var excelHeader = new List<ExcelHeader>()
            {
                new ExcelHeader()
                {
                    Key = nameof(obj.UserName),
                    Value = "Username",
                    Type = ExcelType.Default
                },
                new ExcelHeader()
                {
                    Key = nameof(obj.Name),
                    Value = "Name",
                    Type = ExcelType.Default
                },
                new ExcelHeader()
                {
                    Key = nameof(obj.Surname),
                    Value = "Surname",
                    Type = ExcelType.Default
                },
                new ExcelHeader()
                {
                    Key = nameof(obj.OrganizationName),
                    Value = "Department",
                    Type = ExcelType.Default
                },
                new ExcelHeader()
                {
                    Key = nameof(obj.TitleName),
                    Value = "Title",
                    Type = ExcelType.Default
                }
            };

            return _excelManager.ExportExcelDefault<ExportExcelUserOrganizationDto>(excelHeader, itemsSourch);
        }

        public async Task<List<UserOutputDto>> GetListUserNotDependencyUserOrganizationAsync(Guid organizationId)
        {
            var listUserId = await _userOrganizationRepository.Query.Where(w => w.OrganizationId == organizationId).Select(s => s.UserId).ToListAsync();
            
            var listUser = await _userManager.Users.Where(w => !listUserId.Contains(w.Id)).ToListAsync();

            return listUser.JsonMapTo<List<UserOutputDto>>();
        }

        public async Task<List<UserOrganizationOutputDto>> GetListUserOrganizationByOrganizationIdAsync(Guid organizationId)
        {
            var listUserOrganization = await _userOrganizationRepository.Query.Where(w => w.OrganizationId == organizationId).Include(i => i.User).Include(i => i.Title).ToListAsync();

            return listUserOrganization.JsonMapTo<List<UserOrganizationOutputDto>>();
        }

        public async Task UpdateTitleUserOrganizationByIdAsync(UpdateTitleUserOrganizationDto input)
        {
            var query = await _userOrganizationRepository.GetAsync(input.Id);

            query.TitleId = input.TitleId;

            await _userOrganizationRepository.UpdateAsync(query);
        }

    }
}
