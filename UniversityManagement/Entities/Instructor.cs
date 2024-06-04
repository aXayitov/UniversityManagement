using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace UniversityManagement.Entities
{
    public class Instructor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public virtual ICollection<CourseAssigment> Assigments { get; set; }

        public Instructor()
        {
            Assigments = new List<CourseAssigment>();
        }
    }
}
