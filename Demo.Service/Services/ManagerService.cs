using Demo.EntityFramework.Entities;
using Demo.Service.Business.Consumers;
using Demo.Service.Base;
using Demo.Service.Business.Managers;
using Demo.Service.Middlewares;
using Demo.UnitOfWork.Base;
using Demo.UnitOfWork.interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Demo.Service.Services
{
    public static class ManagerService
    {
        public static void AddManagerRegister(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ExceptionHandlerMiddleware>();

            services.AddTransient<TokenManager>(); 
            services.AddTransient<TitleManager>(); 
            services.AddTransient<RegiterManager>(); 
            services.AddTransient<OrganizationManager>(); 
            services.AddTransient<UserOrganizationManager>(); 

            services.AddTransient<IRepository<Organization, Guid>, Repository<Organization, Guid>>(); 
            services.AddTransient<IRepository<UserOrganization, Guid>, Repository<UserOrganization, Guid>>(); 
            services.AddTransient<IRepository<Title, Guid>, Repository<Title, Guid>>();
        }
    }
}
