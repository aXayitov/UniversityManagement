namespace UniversityManagement.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public int? ParentId { get; set; }
        public virtual Category? Parent { get; set; }

        public virtual ICollection<Course> Courses { get; set;}
        public virtual ICollection<Category> ChildCategories { get; set; }
        
        public Category()
        {
            Courses = new List<Course>();
            ChildCategories = new List<Category>();
        }
    }
}
