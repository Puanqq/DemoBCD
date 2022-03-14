using Demo.EntityFramework.Entities;
using Demo.Service.Base;
using Demo.Service.Base.Dtos;
using Demo.Service.Base.Interfaces;
using Demo.Service.Business.Managers;
using Demo.Service.Dtos;
using Demo.Service.Enums;
using Demo.Service.Exceptions;
using Demo.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo.Service.Business.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenManager _tokenManager;
        private readonly IRegiterManager _regiterManager;
        private readonly IUserResolverService _userResolverService;

        public AuthenticationController(
            SignInManager<User> signInManager,
            IRegiterManager regiterManager,
            ITokenManager tokenManager,
            IUserResolverService userResolverService
        )
        {
            _tokenManager = tokenManager;
            _signInManager = signInManager;
            _regiterManager = regiterManager;
            _userResolverService = userResolverService;
        }

        [HttpGet]
        [Authorize]
        public Task<UserOutputDto> GetCurrentUserAsync()
        {
            var userId = _userResolverService.GetUserId();

            return _tokenManager.GetUserByIdAsync(userId.ToString());
        }

        [HttpPost]
        public async Task<AuthenticateOutputDto> LoginAsync([FromBody] AuthenticateDto input)
        {
            var result = await _signInManager.PasswordSignInAsync(input.UserName, input.Password, false, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    throw new LoginException("User account locked out.");
                }

                throw new LoginException("Incorrect account or password");
            }
            
            var output = await _tokenManager.BuildToken(input.UserName);

            Log.Information("User logged in.");

            return output;
        }

        [HttpPost]
        public async Task<ActionResult<RegisterUserOutputDto>> RegisterAsync([FromBody] RegisterUserInputDto input)
        {
            var result = await _regiterManager.RegisterAsync(input);
            
            if (!result.Succeeded)
            {
                throw new RegisterException(result.Errors.ConvertToJson());
            }

            return Ok(result.JsonMapTo<RegisterUserOutputDto>());
        }

        [HttpPut]
        [Authorize]
        public async Task UpdateAvatarUserAsync([FromBody] FileInputDto input)
        {
            await _regiterManager.UpdateAvatarUserAsync(input);
        }

        [HttpPut]
        [Authorize]
        public async Task UpdateInfomationUserAsync([FromBody] UpdateUserInfomationInputDto input)
        {
            await _regiterManager.UpdateInfomationUserAsync(input);
        }

        [HttpPut]
        [Authorize]
        public async Task UpdateUserPasswordAsync(string input)
        {
            await _regiterManager.UpdateUserPasswordAsync(input);
        }
    }
  
}
