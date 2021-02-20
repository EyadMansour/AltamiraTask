using Domain.Common.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder SetStatusQueryFilter(this ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<MODEL>().HasQueryFilter(p => p.Status != (byte)RecordStatus.Deleted);

            //modelBuilder.Entity<Domain.Entities.System>().HasQueryFilter(p => p.Status != RecordStatus.Deleted);

            return modelBuilder;
        }
    }
}
