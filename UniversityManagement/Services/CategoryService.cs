using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data;
using UniversityManagement.Entities;
using UniversityManagement.Exeptions;
using UniversityManagement.Interfaces;
using UniversityManagement.Mappings;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly UniversityDbContext _context;
        public CategoryService(UniversityDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync(int? parentId, string? searchString, string? sortOrder)
        {
            var query = _context.Categories.Include(c => c.Parent).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x =>
                x.Name.Contains(searchString) ||
                x.Parent.Name.Contains(searchString)||
                x.Description.Contains(searchString));
            }

            if (parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId);
            }

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(x => x.Name),
                "parent_asc" => query.OrderBy(x => x.Parent.Id),
                "parent_desc" => query.OrderByDescending(x => x.Parent.Id),
                _ => query.OrderBy(x => x.Name)
            };

            var categories = await query.Select(x => x.ToViewModel()).ToListAsync();
            return categories;
        }
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            var entity = await _context.Categories.Include(x => x.Parent).FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new EntityNotFoundException($"Category with id {id} does not exist!");
            }

            return entity;
        }
        public async Task<CategoryViewModel> CreateCategoryAsync(CategoryActionViewModel categoryAction)
        {
            var entity = categoryAction.ToEntity();
            var createdCategory = _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            createdCategory.Entity.Parent = await _context.Categories.FirstOrDefaultAsync(x => x.Id == entity.ParentId);

            return createdCategory.Entity.ToViewModel();
        }
        public async Task UpdateCategoryAsync(CategoryActionViewModel categoryAction)
        {
            var entityToUpdate = await _context.Categories.FirstOrDefaultAsync(x => x.Id == categoryAction.Id);

            if (entityToUpdate is null)
            {
                throw new EntityNotFoundException($"Category with id {categoryAction.Id} does not exist!");
            }
            try
            {
                _context.Entry(entityToUpdate).CurrentValues.SetValues(categoryAction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(entityToUpdate.Id))
                {
                    throw new EntityNotFoundException($"Category with id {categoryAction.Id} does not exist!");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task DeleteCategoryAsync(int id)
        {
            var entity = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new EntityNotFoundException($"Category with id {id} does not exist!");
            }

            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
        }
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
