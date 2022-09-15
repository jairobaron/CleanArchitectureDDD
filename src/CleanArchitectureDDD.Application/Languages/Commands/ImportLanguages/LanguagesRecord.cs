using CleanArchitectureDDD.Application.Common.Mappings;
using CleanArchitectureDDD.Domain.Entities;

namespace CleanArchitectureDDD.Application.Languages.Commands.ImportLanguages;

public class LanguagesRecord : IMapFrom<Language>
{
    public string? DsLanguage { get; set; }

    public string? DsPrefix { get; set; }
}
