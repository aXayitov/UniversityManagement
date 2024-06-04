using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UniversityManagement.Entities;

namespace UniversityManagement.Data
{
    public class UniversityDbContext : DbContext
    {
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseAssigment> CourseAssigments { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        public UniversityDbContext(DbContextOptions<UniversityDbContext> options) 
            : base(options) 
        { 
        }
    }
}
