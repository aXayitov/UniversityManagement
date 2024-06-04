using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data;
using UniversityManagement.Entities;
using UniversityManagement.Interfaces;
using UniversityManagement.Mappings;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Controllers
{
    public class CourseAssigmentsController : Controller
    {
        private readonly UniversityDbContext _context;
        private readonly ICourseAssigmentService _service;

        public CourseAssigmentsController(UniversityDbContext context, ICourseAssigmentService service)
        {
            _context = context;
            _service = service;
        }

        // GET: CourseAssigments
        public async Task<IActionResult> Index( int? instructorId, int? courseId, string? searchString, string? sortOrder)
        {
            ViewData["RoomSort"] = sortOrder == "room_asc" ? "room_desc" : "room_asc";

            var courseAssigments = await _service.GetCourseAssigmentsAsync(instructorId, courseId, searchString, sortOrder);

            var instructors = await _context.Instructors.ToListAsync();
            var courses = await _context.Courses.ToListAsync();

            var selectInstructors = instructors.FirstOrDefault(x => x.Id == instructorId);
            var selectCourses = courses.FirstOrDefault(x => x.Id == courseId);

            ViewBag.Instructors = new SelectList(instructors, "Id", "FirstName", selectInstructors?.Id);
            ViewBag.Courses = new SelectList(courses, "Id", "Name", selectCourses?.Id);
            ViewBag.Search = searchString;

            return View(courseAssigments);
        }

        // GET: CourseAssigments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseAssigment = await _service.GetCourseAssigmentByIdAsync(id.Value);
            if (courseAssigment == null)
            {
                return NotFound();
            }

            return View(courseAssigment);
        }

        // GET: CourseAssigments/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id");
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "Id");
            return View();
        }

        // POST: CourseAssigments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseAssigmentActionViewModel courseAssigment)
        {
            if (ModelState.IsValid)
            {
                var createCourseAssigment = _service.CreateCourseAssigmentAsync(courseAssigment);
                return RedirectToAction(nameof(Details), new { id = createCourseAssigment.Id });
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", courseAssigment.CourseId);
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "Id", courseAssigment.InstructorId);
            return View(courseAssigment);
        }

        // GET: CourseAssigments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseAssigment = await _context.CourseAssigments.FindAsync(id);
            if (courseAssigment == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", courseAssigment.CourseId);
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "Id", courseAssigment.InstructorId);
            return View(courseAssigment);
        }

        // POST: CourseAssigments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseAssigmentActionViewModel courseAssigment)
        {
            if (id != courseAssigment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.UpdateCourseAssigmentAsync(courseAssigment);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", courseAssigment.CourseId);
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "Id", courseAssigment.InstructorId);
            return View(courseAssigment);
        }

        // GET: CourseAssigments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseAssigment = await _context.CourseAssigments
                .Include(c => c.Course)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseAssigment == null)
            {
                return NotFound();
            }

            return View(courseAssigment);
        }

        // POST: CourseAssigments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteCourseAssigmentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
