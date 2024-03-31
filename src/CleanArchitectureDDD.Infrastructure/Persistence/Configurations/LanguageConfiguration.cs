using CleanArchitectureDDD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureDDD.Infrastructure.Persistence.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("tb_mt_language", "sc_config");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasColumnName("id")
            .IsRequired();
        builder.Property(t => t.DsLanguage)
            .HasColumnName("ds_language")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(t => t.DsPrefix)
            .HasColumnName("ds_prefix")
            .HasMaxLength(10)
            .IsRequired();
        builder.Property(t => t.DtCreatedAud)
            .HasColumnName("dt_created_aud")
            .IsRequired();
        builder.Property(t => t.IdCreatedAud)
            .HasColumnName("id_created_aud")
            .IsRequired();
        builder.Property(t => t.DtUpdatedAud)
            .HasColumnName("dt_updated_aud");
        builder.Property(t => t.IdUpdatedAud)
            .HasColumnName("id_updated_aud");
        builder.Property(t => t.IsLogicalDelete)
            .HasColumnName("is_logical_delete")
            .IsRequired();
        builder.Property(t => t.DtDeletedAud)
            .HasColumnName("dt_deleted_aud");
        builder.Property(t => t.IdDeletedAud)
            .HasColumnName("id_deleted_aud");
    }
}
