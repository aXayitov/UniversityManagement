using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace UniversityManagement.ViewModels
{
    public class EnrollmentActionViewModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int AssigmentId { get; set; }
    }
}
