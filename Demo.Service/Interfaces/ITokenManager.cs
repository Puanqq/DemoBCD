using Demo.Service.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Interfaces
{
    public interface ITokenManager
    {
        Task<UserOutputDto> GetUserByIdAsync(string id);

        Task<AuthenticateOutputDto> BuildToken(string userName);
    }
}
