namespace Ordering.Infrastructure.Data.Configurations;

/// <summary>
/// Using EF Core ModelBuilder for mapping DDD Entities to EF Core Entities
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasConversion(
                        productId => productId.Value,
                        dbId => ProductId.Of(dbId));

        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
    }
}
