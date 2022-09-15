using CleanArchitectureDDD.Application.Common.Mappings;
using CleanArchitectureDDD.Domain.Entities;

namespace CleanArchitectureDDD.Application.Languages.Queries.ExportLanguages;

public class LanguageItemRecord : IMapFrom<Language>
{
    public string? DsLanguage { get; set; }

    public string? DsPrefix { get; set; }
}
