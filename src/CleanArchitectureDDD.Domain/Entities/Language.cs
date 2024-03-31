namespace CleanArchitectureDDD.Domain.Entities;

public class Language: AuditableEntity
{
    public string? DsLanguage { get; set; }
    public string? DsPrefix { get; set; }
}
