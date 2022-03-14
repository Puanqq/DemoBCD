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
using Demo.Service.Dtos.Message;
using Demo.Service.Interfaces;
using Serilog;

namespace Demo.Service.Business.Managers
{
    public class TitleManager : ITitleManager
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

        public async Task<ActionResult> UpdateTitleOrganizationAsync(Title input)
        {
            try
            {
                await _publishEndpoint.Publish<TitleMessage>(new TitleMessage
                {
                    CorrelationId = Guid.NewGuid(),
                    IsUpdate = true,
                    Title = input
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            return null;
        }

        public async Task<ActionResult> DeleteTitleOrganizationAsync(Guid id)
        {
            try
            {
                await _publishEndpoint.Publish<TitleMessage>(new TitleMessage
                {
                    CorrelationId = Guid.NewGuid(),
                    IsUpdate = false,
                    Title = new Title() { Id = id }
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }

            return null;
        }
    }
}
