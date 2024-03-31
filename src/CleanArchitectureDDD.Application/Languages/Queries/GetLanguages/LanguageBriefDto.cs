using CleanArchitectureDDD.Domain.Entities;

namespace CleanArchitectureDDD.Application.Languages.Queries.GetLanguages;

public class LanguageBriefDto
{
    public string? CdLanguage { get; set; }
    public string? DsLanguage { get; set; }
    public string? DsPrefix { get; set; }
    private class Mapping : Profile
    {
        public Mapping() 
        {
            CreateMap<Language, LanguageBriefDto>();
        }
    }
}
