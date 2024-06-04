using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data;
using UniversityManagement.Entities;
using UniversityManagement.Exeptions;
using UniversityManagement.Interfaces;
using UniversityManagement.Mappings;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly UniversityDbContext _context;
        public EnrollmentService(UniversityDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<EnrollmentViewModel>> GetEnrollmentsAsync(int? studentId, int? courseAssigmentId, string? searchString, string? sort)
        {
            var query = _context.Enrollments.Include(x => x.Assigment).Include(x => x.Student).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x =>
                x.Student.FirstName.Contains(searchString) ||
                x.Student.LastName.Contains(searchString));
            }

            if (studentId.HasValue)
            {
                query = query.Where(x => x.StudentId == studentId);
            }

            if (courseAssigmentId.HasValue)
            {
                query = query.Where(x => x.AssigmentId == courseAssigmentId);
            }

            query = sort switch
            {
                "student_desc" => query.OrderByDescending(x => x.Student.FirstName).ThenByDescending(x => x.Student.LastName),
                "assigment_asc" => query.OrderBy(x => x.AssigmentId),
                "assigment_desc" => query.OrderByDescending(x => x.AssigmentId),
                _ => query.OrderBy(x => x.StudentId).ThenBy(x => x.Student.LastName),
            };

            var enrollments = await query.Select(x => x.ToViewModel()).ToListAsync();

            return enrollments;
        }
        public async Task<Enrollment> GetEnrollmentByIdAsync(int id)
        {
            var entity = await _context.Enrollments.Include(x => x.Student).Include(x => x.Assigment).FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new EntityNotFoundException($"Enrollment with id {id} does not exist!");
            }

            return entity;
        }
        public async Task<EnrollmentViewModel> CreateEnrollmentAsync(EnrollmentActionViewModel enrollmentAction)
        {
            var entity = enrollmentAction.ToEntity();
            var createdEnrollment = _context.Enrollments.Add(entity);
            await _context.SaveChangesAsync();
            createdEnrollment.Entity.Assigment = await _context.CourseAssigments.FirstOrDefaultAsync(x => x.Id == entity.AssigmentId);
            createdEnrollment.Entity.Student = await _context.Students.FirstOrDefaultAsync(x => x.Id == entity.StudentId);

            return createdEnrollment.Entity.ToViewModel();
        }
        public async Task UpdateEnrollmentAsync(EnrollmentActionViewModel enrollmentAction)
        {
            var entityToUpdate = await _context.Enrollments.FirstOrDefaultAsync(x => x.Id == enrollmentAction.Id);

            if (entityToUpdate is null)
            {
                throw new EntityNotFoundException($"Enrolment with id {enrollmentAction.Id} does not exist!");
            }
            try
            {
                _context.Entry(entityToUpdate).CurrentValues.SetValues(enrollmentAction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(entityToUpdate.Id))
                {
                    throw new EntityNotFoundException($"Enrollment with id {enrollmentAction.Id} does not exist!");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task DeleteEnrollmentAsync(int id)
        {
            var entity = await _context.Enrollments.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new EntityNotFoundException($"Enrollment with id {id} does not exist!");
            }

            _context.Enrollments.Remove(entity);
            await _context.SaveChangesAsync();
        }
        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.Id == id);
        }
    }
}
