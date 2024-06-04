using UniversityManagement.Entities;

namespace UniversityManagement.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }

    }
}
