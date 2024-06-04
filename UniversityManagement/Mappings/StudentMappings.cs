using UniversityManagement.Entities;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Mappings
{
    public static class StudentMappings
    {
        public static StudentViewModel ToViewModel(this Student student)
        {
            return new StudentViewModel
            {
                Id = student.Id,
                FullName = student.FirstName + " " + student.LastName,
                Email = student.Email
            };
        }
        public static Student ToEntity(this StudentActionViewModel student) => new Student
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email
        };
    }
}
