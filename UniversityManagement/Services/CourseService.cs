using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data;
using UniversityManagement.Entities;
using UniversityManagement.Exeptions;
using UniversityManagement.Interfaces;
using UniversityManagement.Mappings;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Services
{
    public class CourseService : ICourseService
    {
        private readonly UniversityDbContext _context;
        public CourseService(UniversityDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CourseViewModel>> GetCoursesAsync(int? categoryId ,string? searchString, string? sortOrder)
        {
            var query = _context.Courses.Include(x => x.Category).AsQueryable();

            if(!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x =>
                x.Name.Contains(searchString) ||
                x.Category.Name.Contains(searchString));
            }

            if(categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            query = sortOrder switch
            {
                "name-desc" => query.OrderByDescending(x => x.Name),
                "category-asc" => query.OrderBy(x => x.Category.Name),
                "cagtegory-desc" => query.OrderByDescending(x => x.Category.Name),
                _ => query.OrderBy(x => x.Name)
            };

            var courses = await query.Select(x => x.ToViewModel()).ToListAsync();
            return courses;
        }
        public async Task<Course> GetCourseByIdAsync(int id)
        {
            var entity = await _context.Courses.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new EntityNotFoundException($"Course with id {id} does not exist!");
            }

            return entity;
        } 
        public async Task<CourseViewModel> CreateCourseAsync(CourseActionViewModel courseActionViewModel)
        {
            var entity = courseActionViewModel.ToEntity();
            var createdCourse = _context.Courses.Add(entity);
            await _context.SaveChangesAsync();
            createdCourse.Entity.Category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == entity.CategoryId);

            return createdCourse.Entity.ToViewModel();
        }
        public async Task UpdateCourseAsync(CourseActionViewModel courseAction)
        {
            var entityToUpdate = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseAction.Id);

            if (entityToUpdate is null)
            {
                throw new EntityNotFoundException($"Course with id {courseAction.Id} does not exist!");
            }
            try
            {
                _context.Entry(entityToUpdate).CurrentValues.SetValues(courseAction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(entityToUpdate.Id))
                {
                    throw new EntityNotFoundException($"Instructor with id {courseAction.Id} does not exist!");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task DeleteCourseAsync(int id)
        {
            var entity = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new EntityNotFoundException($"Course with id {id} does not exist!");
            }

            _context.Courses.Remove(entity);
            await _context.SaveChangesAsync();
        }
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
