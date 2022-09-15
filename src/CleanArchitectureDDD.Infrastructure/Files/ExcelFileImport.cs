using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Application.Languages.Commands.ImportLanguages;

namespace CleanArchitectureDDD.Infrastructure.Files;

public class ExcelFileImport : IExcelFileImport
{
    public Task<List<LanguagesRecord>> ImportLanguagesFile(Stream excelFile)
    {
        var languages = new List<LanguagesRecord>();
        //Lets open the existing excel file and read through its content . Open the excel using openxml sdk
        using (var Excel = SpreadsheetDocument.Open(excelFile, false))
        {
            //create the object for workbook part  
            var workbookPart = Excel.WorkbookPart;
            var worksheetPart = workbookPart?.WorksheetParts.First();

            //statement to get the worksheet object by using the first sheet
            var sheetData = worksheetPart?.Worksheet.Elements<SheetData>().First();
            var rows = sheetData?.Descendants<Row>();
                
            foreach (Row row in rows!)
            {
                if (row == sheetData?.GetFirstChild<Row>())
                    continue;
                var language = new LanguagesRecord();
                var index = 0;

                foreach (Cell cell in row.Elements<Cell>())
                {
                    //statement to take the integer value  
                    var cellValue = string.Empty;
                    if (cell.DataType != null)
                    {
                        if (cell.DataType == CellValues.SharedString)
                        {
                            if (Int32.TryParse(cell.InnerText, out int id))
                            {
                                var item = workbookPart?.SharedStringTablePart?
                                    .SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                if (item?.Text != null)
                                {
                                    //code to take the string value
                                    cellValue = item.Text.Text;
                                }
                                else if (item?.InnerText != null)
                                {
                                    cellValue = item.InnerText;
                                }
                                else if (item?.InnerXml != null)
                                {
                                    cellValue = item.InnerXml;
                                }
                            }
                        }
                    }
                    else
                    {
                        cellValue = cell.InnerText;
                    }

                    switch (index)
                    {
                        case 0:
                            language.DsLanguage = cellValue;
                            break;
                        case 1:
                            language.DsPrefix = cellValue;
                            break;
                        default:
                            break;
                    }
                    index++;
                }
                languages.Add(language);
            }

        }
        return Task.FromResult(languages);
    }
}
