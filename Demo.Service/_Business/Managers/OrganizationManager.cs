using Demo.Service.Base;
using Demo.Service.Dtos;
using System;
using System.Threading.Tasks;
using Demo.EntityFramework.Entities;
using Demo.UnitOfWork.interfaces;
using Demo.Service.Interfaces;
using System.Collections.Generic;
using Demo.Service.Base.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Demo.Service.Base.Dtos;
using Demo.Service.Base.Enums;

namespace Demo.Service.Business.Managers
{
    public class OrganizationManager : IOrganizationManager
    {
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IExcelManager _excelManager;

        public OrganizationManager(
            IRepository<Organization, Guid> organizationRepository,
            IExcelManager excelManager
        )
        {
            _organizationRepository = organizationRepository;
            _excelManager = excelManager;
        }

        public async Task UpdateTitleOrganizationAsync(TitleOrganizationInputDto input)
        {
            var entity = await _organizationRepository.GetAsync(input.Id);

            if(entity != null)
            {
                entity.Titles = input.ListTitle.ConvertToJson();
                await _organizationRepository.UpdateAsync(entity);
            }
        
        }

        public ActionResult<FileOutputDto> ExportExcelOrganizationTitleAsync(List<OrganizationOutputDto> list)
        {
            var itemsSourch = new List<OrganizationOutputDto>();

            foreach (var item in list)
            {
               if(item.Titles == null)
               {
                    item.Titles = "#NODATA";
                    itemsSourch.Add(item.JsonMapTo<OrganizationOutputDto>());
                    continue;
               }

                var listTitle = item.Titles.ConvertFromJson<List<TitleDto>>();

                foreach (var title in listTitle)
                {
                    item.Titles = title.Name;
                    itemsSourch.Add(item.JsonMapTo<OrganizationOutputDto>());
                }
            }

            var obj = new OrganizationOutputDto();

            var excelHeader = new List<ExcelHeader>()
            {
                new ExcelHeader()
                {
                    Key = nameof(obj.CodeValue),
                    Value = "Department code",
                    Type = ExcelType.Default
                },
                new ExcelHeader()
                {
                    Key = nameof(obj.Name),
                    Value = "Department Name",
                    Type = ExcelType.Default
                },
                new ExcelHeader()
                {
                    Key = nameof(obj.Titles),
                    Value = "Title",
                    Type = ExcelType.Default
                },
                new ExcelHeader()
                {
                    Key = nameof(obj.CreatedTime),
                    Value = "Created Time",
                    Type = ExcelType.DateTime
                }
            };

            var result = _excelManager.ExportExcelDefault<OrganizationOutputDto>(excelHeader, itemsSourch);

            return result;
        }
    }
}
