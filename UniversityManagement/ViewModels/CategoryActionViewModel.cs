namespace UniversityManagement.ViewModels
{
    public class CategoryActionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }
    }
}
