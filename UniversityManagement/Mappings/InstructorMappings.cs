using UniversityManagement.Entities;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Mappings
{
    public static class InstructorMappings
    {
        public static InstructorViewModel ToViewModel(this Instructor instructor)
        {
            return new InstructorViewModel
            {
                id = instructor.Id,
                FullName = instructor.FirstName + " " + instructor.LastName,
                Email = instructor.Email,
                DepartmentId = instructor.DepartmentId,
                Department = instructor.Department.Name
            };
        }
        public static Instructor ToEntity(this InstructorActionViewModel viewModel)
        {
            return new Instructor
            {
                Id = viewModel.Id,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Email = viewModel.Email,
                DepartmentId = viewModel.DepartmentId
            };
        }
    }
}
