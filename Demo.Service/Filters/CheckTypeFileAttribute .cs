using Demo.Service.Enums;
using Demo.Service.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Service.Filters
{
    public class CheckTypeFileAttribute : ActionFilterAttribute
    {
        private readonly string property;
        private readonly string typeFileImage = ".image/jpg.image/jpeg.image/bmp.image/gif.image/png.jpg.jpeg.bmp.gif.png";
        private readonly string typeFileDocs = ".doc.docx.txt";
        private readonly string typeFileExcel = ".xlsx.xlsm.xlsb.xltx.xltm.xls.xlt.xls.xml.xlam.xla.xlw.xlr";
        private readonly string typeFilePDS = ".pds";

        private string ExtensionFileValid = string.Empty;

        public CheckTypeFileAttribute (string property, TypeFile type)
        {
            this.property = property;
            switch (type)
            {
                case TypeFile.ImageFile :
                    ExtensionFileValid = typeFileImage; break;
                case TypeFile.ExcelFile:
                    ExtensionFileValid = typeFileExcel; break;
                case TypeFile.WordFile:
                    ExtensionFileValid = typeFileDocs; break;
                case TypeFile.PDSFile:
                    ExtensionFileValid = typeFilePDS; break;

            }
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var input = context.ActionArguments.First().Value;
            var value = input.GetType().GetProperty(property).GetValue(input, null).ToString();

            if (!ExtensionFileValid.Contains(value.ToLower()))
            {
                Log.Error("[X][Upload] file upload is not valid files");
                throw new UploadFileException("File upload is not valid files");
            }

            base.OnActionExecuting(context);
        }
    }
}
