using UniversityManagement.Entities;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentViewModel>> GetDepartmentsAsync(string? searchString, string? sort);
        Task<Department> GetDepartmentByIdAsync(int id);
        Task<DepartmentViewModel> CreateDepartmentAsync(DepartmentViewModel departmentViewModel);
        Task UpdateDepartmentAsync(DepartmentViewModel departmentViewModel);
        Task DeleteDepartmentAsync(int id);
    }
}
