using UniversityManagement.Entities;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<InstructorViewModel>> GetInstructorsAsync(int? departmentId,string? searchString, string? sort);
        Task<Instructor?> GetInstructorByIdAsync(int id);
        Task<InstructorViewModel> CreateAsync(InstructorActionViewModel instructor);
        Task UpdateAsync(InstructorActionViewModel instructor);
        Task DeleteAsync(int id);
    }
}
