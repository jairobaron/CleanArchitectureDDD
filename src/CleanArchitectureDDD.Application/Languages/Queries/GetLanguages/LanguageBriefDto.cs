using CleanArchitectureDDD.Application.Common.Mappings;
using CleanArchitectureDDD.Domain.Entities;

namespace CleanArchitectureDDD.Application.Languages.Queries.GetLanguages;

public class LanguageBriefDto : IMapFrom<Language>
{
    public long CdLanguage { get; set; }
    public string? DsLanguage { get; set; }
    public string? DsPrefix { get; set; }
}
