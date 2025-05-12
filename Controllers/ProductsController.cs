using Agri_Connect.Data;
using Agri_Connect.Enums;
using Agri_Connect.Models.Entities;
using Agri_Connect.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Agri_Connect.Controllers
{
    
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Farmer")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = "Farmer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);

                if (farmer == null)
                {
                    return Unauthorized(); 
                }

                var product = new Product
                {
                    ProductName = model.ProductName,
                    Type = model.Type,
                    ProductImageUrl = model.ProductImageUrl,
                    Price = model.Price,
                    Quantity = model.Quantity,
                    Description = model.Description,
                    FarmerId = farmer.Id
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List)); 
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> List(string searchQuery,string type,string farmerId)
        {
            var userId = _userManager.GetUserId(User);

            IQueryable<Product> productQuery;

            if (User.IsInRole("Employee"))
            {
                productQuery = _context.Products.Include(p => p.Farmer);

                // Filter by search query
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    productQuery = productQuery.Where(p => p.ProductName.Contains(searchQuery));
                }

                // Filter by Product Type
                if (!string.IsNullOrWhiteSpace(type))
                {
                    var productType = (ProductType)Enum.Parse(typeof(ProductType), type);
                    productQuery = productQuery.Where(p => p.Type == productType);
                }
                // Filter by farmer (only for employees)
                if (!string.IsNullOrWhiteSpace(farmerId))
                {
                    var farmerIdInt = int.Parse(farmerId);
                    productQuery = productQuery.Where(p => p.FarmerId == farmerIdInt);
                }
                // Fetch all farmers for the dropdown list
                ViewBag.Farmers = await _context.Farmers
                    .Select(f => new SelectListItem
                    {
                        Value = f.Id.ToString(),
                        Text = f.FullName 
                    })
                    .ToListAsync();

                return View(await productQuery.ToListAsync());
            }
            else
            {
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);

                if (farmer == null)
                {
                    return Unauthorized(); 
                }

                productQuery = _context.Products
                    .Where(p => p.FarmerId == farmer.Id)
                    .Include(p => p.Farmer);

                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    productQuery = productQuery.Where(p => p.ProductName.Contains(searchQuery));
                }

                if (!string.IsNullOrWhiteSpace(type))
                {
                    // Filter by Product Type
                    var productType = (ProductType)Enum.Parse(typeof(ProductType), type);
                    productQuery = productQuery.Where(p => p.Type == productType);
                }

            }

            return View(await productQuery.ToListAsync());
        }



        [Authorize(Roles = "Farmer")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);

            if (farmer == null)
                return Unauthorized();

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.FarmerId == farmer.Id);

            if (product == null)
                return NotFound();

            var model = new AddProductViewModel
            {
                ProductName = product.ProductName,
                Type = product.Type,
                ProductImageUrl = product.ProductImageUrl,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description
            };

            return View(model);
        }

        [Authorize(Roles = "Farmer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = _userManager.GetUserId(User);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);

            if (farmer == null)
                return Unauthorized();

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.FarmerId == farmer.Id);

            if (product == null)
                return NotFound();

            product.ProductName = model.ProductName;
            product.Type = model.Type;
            product.ProductImageUrl = model.ProductImageUrl;
            product.Price = model.Price;
            product.Quantity = model.Quantity;
            product.Description = model.Description;

            await _context.SaveChangesAsync();

            return RedirectToAction("List", "Products");
        }

        [Authorize(Roles = "Farmer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);

            if (farmer == null)
            {
                TempData["ErrorMessage"] = "Unauthorized: You don't have permission to delete this product.";
                return RedirectToAction("List", "Products");
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.FarmerId == farmer.Id);

            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found or does not belong to you.";
                return RedirectToAction("List", "Products");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Product '{product.ProductName}' deleted successfully!";
            return RedirectToAction("List", "Products");
        }

    }
}
