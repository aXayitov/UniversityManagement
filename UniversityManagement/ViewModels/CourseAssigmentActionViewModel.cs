using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UniversityManagement.ViewModels
{
    public class CourseAssigmentActionViewModel
    {
        public int Id { get; set; }
        //[Required, MaxLength(25, ErrorMessage = "Name can not be more than 255 characters."),
        //  MinLength(1, ErrorMessage = "Name must be least 1 characters")]
        //[DisplayName("Room")]
        public string Room { get; set; }
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
    }
}
