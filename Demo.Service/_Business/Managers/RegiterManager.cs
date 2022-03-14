using AutoMapper;
using Demo.EntityFramework.Entities;
using Demo.Service.Base;
using Demo.Service.Base.Dtos;
using Demo.Service.Base.Interfaces;
using Demo.Service.Dtos;
using Demo.Service.Enums;
using Demo.Service.Exceptions;
using Demo.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Threading.Tasks;

namespace Demo.Service.Business.Managers
{
    public class RegiterManager : IRegiterManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserResolverService _userResolverService;
        private readonly ITokenManager _tokenManager;
        private readonly IFileManager _fileManager;
        private readonly IMapper _mapper;

        public RegiterManager(
            UserManager<User> userManager,
            IUserResolverService userResolverService,
            ITokenManager tokenManager,
            IFileManager fileManager,
            IMapper mapper
        )
        {
            _userManager = userManager;
            _userResolverService = userResolverService;
            _tokenManager = tokenManager;
            _fileManager = fileManager;
            _mapper = mapper;
        }

        /// <summary>
        /// register user
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterAsync(RegisterUserInputDto input)
        {
            var entity = input.JsonMapTo<User>();

            entity.PasswordHash = new PasswordHasher<User>().HashPassword(entity, input.Password);

            var result = await _userManager.CreateAsync(entity);

            return result;
        }


        /// <summary>
        /// Update avatar user
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAvatarUserAsync(FileInputDto input)
        {
            var userId = _userResolverService.GetUserId().ToString();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException($"Can't find user {userId}");
            }

            var file = await _fileManager.SaveFileAsync(input, FileFolder.User.ToString());

            user.Avatar = file.Data;

            await _userManager.UpdateAsync(user);

            Log.Information("Update avatar user success");
        }

        /// <summary>
        /// Update infomation user
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateInfomationUserAsync(UpdateUserInfomationInputDto input)
        {
            var userId = _userResolverService.GetUserId().ToString();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException($"Can't find user {userId}");
            }

            user = _mapper.Map<UpdateUserInfomationInputDto, User>(input, user);

            await _userManager.UpdateAsync(user);

            Log.Information("Update infomation user success");
        }

        public async Task UpdateUserPasswordAsync(string password)
        {
            var userId = _userResolverService.GetUserId().ToString();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException($"Can't find user {userId}");
            }

            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, password);

            await _userManager.UpdateAsync(user);

            Log.Information("Update password user success");
        }
    }
}
