using Microsoft.CodeAnalysis.CSharp.Syntax;
using UniversityManagement.Entities;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Mappings
{
    public static class CourseAssigmentsMappings
    {
        public static CourseAssigmentViewModel ToViewModel(this CourseAssigment courseAssigment)
        {
            return new CourseAssigmentViewModel
            {
                Id = courseAssigment.Id,
                Room = courseAssigment.Room,
                CourseId = courseAssigment.Course.Id,
                Course = courseAssigment.Course.Name,
                InstructorId = courseAssigment.InstructorId,
                Instructor = courseAssigment.Instructor.FirstName
            };
        }
        public static CourseAssigment ToEntity(this CourseAssigmentActionViewModel courseAssigment)
        {
            return new CourseAssigment
            {
                Id = courseAssigment.Id,
                Room = courseAssigment.Room,
                CourseId = courseAssigment.CourseId,
                InstructorId = courseAssigment.InstructorId
            };
        }
    }
}
