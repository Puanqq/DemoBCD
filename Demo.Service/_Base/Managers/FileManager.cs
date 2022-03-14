using Demo.Service.Base.Interfaces;
using Demo.Service.Base;
using Demo.Service.Base.Dtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Service.Base.Managers
{
    public class FileManager : IFileManager
    {

        private string _path = "";
        private string _urlFormat = "";

        public FileManager(
            IConfiguration configuration
        )
        {
            _path = configuration.GetSection("File:Folder").Value;
            _urlFormat = configuration.GetSection("File:UrlFormat").Value;
        }

        public async Task<FileOutputDto> SaveFileAsync(FileInputDto input, string folder)
        {

            var path = $"{_path}\\{folder}\\";

            var file = input.JsonMapTo<FileEntity>();

            var id = Guid.NewGuid().ToString() + "_" + input.Name;

            file.Id = id;

            if(file.Data != null && file.Data != "")
            {

                if (System.IO.Directory.Exists(path) == false)
                {
                    System.IO.Directory.CreateDirectory(path);
                }
            }

            path += "\\";

            await System.IO.File.WriteAllTextAsync(path + file.Id, file.ConvertToJson());

            var result = file.JsonMapTo<FileOutputDto>();

            result.Data = string.Format(_urlFormat, folder, id);

            return result;
        }
    }
}
