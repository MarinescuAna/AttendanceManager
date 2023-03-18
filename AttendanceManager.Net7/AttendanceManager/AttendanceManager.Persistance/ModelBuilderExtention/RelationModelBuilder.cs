using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance.ModelBuilderExtention
{
    public static class RelationModelBuilder
    {
        public static void SetRelationships(this ModelBuilder modelBuilder)
        {
            // remove the onCascade behavior for Specialization table 
            modelBuilder.Entity<Specialization>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Specializations)
                .OnDelete(DeleteBehavior.Cascade);

            // remove the onCascade behavior for Attendance table 
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.User)
                .WithMany(u => u.Attendances)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.AttendanceCollection)
                .WithMany(ac => ac.Attendances)
                .OnDelete(DeleteBehavior.Cascade);

            // remove the onCascade behavior for AttendanceCollection table 
            modelBuilder.Entity<AttendanceCollection>()
                .HasOne(ac => ac.Document)
                .WithMany(doc => doc.AttendanceCollections)
                .OnDelete(DeleteBehavior.Cascade);

            // remove the onCascade behavior for Course table 
            modelBuilder.Entity<Course>()
                .HasOne(c => c.UserSpecialization)
                .WithMany(uc => uc.Courses)
                .OnDelete(DeleteBehavior.Cascade);

            // remove the onCascade behavior for Document table 
            modelBuilder.Entity<Document>()
                .HasOne(doc => doc.Course)
                .WithMany(c => c.Documents)
                .OnDelete(DeleteBehavior.Cascade);

            // remove the onCascade behavior for DocumentMember table 
            modelBuilder.Entity<DocumentMember>()
                .HasOne(dm => dm.Document)
                .WithMany(doc => doc.DocumentMembers)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DocumentMember>()
                .HasOne(dm => dm.User)
                .WithMany(u => u.DocumentMembers)
                .OnDelete(DeleteBehavior.Cascade);

            // remove the onCascade behavior for UserSpecialization table 
            modelBuilder.Entity<UserSpecialization>()
                .HasOne(us => us.Specialization)
                .WithMany(s => s.UserSpecializations)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserSpecialization>()
                .HasOne(dm => dm.User)
                .WithMany(u => u.UserSpecializations)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
