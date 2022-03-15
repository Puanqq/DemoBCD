using Demo.Service.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Service.Filters
{
    public class AllowImageExtensionFiles : ActionFilterAttribute
    {
        private readonly string property;
        private readonly List<string> _exImgFile = new List<string>()
        {
            "jpg", "jpeg", "png"
        };

        public AllowImageExtensionFiles(string property)
        {
            this.property = property;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var input = context.ActionArguments.First().Value;
            var value = input.GetType().GetProperty(property).GetValue(input, null).ToString();

            if (!_exImgFile.Contains(value.ToLower()))
            {
                Log.Error("[X][Upload] file upload is not image files");
                throw new UploadFileException("File upload is not Image files");
            }

            base.OnActionExecuting(context);
        }
    }
}
