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
using Demo.Service.Interfaces;
using Demo.Service.Base.Interfaces;
using Demo.Service.Base.Managers;

namespace Demo.Service.Services
{
    public static class ManagerService
    {
        public static void AddManagerRegister(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ExceptionHandlerMiddleware>();

            services.AddTransient<IFileManager, FileManager>();
            services.AddTransient<IExcelManager, ExcelManager>();
            services.AddTransient<IUserResolverService, UserResolverService>();
            services.AddTransient<ITokenManager, TokenManager>(); 
            services.AddTransient<ITitleManager, TitleManager>(); 
            services.AddTransient<IRegiterManager, RegiterManager>(); 
            services.AddTransient<IOrganizationManager, OrganizationManager>(); 
            services.AddTransient<IUserOrganizationManager, UserOrganizationManager>(); 

            services.AddTransient<IRepository<Organization, Guid>, Repository<Organization, Guid>>(); 
            services.AddTransient<IRepository<UserOrganization, Guid>, Repository<UserOrganization, Guid>>(); 
            services.AddTransient<IRepository<Title, Guid>, Repository<Title, Guid>>();
        }
    }
}
