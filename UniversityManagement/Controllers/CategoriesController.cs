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
using UniversityManagement.ViewModels;

namespace UniversityManagement.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly UniversityDbContext _context;
        private readonly ICategoryService _service;

        public CategoriesController(UniversityDbContext context, ICategoryService service)
        {
            _context = context;
            _service = service;
        }

        // GET: Categories
        public async Task<IActionResult> Index(int? parentId, string? searchString, string? sortOrder)
        {
            ViewData["NameSort"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["ParentSort"] = sortOrder == "parent_asc" ? "parent_desc" : "parent_asc";

            var categories = await _service.GetCategoriesAsync(parentId, searchString, sortOrder);

            var parentCategories = await _context.Categories
                .Where(x => x.ChildCategories.Any())
                .ToListAsync();
            var selectedParent = parentCategories.FirstOrDefault(x => x.ParentId == parentId);

            ViewBag.ParentCategories = new SelectList(parentCategories, "Id", "Name", selectedParent?.Id);
            ViewBag.Search = searchString;
                
            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _service.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryActionViewModel category)
        {
            if (ModelState.IsValid)
            {
                var createdCategory = await _service.CreateCategoryAsync(category);
                return RedirectToAction(nameof(Details), new {id = createdCategory.Id});
            }
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Id", category.ParentId);
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Id", category.ParentId);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryActionViewModel category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.UpdateCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Id", category.ParentId);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
