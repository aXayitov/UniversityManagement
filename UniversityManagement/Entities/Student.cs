namespace UniversityManagement.Entities
{
    public class Student
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        public ICollection<Enrollment> Enrollments { get; set; }

        public Student()
        {
            Enrollments = new List<Enrollment>();
        }
    }
}
