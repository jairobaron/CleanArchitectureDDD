using CleanArchitectureDDD.Application.Languages.Commands.ImportLanguages;

namespace CleanArchitectureDDD.Application.Common.Interfaces;

public interface IExcelFileImport
{
    Task<List<LanguagesRecord>> ImportLanguagesFile(Stream excelFile);
}
