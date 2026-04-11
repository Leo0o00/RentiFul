using Applications.Sagas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Applications.Data.Configurations;

public class UpdateApplicationSagaDataEntityConfiguration : IEntityTypeConfiguration<UpdateApplicationSagaData>
{
    public void Configure(EntityTypeBuilder<UpdateApplicationSagaData> builder)
    {
        builder.HasKey(x => x.CorrelationId);

        builder.Property(x => x.ApplicationId).ValueGeneratedNever();

        builder.Property(x => x.PropertyId).ValueGeneratedNever();

        builder.Property(x => x.TenantCognitoId).ValueGeneratedNever();
    }
}