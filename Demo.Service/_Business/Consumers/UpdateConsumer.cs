using Demo.EntityFramework.Entities;
using Demo.Service._Dtos.Message;
using Demo.UnitOfWork.interfaces;
using MassTransit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service._Business.Consumers
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
            Title title = context.Message.Title;

            var listOrganizations = await _organizationRepository.GetListAsync();
            var organizationsNeedUpdate = listOrganizations.ToList();
            if (!context.Message.IsUpdate)
            {
                organizationsNeedUpdate = listOrganizations.Where(x => x.Titles.Contains(title.Id.ToString())).ToList();
            }

            foreach(var org in organizationsNeedUpdate)
            {
                var listTitles = JsonConvert.DeserializeObject<List<Title>>(org.Titles);

                if (context.Message.IsUpdate)
                {
                    listTitles.Add(title);
                }
                else
                {
                    var titleNeedRemove = listTitles.First(t => t.Id == title.Id);
                    listTitles.Remove(titleNeedRemove);
                }
                org.Titles = JsonConvert.SerializeObject(listTitles);
                await _organizationRepository.UpdateAsync(org);
            }
        }
    }
}
