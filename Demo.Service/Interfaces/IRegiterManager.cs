using Demo.Service.Base.Dtos;
using Demo.Service.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Interfaces
{
    public interface IRegiterManager
    {
        Task<IdentityResult> RegisterAsync(RegisterUserInputDto input);

        Task UpdateAvatarUserAsync(FileInputDto input);

        Task UpdateInfomationUserAsync(UpdateUserInfomationInputDto input);

        Task UpdateUserPasswordAsync(string input);
    }
}
