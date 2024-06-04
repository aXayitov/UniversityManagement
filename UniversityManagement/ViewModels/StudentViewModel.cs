using System.ComponentModel;

namespace UniversityManagement.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        [DisplayName("Full Name")]
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
