using Microsoft.EntityFrameworkCore;
using Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extensions.FluentAPI
{
    public static class EnvironmentTypeFluentAPI
    {
        public static ModelBuilder EnvironmentTypeBuilder(this ModelBuilder builder)
        {
            builder.Entity<EnvironmentType>(entity => 
            {
                entity.Property(x => x.ID).HasColumnName(nameof(EnvironmentType) + "ID");
                entity.HasKey(x => x.ID);

                entity.Property(x => x.Name).HasColumnType("VARCHAR(20)").HasMaxLength(20);
                entity.Property(x => x.NormalizedName).HasColumnType("VARCHAR(20)").HasMaxLength(20);

            });
            return builder;
        }
    }
}
