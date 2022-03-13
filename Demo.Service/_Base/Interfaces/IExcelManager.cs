using Demo.Service.Base.Dtos;
using System.Collections.Generic;

namespace Demo.Service.Base.Interfaces
{
    public interface IExcelManager
    {
        FileOutputDto ExportExcelDefault<T>(List<ExcelHeader> headers, List<T> itemsSource);
    }
}
