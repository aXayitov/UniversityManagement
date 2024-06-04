using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UniversityManagement.ViewModels
{
    public class InstructorActionViewModel
    {
        public int Id { get; set; }
        [Required, MaxLength(255, ErrorMessage = "Name can not be more than 255 characters."),
            MinLength(3, ErrorMessage = "Name must be least 5 characters")]
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [Required, MaxLength(255, ErrorMessage = "Name can not be more than 255 characters."),
            MinLength(3, ErrorMessage = "Name must be least 5 characters")]
        [DisplayName("Last name")]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage ="Incorrect email format.")]
        public string Email { get; set; }
        [DisplayName("Department")]
        public int DepartmentId { get; set; }
    }
}
