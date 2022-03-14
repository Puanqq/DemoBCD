using Demo.Service.Base.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Base.Interfaces
{
    public interface IFileManager
    {
        Task<FileOutputDto> SaveFileAsync(FileInputDto input, string folder);
    }
}
