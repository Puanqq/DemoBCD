using Demo.Service.Base;
using Demo.Service.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.EntityFramework.Entities;
using Demo.UnitOfWork.interfaces;
using MassTransit;
using Demo.Service._Dtos.Message;

namespace Demo.Service.Business.Managers
{
    public class TitleManager
    {
        private readonly IRepository<Title, Guid> _titleRepository;
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public TitleManager(
            IRepository<Title, Guid> titleRepository,
            IRepository<Organization, Guid> organizationRepository,
            IPublishEndpoint publishEndpoint
        )
        {
            _titleRepository = titleRepository;
            _organizationRepository = organizationRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ActionResult> UpdateTitleOrganization(Title input)
        {
            await _publishEndpoint.Publish<TitleMessage>(new TitleMessage
            {
                CorrelationId = Guid.NewGuid(),
                IsUpdate = true,
                Title = input
            });
            return null;
        }

        public async Task<ActionResult> DeleteTitleOrganization(Guid id)
        {
            var title = await _titleRepository.GetAsync(id);
            await _publishEndpoint.Publish<TitleMessage>(new TitleMessage
            {
                CorrelationId = Guid.NewGuid(),
                IsUpdate = false,
                Title = title
            });

            return null;
        }
    }
}
