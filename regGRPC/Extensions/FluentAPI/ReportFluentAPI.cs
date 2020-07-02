using Microsoft.EntityFrameworkCore;
using Scaffolds;

namespace Extensions.FluentAPI
{
    public static class ReportFluentAPI
    {
        public static ModelBuilder ReportModelBuilder(this ModelBuilder builder)
        {
            builder.Entity<Report>(entity => 
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID).HasColumnName(nameof(Report) + "ID");
                entity.Property(x => x.Events).IsRequired();
                entity.Property(x => x.LevelTypeID).IsRequired();
                entity.Property(x => x.ReportDescription).HasColumnType("VARCHAR(3000)").HasMaxLength(3000).IsRequired();
                entity.Property(x => x.ReportSource).HasColumnType("VARCHAR(1000)").HasMaxLength(1000).IsRequired();
                entity.Property(x => x.Title).HasColumnType("VARCHAR(500)").HasMaxLength(500).IsRequired();
                entity.Property(x => x.LevelTypeID).IsRequired();

                entity.Ignore(x => x.CreatedDate);

                entity.HasOne(x => x.LevelType).WithMany(x => x.Reports).HasForeignKey(x => x.LevelTypeID).OnDelete(DeleteBehavior.Restrict);
            });
            return builder;
        }
    }
}
