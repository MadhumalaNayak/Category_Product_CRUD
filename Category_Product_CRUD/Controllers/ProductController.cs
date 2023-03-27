using Category_Product_CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Category_Product_CRUD.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                List<Product> productModel = new List<Product>();
                var products = await _context.Products.ToListAsync();
                return View(products);
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
                var model = new Product();
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var categories = GetAllCategory();
                    Product product = new Product()
                    {
                        ProductName = model.ProductName,
                        Description = model.Description,
                        Category = GetCategory(categories),
                    };
                    ViewBag.Category = GetCategory(categories);
                    await _context.Products.AddAsync(product); ;
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        private Category GetCategory(Task<IEnumerable<Category>> categories)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> Edit(int Id)
        {
            Product product = await _context.Products.FindAsync(Id);
            if (product == null) return View("NotFound");

            var ProductModel = new Product
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
            };
            return View(ProductModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Product model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = await _context.Products.FindAsync(model.ProductId);
                    if (product == null) return View("NotFound");

                    product.ProductName = model.ProductName;
                    product.Description = model.Description;
                    product.Category = model.Category;

                    _context.Entry(product).State = EntityState.Modified;
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
                    var product = await _context.Products.FindAsync(Id);
                    if (product == null)
                    {
                        return View("NotFound");
                    }
                    return View(product);

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
            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == Id);

            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int Id, IFormCollection collection)
        {
            var product = await _context.Products.FindAsync(Id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> GetCategory(IEnumerable<Category> categories)
        {
            List<SelectListItem> categoryList = new List<SelectListItem>();

            foreach (var category in categories)
            {
                categoryList.Add(new SelectListItem { Text = category.CategoryName.ToString(), Value = category.CategoryId.ToString() });
            }
            return categoryList;
        }

        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }
    }
}
