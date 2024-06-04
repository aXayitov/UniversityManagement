using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using UniversityManagement.Entities;

namespace UniversityManagement.ViewModels
{
    public class CourseActionViewModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [DisplayName("Category")]
        public int CategoryId { get; set; }

    }
}
