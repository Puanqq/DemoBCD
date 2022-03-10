using Demo.EntityFramework.Entities;
using Demo.Service.Base;
using Demo.Service.Business.Managers;
using Demo.Service.Dtos;
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
        private readonly ITokenManager _tokenManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IRegiterManager _regiterManager;

        public AuthenticationController(
            SignInManager<User> signInManager,
            IRegiterManager regiterManager,
            ITokenManager tokenManager
        )
        {
            _tokenManager = tokenManager;
            _signInManager = signInManager;
            _regiterManager = regiterManager;
        }

        [HttpGet]
        [Authorize]
        public Task<UserOutputDto> GetCurrentUserAsync()
        {
            var userId = User.Claims.ToList().Find(f => f.Type == ClaimTypes.NameIdentifier).Value;

            return _tokenManager.GetUserByIdAsync(userId);
        }

        [HttpPost]
        public async Task<ActionResult<AuthenticateOutputDto>> LoginAsync([FromBody] AuthenticateDto input)
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

            return Ok(output);
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
    }
  
}
