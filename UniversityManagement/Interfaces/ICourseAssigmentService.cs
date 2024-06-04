using UniversityManagement.Entities;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Interfaces
{
    public interface ICourseAssigmentService
    {
        Task<IEnumerable<CourseAssigmentViewModel>> GetCourseAssigmentsAsync(int? instructorId, int? courseId, string? searchString, string? sort);
        Task<CourseAssigment> GetCourseAssigmentByIdAsync(int id);
        Task<CourseAssigmentViewModel> CreateCourseAssigmentAsync(CourseAssigmentActionViewModel courseAssigment);
        Task UpdateCourseAssigmentAsync(CourseAssigmentActionViewModel courseAssigment);
        Task DeleteCourseAssigmentAsync(int id);
    }
}
