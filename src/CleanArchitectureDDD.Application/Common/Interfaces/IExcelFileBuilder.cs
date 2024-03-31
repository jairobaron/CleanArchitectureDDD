using CleanArchitectureDDD.Application.Languages.Queries.ExportLanguages;

namespace CleanArchitectureDDD.Application.Common.Interfaces;

public interface IExcelFileBuilder
{
    byte[] BuildLanguagesFile(IEnumerable<LanguageItemRecord> records);
}
