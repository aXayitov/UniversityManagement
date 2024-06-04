using UniversityManagement.Entities;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync(int? parentId, string? searchString, string? sort);
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<CategoryViewModel> CreateCategoryAsync(CategoryActionViewModel categoryAction);
        Task UpdateCategoryAsync(CategoryActionViewModel categoryAction);
        Task DeleteCategoryAsync(int id);
    }
}
