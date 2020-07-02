using Microsoft.EntityFrameworkCore;
using Scaffolds;

namespace Extensions.FluentAPI
{
    public static class LevelTypeFluentAPI
    {
        public static ModelBuilder LevelTypeBuilder(this ModelBuilder builder)
        {
            builder.Entity<LevelType>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID).HasColumnName(nameof(LevelType) + "ID");

                entity.Ignore(x => x.CreatedDate);

                entity.Property(x => x.Name).HasColumnType("VARCHAR(50)").HasMaxLength(50);
                entity.Property(x => x.NormalizedName).HasColumnType("VARCHAR(50)").HasMaxLength(50);
            });

            return builder;
        }
    }
}
