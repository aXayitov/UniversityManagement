using UniversityManagement.Entities;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Mappings
{
    public static class CategoryMappings
    {
        public static CategoryViewModel ToViewModel(this Category category) => new CategoryViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ParentId = category.ParentId,
            Parent = category.Parent?.Name
        };

        public static Category ToEntity(this CategoryActionViewModel categoryAction) => new Category
        {
            Id = categoryAction.Id,
            Name = categoryAction.Name,
            Description = categoryAction.Description,
            ParentId = categoryAction.ParentId
        };
    }
}
