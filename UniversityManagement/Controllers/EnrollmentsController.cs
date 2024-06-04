using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using SQLitePCL;
using UniversityManagement.Data;
using UniversityManagement.Entities;
using UniversityManagement.Interfaces;
using UniversityManagement.Mappings;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly UniversityDbContext _context;
        private readonly IEnrollmentService _service;
        public EnrollmentsController(UniversityDbContext context, IEnrollmentService service)
        {
            _context = context;
            _service = service;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index(int? studentId, int? assigmentId, string? searchString, string? sortOrder)
        {
            ViewData["StudentSort"] = sortOrder == "student_asc" ? "student_desc" : "student_asc";
            ViewData["AssigmentSort"] = sortOrder == "assigment_asc" ? "assigment_desc" : "assigment_asc";

            var enrollments = await _service.GetEnrollmentsAsync(studentId, assigmentId, searchString, sortOrder);

            var students = await _context.Students.ToListAsync();
            var assigments = await _context.CourseAssigments.ToListAsync();

            var selectStudent = students.FirstOrDefault(x => x.Id == studentId);
            var selectAssigment = assigments.FirstOrDefault(x => x.Id == assigmentId);

            ViewBag.Students = new SelectList(students, "Id", "FirstName", selectStudent?.Id);
            ViewBag.Assigments = new SelectList(assigments, "Id", "Id", selectAssigment?.Id);
            ViewBag.Search = searchString;

            return View(enrollments);
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _service.GetEnrollmentByIdAsync(id.Value);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewData["AssigmentId"] = new SelectList(_context.CourseAssigments, "Id", "Id");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnrollmentActionViewModel enrollment)
        {
            if (ModelState.IsValid)
            {
                var enroll = await _service.CreateEnrollmentAsync(enrollment);
                return RedirectToAction(nameof(Details), new { id = enroll.Id });
            }
            ViewData["AssigmentId"] = new SelectList(_context.CourseAssigments, "Id", "Id", enrollment.AssigmentId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["AssigmentId"] = new SelectList(_context.CourseAssigments, "Id", "Id", enrollment.AssigmentId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", enrollment.StudentId);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EnrollmentActionViewModel enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.UpdateEnrollmentAsync(enrollment);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssigmentId"] = new SelectList(_context.CourseAssigments, "Id", "Id", enrollment.AssigmentId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Assigment)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteEnrollmentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
