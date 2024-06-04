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
    public class CoursesController : Controller
    {
        private readonly UniversityDbContext _context;
        private readonly ICourseService _service;

        public CoursesController(UniversityDbContext context, ICourseService service)
        {
            _context = context;
            _service = service;
        }

        // GET: Courses
        public async Task<IActionResult> Index(int? categoryId, string? searchString, string? sortOrder)
        {
            ViewData["NameSort"] = sortOrder == "name-asc" ? "name-desc" : "name-asc";
            ViewData["CategorySort"] = sortOrder == "category-asc" ? "category-desc" : "category-asc";

            var courses = await _service.GetCoursesAsync(categoryId, searchString, sortOrder);

            var categories = await _context.Categories.ToListAsync();
            var selectCategory = categories.FirstOrDefault(x => x.Id == categoryId);

            ViewBag.Categories = new SelectList(categories, "Id", "Name", selectCategory?.Id);
            ViewBag.Search = searchString;

            return View(courses);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _service.GetCourseByIdAsync(id.Value);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseActionViewModel course)
        {
            if (ModelState.IsValid)
            {
                var createdCourse = await _service.CreateCourseAsync(course);
                return RedirectToAction(nameof(Details), new { id = createdCourse.Id });
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", course.CategoryId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", course.CategoryId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseActionViewModel course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.UpdateCourseAsync(course);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", course.CategoryId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteCourseAsync(id);
            return RedirectToAction(nameof(Index));
        }

       
    }
}
