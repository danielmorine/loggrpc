using Microsoft.EntityFrameworkCore;
using Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extensions.FluentAPI
{
    public static class RegistrationProcessFluentAPI
    {
        public static ModelBuilder RegistrationProcessBuilder(this ModelBuilder builder)
        {
            builder.Entity<RegistrationProcess>(entity => 
            {
                entity.Property(x => x.ID).HasColumnName(nameof(RegistrationProcess) + "ID").IsRequired();
                entity.HasKey(x => x.ID);

                entity.Property(x => x.EnvironmentTypeID).IsRequired();
                entity.Property(x => x.OwnerID).IsRequired();
                entity.Property(x => x.ReportID).IsRequired();
                entity.Property(x => x.CreatedDate).IsRequired();

                entity.HasOne(x => x.EnvironmentType).WithMany(x => x.RegistrationProcess).HasForeignKey(x => x.EnvironmentTypeID).OnDelete(DeleteBehavior.Restrict);
            });
            return builder;
        }
    }
}
