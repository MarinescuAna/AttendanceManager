using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance.ModelBuilderExtention
{
    public static class RelationModelBuilder
    {
        public static void SetRelationships(this ModelBuilder modelBuilder)
        {

            // remove the onCascade behavior for Attendance table 
            modelBuilder.Entity<Involvement>()
                .HasOne(a => a.User)
                .WithMany(u => u.Attendances)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
