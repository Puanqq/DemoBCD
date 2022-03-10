using Demo.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo.Service.Services
{
    public class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor _context;

        public UserResolverService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public Guid? GetUserId()
        {
            var nameIdentifier = _context.HttpContext?.User?.Claims?.ToList()?.Find(f => f.Type == ClaimTypes.NameIdentifier)?.Value;

            return nameIdentifier != null ? Guid.Parse(nameIdentifier) : null;
        }
    }
}
