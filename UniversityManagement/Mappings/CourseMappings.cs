using UniversityManagement.Entities;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Mappings
{
    public static class CourseMappings
    {
        public static CourseViewModel ToViewModel(this Course course)
        {
            return new CourseViewModel
            {
                Id = course.Id,
                Price = course.Price,
                Name = course.Name,
                CategoryId = course.CategoryId,
                Category = course.Category.Name
            };
        }
        public static Course ToEntity(this CourseActionViewModel action)
        {
            return new Course
            {
                Id = action.Id,
                Name = action.Name,
                Price = action.Price,
                CategoryId = action.CategoryId
            };
        }
    }
}
