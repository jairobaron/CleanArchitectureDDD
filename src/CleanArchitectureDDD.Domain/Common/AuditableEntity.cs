namespace CleanArchitectureDDD.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime DtUserCreationAud { get; set; }

    public long CdUserCreatorAud { get; set; }

    public DateTime? DtUserUpdateAud { get; set; }

    public long? CdUserUpdateAud { get; set; }

    public int? IsLogicalDelete { get; set; }

    public int? IsValidRecord { get; set; }

    public int? IsSync { get; set; }
}
