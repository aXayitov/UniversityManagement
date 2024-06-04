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
    public class DepartmentsController : Controller
    {
        private readonly UniversityDbContext _context;
        private readonly IDepartmentService _service;
        public DepartmentsController(UniversityDbContext context, IDepartmentService service)
        {
            _context = context;
            _service = service;
        }

        // GET: Departments
        public async Task<IActionResult> Index(string? searchString, string? sortOrder)
        {
            ViewData["NameSort"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";

            var departments = await _service.GetDepartmentsAsync(searchString, sortOrder);
            
            ViewBag.Search = searchString;
            return View(departments);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _service.GetDepartmentByIdAsync(id.Value);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel department)
        {
            if (ModelState.IsValid)
            {
                var createdDepartment = await _service.CreateDepartmentAsync(department);
                return RedirectToAction(nameof(Details), new { id = createdDepartment.Id });
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentViewModel department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.UpdateDepartmentAsync(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteDepartmentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
