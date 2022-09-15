using System.Data;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Application.Languages.Queries.ExportLanguages;
using CleanArchitectureDDD.Infrastructure.Files.Maps;

namespace CleanArchitectureDDD.Infrastructure.Files;

public class ExcelFileBuilder : IExcelFileBuilder
{
    public byte[] BuildLanguagesFile(IEnumerable<LanguageItemRecord> records)
    {
        using var memoryStream = new MemoryStream();
        using (var Excel = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookPart = Excel.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);

            Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
            var sheet = new Sheet() 
            { 
                Id = workbookPart.GetIdOfPart(worksheetPart), 
                SheetId = 1, 
                Name = "Sheet1" 
            };

            sheets.Append(sheet);

            var headerRow = new Row();

            var columns = new List<string>();
            foreach (var column in typeof(LanguageItemRecord).GetProperties())
            {
                columns.Add(column.Name);

                var cell = new Cell()
                {
                    DataType = CellValues.String,
                    CellValue = new CellValue(column.Name)
                };
                headerRow.AppendChild(cell);
            }

            sheetData.AppendChild(headerRow);

            foreach (var dsrow in records)
            {
                Type t = dsrow.GetType();
                PropertyInfo[] props = t.GetProperties();

                var newRow = new Row();
                foreach (var prop in props)
                {
                    var cell = new Cell()
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(prop.GetValue(dsrow)?.ToString() ?? "")
                    };

                    newRow.AppendChild(cell);
                }
                sheetData.AppendChild(newRow);
            }

            workbookPart.Workbook.Save();
            Excel.Close();
        }

        return memoryStream.ToArray();
    }
}
