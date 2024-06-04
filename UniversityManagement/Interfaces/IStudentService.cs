using UniversityManagement.Entities;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentViewModel>> GetStudentsAsync(string? searchString, string? sort);
        Task<Student> GetStudentByIdAsync(int id);
        Task<StudentViewModel> CreateStudentAsync(StudentActionViewModel student);
        Task UpdateStudentAsync(StudentActionViewModel student);
        Task DeleteStudentAsync(int id);
    }
}
