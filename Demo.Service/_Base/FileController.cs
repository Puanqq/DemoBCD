using Demo.Service.Base.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Service.Base
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private string _path = "";

        public FileController(
            IConfiguration configuration
        )
        {
            _path = configuration.GetSection("File:Folder").Value;
        }

        [HttpGet]
        public async Task<ActionResult> GetFileAsync(string folder, string id)
        {
      
            var fileValue = await System.IO.File.ReadAllTextAsync(_path + "\\" + folder + "\\" + id);

            var file = fileValue.ConvertFromJson<FileEntity>();

            var base64 = file.Data.Split(",")[1];

            var data = Convert.FromBase64String(base64);

            return File(data, "application/octet-stream", file.Name);
        }
    }
}
