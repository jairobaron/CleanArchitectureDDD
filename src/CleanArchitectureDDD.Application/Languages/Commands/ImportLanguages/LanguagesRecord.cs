using CleanArchitectureDDD.Domain.Entities;

namespace CleanArchitectureDDD.Application.Languages.Commands.ImportLanguages;

public class LanguagesRecord
{
    public string? DsLanguage { get; set; }
    public string? DsPrefix { get; set; }
    public class Mapping : Profile
    {
        public Mapping() 
        {
            CreateMap<Language, LanguagesRecord>();
        }
    }
}
