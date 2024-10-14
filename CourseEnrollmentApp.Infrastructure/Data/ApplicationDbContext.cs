using CourseEnrollmentApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseEnrollmentApp.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseRegistration> CourseRegistrations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseRegistration>()
                .HasKey(cr => new { cr.CourseId, cr.StudentId });

            modelBuilder.Entity<CourseRegistration>()
                .HasOne(cr => cr.Student)
                .WithMany(s => s.CourseRegistrations)
                .HasForeignKey(cr => cr.StudentId);

            modelBuilder.Entity<CourseRegistration>()
                .HasOne(cr => cr.Course)
                .WithMany(c => c.CourseRegistrations)
                .HasForeignKey(cr => cr.CourseId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
