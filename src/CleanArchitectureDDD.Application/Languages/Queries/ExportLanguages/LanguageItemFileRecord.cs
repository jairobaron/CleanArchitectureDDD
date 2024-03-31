using CleanArchitectureDDD.Domain.Entities;

namespace CleanArchitectureDDD.Application.Languages.Queries.ExportLanguages;

public class LanguageItemRecord
{
    public string? DsLanguage { get; set; }
    public string? DsPrefix { get; set; }
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Language, LanguageItemRecord>();
        }
    }
}
