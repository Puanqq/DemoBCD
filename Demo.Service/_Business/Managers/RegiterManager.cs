using Demo.EntityFramework.Entities;
using Demo.Service.Base;
using Demo.Service.Dtos;
using Demo.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Demo.Service.Business.Managers
{
    public class RegiterManager : IRegiterManager
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;

        public RegiterManager(
            UserManager<User> userManager,
            ILoggerFactory loggerFactory
        )
        {
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<RegiterManager>();
        }

        public async Task<IdentityResult> RegisterAsync(RegisterUserInputDto input)
        {
            var entity = input.JsonMapTo<User>();

            entity.PasswordHash = new PasswordHasher<User>().HashPassword(entity, input.Password);

            var result = await _userManager.CreateAsync(entity);

            return result;
        }

    }
}
