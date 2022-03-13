using Demo.Service.Base.Dtos;
using Demo.Service.Base.Enums;
using Demo.Service.Base.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Demo.Service.Base.Managers
{
    public class ExcelManager : IExcelManager
    {
        public ExcelManager()
        {

        }
        public FileOutputDto ExportExcelDefault<T>(List<ExcelHeader> headers, List<T> itemsSource)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                ExcelPackage p = new ExcelPackage();

                p.Workbook.Worksheets.Add("DATA");

                ExcelWorksheet ws = p.Workbook.Worksheets[0];

                ws.Name = "DATA";
                ws.Cells.Style.Font.Size = 13;
                ws.Cells.Style.Font.Name = "Calibri";

                int rowIndex = 1;

                for (int i = 0; i < headers.Count; i++)
                {
                    var cell = ws.Cells[rowIndex, i + 1];

                    cell.Value = headers[i].Value;
                }

                rowIndex++;

                foreach (var item in itemsSource)
                {

                    for (int i = 0; i < headers.Count; i++)
                    {
                        var value = item.GetType().GetProperty(headers[i].Key).GetValue(item);

                        if(value != null && headers[i].Type == ExcelType.DateTime)
                        {
                            ws.Cells[rowIndex, i + 1].Style.Numberformat.Format = "yyyy-mm-dd";
                        }

                        ws.Cells[rowIndex, i + 1].Value = value;
                    }
           
                    rowIndex++;
                }

                var bin = p.GetAsByteArray();

                File.WriteAllBytes(@"D:\\_LEARN\\A.xlsx", bin);

                string base64String = Convert.ToBase64String(bin, 0, bin.Length);

                var base64 = "data:application/octet-stream;base64," + base64String;

                return new FileOutputDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString(),
                    Data = base64,
                    Type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                };
            }
            catch (Exception EE)
            {
            }

            return null;
        }
    }
}
