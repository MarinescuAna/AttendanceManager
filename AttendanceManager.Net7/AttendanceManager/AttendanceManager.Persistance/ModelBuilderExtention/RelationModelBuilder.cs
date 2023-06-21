using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance.ModelBuilderExtention
{
    public static class RelationModelBuilder
    {
        public static void SetRelationships(this ModelBuilder modelBuilder)
        {

            // remove the onCascade behavior for Involvement table 
            modelBuilder.Entity<Involvement>()
                .HasOne(a => a.User)
                .WithMany(u => u.Involvements)
                .OnDelete(DeleteBehavior.Restrict);

            // remove the onCascade behavior for Memebr table 
            //modelBuilder.Entity<Member>()
            //    .HasOne(a => a.User)
            //    .WithMany(u => u.)
            //    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
