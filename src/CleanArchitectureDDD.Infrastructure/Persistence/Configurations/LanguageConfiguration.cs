using CleanArchitectureDDD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureDDD.Infrastructure.Persistence.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("tb_mt_language", "sc_config");
        builder.HasKey(t => t.CdLanguage);
        builder.Property(t => t.CdLanguage)
            .HasColumnName("cd_language")
            .UseIdentityColumn(seed: 1, increment: 1)
            .IsRequired();
        builder.Property(t => t.DsLanguage)
            .HasColumnName("ds_language")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(t => t.DsPrefix)
            .HasColumnName("ds_prefix")
            .HasMaxLength(10)
            .IsRequired();
        builder.Property(t => t.DtUserCreationAud)
            .HasColumnName("dt_user_creation_aud")
            .IsRequired();
        builder.Property(t => t.CdUserCreatorAud)
            .HasColumnName("cd_user_creator")
            .IsRequired();
        builder.Property(t => t.DtUserUpdateAud)
            .HasColumnName("dt_user_update_aud");
        builder.Property(t => t.CdUserUpdateAud)
            .HasColumnName("cd_user_update_aud");
        builder.Property(t => t.IsLogicalDelete)
            .HasColumnName("is_logical_delete")
            .IsRequired();        
        builder.Property(t => t.IsValidRecord)
            .HasColumnName("is_valid_record");
        builder.Property(t => t.IsSync)
            .HasColumnName("is_sync");

    }
}
