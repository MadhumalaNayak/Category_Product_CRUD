using Category_Product_CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Category_Product_CRUD.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                List<Category> categoryModel = new List<Category>();
                var categories = await _context.Categories.ToListAsync();
                return View(categories);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                var model = new Category();
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category category = new Category()
                    {
                        CategoryName = model.CategoryName,
                        Description = model.Description,
                    };
                    await _context.Categories.AddAsync(model); ;
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public async Task<IActionResult> Edit(int Id)
        {
            Category category = await _context.Categories.FindAsync(Id);
            if (category == null) return View("NotFound");

            var CategoryModel = new Category
            {
                CategoryId =  category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
            };
            return View(CategoryModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category = await _context.Categories.FindAsync(model.CategoryId);
                    if (category == null) return View("NotFound");

                    category.CategoryName = model.CategoryName;
                    category.Description = model.Description;

                    _context.Entry(category).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            try
            {
                if (Id == 0) return View("NotFound");
                if (Id != null)
                {
                    var category = await _context.Categories.FindAsync(Id);
                    if (category == null)
                    {
                        return View("NotFound");
                    }
                    return View(category);

                }
                else
                {
                    return View("NotFound");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == Id);

            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int Id, IFormCollection collection)
        {
            var category = await _context.Categories.FindAsync(Id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
