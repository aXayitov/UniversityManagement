namespace UniversityManagement.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<CourseAssigment> Assigments { get; set; }

        public Course()
        {
            Assigments = [];
        }
    }
}
