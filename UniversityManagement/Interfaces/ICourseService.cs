using UniversityManagement.Entities;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseViewModel>> GetCoursesAsync(
            int? categoryId,
            string? searchString,
            string? sort);
        Task<Course> GetCourseByIdAsync(int id);
        Task<CourseViewModel> CreateCourseAsync(CourseActionViewModel courseAction);
        Task UpdateCourseAsync(CourseActionViewModel courseAction);
        Task DeleteCourseAsync(int id);
    }
}
