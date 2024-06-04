
using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data;
using UniversityManagement.Entities;
using UniversityManagement.Exeptions;
using UniversityManagement.Interfaces;
using UniversityManagement.Mappings;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Services
{
    public class InstructorsService : IInstructorService
    {
        private readonly UniversityDbContext _context;
        public InstructorsService(UniversityDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<InstructorViewModel>> GetInstructorsAsync(int? departmentId, string? searchString, string? sort)
        {
            var query = _context.Instructors.Include(x => x.Department).AsQueryable();

            if (departmentId.HasValue)
            {
                query = query.Where(x => x.DepartmentId == departmentId);
            }

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x =>
                 x.FirstName.Contains(searchString)||
                 x.LastName.Contains(searchString)||
                 x.Email.Contains(searchString)||
                 x.Department.Name.Contains(searchString));
            }

            query = sort switch
            {
                "FullName_asc" => query.OrderBy(x => x.FirstName).ThenBy(x => x.LastName),
                "FullName_desc" => query.OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName),
                "email_asc" => query.OrderBy(x => x.Email),
                "email_desc" => query.OrderByDescending(x => x.Email),
                _ => query.OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
            };

            var instructors = await query.Select(x => x.ToViewModel()).ToListAsync();

            return instructors;
        }
        public async Task<Instructor?> GetInstructorByIdAsync(int id)
        {
            var entity = await _context.Instructors.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);

            if(entity is null)
            {
                throw new EntityNotFoundException($"Instructor with id {id} does not exist!");
            }

            return entity;
        }
        public async Task<InstructorViewModel> CreateAsync(InstructorActionViewModel instructor)
        {
            var entity = instructor.ToEntity();
            var createdInstructor = _context.Instructors.Add(entity);
            await _context.SaveChangesAsync();
            createdInstructor.Entity.Department = await _context.Departments.FirstOrDefaultAsync(x => x.Id == entity.DepartmentId);

            return createdInstructor.Entity.ToViewModel();
        }
        public async Task UpdateAsync(InstructorActionViewModel instructor)
        {
            var entityToUpdate = await _context.Instructors.FirstOrDefaultAsync(x => x.Id == instructor.Id);

            if(entityToUpdate is null)
            {
                throw new EntityNotFoundException($"Instructor with id {instructor.Id} does not exist!");
            }
            try
            {
                _context.Entry(entityToUpdate).CurrentValues.SetValues(instructor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructorExists(entityToUpdate.Id))
                {
                    throw new EntityNotFoundException($"Instructor with id {instructor.Id} does not exist!");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Instructors.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new EntityNotFoundException($"Instructor with id {id} does not exist!");
            }

            _context.Instructors.Remove(entity);
            await _context.SaveChangesAsync();
        }
        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.Id == id);
        }
    }
}
