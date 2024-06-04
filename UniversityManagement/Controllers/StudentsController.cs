using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data;
using UniversityManagement.Entities;
using UniversityManagement.Interfaces;
using UniversityManagement.Mappings;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Controllers
{
    public class StudentsController : Controller
    {
        private readonly UniversityDbContext _context;
        private readonly IStudentService _service;

        public StudentsController(UniversityDbContext context, IStudentService service)
        {
            _context = context;
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: Students
        public async Task<IActionResult> Index(string? searchString, string? sortOrder)
        {
            ViewData["NameSort"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["EmailSort"] = sortOrder == "email_asc" ? "email_desc" : "email_asc";

            var students = await _service.GetStudentsAsync(searchString, sortOrder);
            ViewBag.Search = searchString;

            return View(students);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _service.GetStudentByIdAsync(id.Value);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentActionViewModel student)
        {
            if (ModelState.IsValid)
            {
                var students =  await _service.CreateStudentAsync(student);
                return RedirectToAction(nameof(Details), new { id = students.Id });
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentActionViewModel student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.UpdateStudentAsync(student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteStudentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        
    }
}
