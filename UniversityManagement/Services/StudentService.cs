using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data;
using UniversityManagement.Entities;
using UniversityManagement.Exeptions;
using UniversityManagement.Interfaces;
using UniversityManagement.Mappings;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Services
{
    public class StudentService : IStudentService
    {
        private readonly UniversityDbContext _context;
        public StudentService(UniversityDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<StudentViewModel>> GetStudentsAsync(string? searchString, string? sortOrder)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x =>
                x.FirstName.Contains(searchString) ||
                x.LastName.Contains(searchString) ||
                x.Email.Contains(searchString));
            }

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName),
                "email_asc" => query.OrderBy(x => x.Email),
                "email_desc" => query.OrderByDescending(x => x.Email),
                _ => query.OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
            };

            var students = await query.Select(x => x.ToViewModel()).ToListAsync();
            return students;
        }
        public async Task<Student> GetStudentByIdAsync(int id)
        {
            var entity = await _context.Students.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new EntityNotFoundException($"Student with id {id} does not exist!");
            }

            return entity;
        }
        public async Task<StudentViewModel> CreateStudentAsync(StudentActionViewModel student)
        {
            var entity = student.ToEntity();
            var createdStudent = _context.Students.Add(entity);
            await _context.SaveChangesAsync();

            return createdStudent.Entity.ToViewModel();
        }
        public async Task UpdateStudentAsync(StudentActionViewModel student)
        {
            var entityToUpdate = await _context.Students.FirstOrDefaultAsync(x => x.Id == student.Id);

            if (entityToUpdate is null)
            {
                throw new EntityNotFoundException($"Student with id {student.Id} does not exist!");
            }
            try
            {
                _context.Entry(entityToUpdate).CurrentValues.SetValues(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(entityToUpdate.Id))
                {
                    throw new EntityNotFoundException($"Student with id {student.Id} does not exist!");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task DeleteStudentAsync(int id)
        {
            var entity = await _context.Students.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new EntityNotFoundException($"Student with id {id} does not exist!");
            }

            _context.Students.Remove(entity);
            await _context.SaveChangesAsync();
        }
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
