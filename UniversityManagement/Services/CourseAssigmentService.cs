using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using UniversityManagement.Data;
using UniversityManagement.Entities;
using UniversityManagement.Exeptions;
using UniversityManagement.Interfaces;
using UniversityManagement.Mappings;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Services
{
    public class CourseAssigmentService: ICourseAssigmentService
    {
        private readonly UniversityDbContext _context;
        public CourseAssigmentService(UniversityDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CourseAssigmentViewModel>> GetCourseAssigmentsAsync(int? instructorId ,int? courseId, string? searchString, string? sortOrder)
        {
            var query = _context.CourseAssigments.Include(x => x.Course).Include(x => x.Instructor).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x =>
                x.Room.Contains(searchString) ||
                x.Instructor.FirstName.Contains(searchString) ||
                x.Instructor.LastName.Contains(searchString) ||
                x.Course.Name.Contains(searchString));
            }

            if (instructorId.HasValue)
            {
                query = query.Where(x => x.InstructorId == instructorId);
            }

            if (courseId.HasValue)
            {
                query = query.Where(x => x.CourseId == courseId);
            }

            query = sortOrder switch
            {
                "room_desc" => query.OrderBy(x => x.Room),
                _ => query.OrderByDescending(x => x.Room)
            };

            var courseAssigmnets = await query.Select(x => x.ToViewModel()).ToListAsync();

           return courseAssigmnets;
        }
        public async Task<CourseAssigment> GetCourseAssigmentByIdAsync(int id)
        {
            var entity = await _context.CourseAssigments.Include(x => x.Course).Include(x => x.Instructor).FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new EntityNotFoundException($"Course assigment with id {id} does not exist!");
            }

            return entity;
        }
        public async Task<CourseAssigmentViewModel> CreateCourseAssigmentAsync(CourseAssigmentActionViewModel courseAssigment)
        {
            var entity = courseAssigment.ToEntity();
            var createdCourseAssigment = _context.CourseAssigments.Add(entity);
            await _context.SaveChangesAsync();

            return createdCourseAssigment.Entity.ToViewModel();
        }
        public async Task UpdateCourseAssigmentAsync(CourseAssigmentActionViewModel courseAssigment)
        {
            var entityToUpdate = await _context.CourseAssigments.FirstOrDefaultAsync(x => x.Id == courseAssigment.Id);

            if (entityToUpdate is null)
            {
                throw new EntityNotFoundException($"Course assigment with id {courseAssigment.Id} does not exist!");
            }
            try
            {
                _context.Entry(entityToUpdate).CurrentValues.SetValues(courseAssigment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseAssigmentExists(entityToUpdate.Id))
                {
                    throw new EntityNotFoundException($"Course assigment with id {courseAssigment.Id} does not exist!");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task DeleteCourseAssigmentAsync(int id)
        {
            var entity = await _context.CourseAssigments.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new EntityNotFoundException($"Course assigment with id {id} does not exist!");
            }

            _context.CourseAssigments.Remove(entity);
            await _context.SaveChangesAsync();
        }
        private bool CourseAssigmentExists(int id)
        {
            return _context.CourseAssigments.Any(e => e.Id == id);
        }
    }
}
