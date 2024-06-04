using UniversityManagement.Entities;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Interfaces
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentViewModel>> GetEnrollmentsAsync(int? studentId, int? courseAssigmentId, string? searchString, string? sort);
        Task<Enrollment> GetEnrollmentByIdAsync(int id);
        Task<EnrollmentViewModel> CreateEnrollmentAsync(EnrollmentActionViewModel enrollmentAction);
        Task UpdateEnrollmentAsync(EnrollmentActionViewModel enrollmentAction);
        Task DeleteEnrollmentAsync(int id);
    }
}
