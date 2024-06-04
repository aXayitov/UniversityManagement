using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.Configuration;
using NuGet.Protocol;
using UniversityManagement.Data;
using UniversityManagement.Entities;
using UniversityManagement.Interfaces;
using UniversityManagement.Mappings;
using UniversityManagement.Services;
using UniversityManagement.ViewModels;

namespace UniversityManagement.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly UniversityDbContext _context;
        private readonly IInstructorService _service;

        public InstructorsController(UniversityDbContext context, IInstructorService service)
        {
            _context = context;
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: Instructors
        public async Task<IActionResult> Index(string? searchString, int? departmentId, string? sortOrder)
        {
            ViewData["NameSort"] = sortOrder == "FullName_asc" ? "FullName_desc" : "FullName_asc";
            ViewData["EmailSort"] = sortOrder == "email_asc" ? "email_desc" : "email_asc";

            var instructors = await _service.GetInstructorsAsync(departmentId, searchString, sortOrder);

            var departments = await _context.Departments.ToListAsync();
            var selectedDepartment = departments.FirstOrDefault(x => x.Id == departmentId);

            ViewBag.Departments = new SelectList(departments, "Id", "Name", selectedDepartment?.Id);
            ViewBag.Search = searchString;

            return View(instructors);
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _service.GetInstructorByIdAsync(id.Value);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }
        
        // GET: Instructors/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id");
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InstructorActionViewModel instructor)
        {
            if (ModelState.IsValid)
            {
                var createdInstructor = await _service.CreateAsync(instructor);
                return RedirectToAction(nameof(Details), new {Id = createdInstructor.id});
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", instructor.DepartmentId);
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", instructor.DepartmentId);
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InstructorActionViewModel instructor)
        {
            if (id != instructor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(instructor);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", instructor.DepartmentId);
            return View(instructor);
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        
    }
}
