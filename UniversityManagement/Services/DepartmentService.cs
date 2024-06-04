using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data;
using UniversityManagement.Entities;
using UniversityManagement.Exeptions;
using UniversityManagement.Interfaces;
using UniversityManagement.Mappings;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly UniversityDbContext _context;
        public DepartmentService(UniversityDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DepartmentViewModel>> GetDepartmentsAsync(string? searchString, string? sortOrder)
        {
            var query = _context.Departments.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x =>
                x.Name.Contains(searchString));
            }

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(x => x.Name),
                _ => query.OrderBy(x => x.Name)
            };

            var departments = await query.Select(x => x.ToViewModel()).ToListAsync();

            return departments;
        }
        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            var entity = await _context.Departments.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new EntityNotFoundException($"Department with id {id} does not exist!");
            }

            return entity;
        }
        public async Task<DepartmentViewModel> CreateDepartmentAsync(DepartmentViewModel departmentViewModel)
        {
            var entity = departmentViewModel.ToEntity();
            var createdInstructor = _context.Departments.Add(entity);
            await _context.SaveChangesAsync();

            return createdInstructor.Entity.ToViewModel();
        }
        public async Task UpdateDepartmentAsync(DepartmentViewModel departmentView)
        {
            var entityToUpdate = await _context.Departments.FirstOrDefaultAsync(x => x.Id == departmentView.Id);

            if (entityToUpdate is null)
            {
                throw new EntityNotFoundException($"Department with id {departmentView.Id} does not exist!");
            }

            try
            {
                _context.Entry(entityToUpdate).CurrentValues.SetValues(departmentView);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(entityToUpdate.Id))
                {
                    throw new EntityNotFoundException($"Department with id {departmentView.Id} does not exist!");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task DeleteDepartmentAsync(int id)
        {
            var entityToDelete = await _context.Departments.FirstOrDefaultAsync(x => x.Id == id);

            if (entityToDelete is null)
            {
                throw new EntityNotFoundException($"Department with id {id} does not exist!");
            }

            _context.Departments.Remove(entityToDelete);
            await _context.SaveChangesAsync();
        }
        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
