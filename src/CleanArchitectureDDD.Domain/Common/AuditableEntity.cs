namespace CleanArchitectureDDD.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTimeOffset DtCreatedAud { get; set; }
    public string? IdCreatedAud { get; set; }
    public DateTimeOffset? DtUpdatedAud { get; set; }
    public string? IdUpdatedAud { get; set; }
    public DateTimeOffset? DtDeletedAud { get; set; }
    public string? IdDeletedAud { get; set; }
    public int? IsLogicalDelete { get; set; }
}
