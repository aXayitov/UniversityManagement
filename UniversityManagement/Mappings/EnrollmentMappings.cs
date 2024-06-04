using System.ComponentModel;
using UniversityManagement.Entities;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Mappings
{
    public static class EnrollmentMappings
    {
        public static EnrollmentViewModel ToViewModel(this Enrollment enrollment)
        {
            return new EnrollmentViewModel
            {
                Id = enrollment.Id,
                StudentId = enrollment.StudentId,
                Student = enrollment.Student.FirstName,
                AssigmentId = enrollment.AssigmentId,
                Assigment = enrollment.Assigment.Id.ToString()
            };
        }
        public static Enrollment ToEntity(this EnrollmentActionViewModel action)
        {
            return new Enrollment
            {
                Id = action.Id,
                StudentId = action.StudentId,
                AssigmentId = action.AssigmentId
            };
        }
    }
}
