using Demo.EntityFramework.Entities;
using Demo.Service.Dtos.Message;
using Demo.Service.Base;
using Demo.Service.Dtos;
using Demo.UnitOfWork.interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Service.Interfaces;

namespace Demo.Service.Business.Consumers
{
    public class UpdateConsumer : IConsumer<TitleMessage>
    {
        private readonly IRepository<Organization, Guid> _organizationRepository;

        public UpdateConsumer(IRepository<Organization, Guid> organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }
        public async Task Consume(ConsumeContext<TitleMessage> context)
        {
            Log.Information("There is an updated title");
            var title = context.Message.Title.JsonMapTo<TitleInputDto>();

            var listOrganizations = await _organizationRepository.Query.Where(w => w.Titles.Contains(title.Id.ToString())).ToListAsync();

            var flag = context.Message.IsUpdate;

            foreach (var org in listOrganizations)
            {
                var listTitle = org.Titles.ConvertFromJson<List<TitleInputDto>>();

                var index = listTitle.FindIndex(f => f.Id == context.Message.Title.Id);

                if (flag)
                {
                    listTitle[index] = title;
                }
                else
                {
                    listTitle.RemoveAt(index);
                }

                org.Titles = listTitle.ConvertToJson();

            }

            await _organizationRepository.UpdateListAsync(listOrganizations);
        }
    }
}
