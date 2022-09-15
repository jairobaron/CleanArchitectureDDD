using CleanArchitectureDDD.Application.Languages.Queries.ExportLanguages;

namespace CleanArchitectureDDD.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildLanguagesFile(IEnumerable<LanguageItemRecord> records);
}
