namespace UniversityManagement.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public int AssigmentId { get; set; }
        public virtual CourseAssigment Assigment { get; set; }  
    }
}
