using UniversityManagement.Entities;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Mappings
{
    public static class DepartmentMappings
    {
        public static DepartmentViewModel ToViewModel(this Department department)
        {
            return new DepartmentViewModel
            {
                Id = department.Id,
                Name = department.Name,
            };
        }
        public static Department ToEntity(this DepartmentViewModel department)
        {
            return new Department
            {
                Id = department.Id,
                Name = department.Name,
            };
        }
    }
}
