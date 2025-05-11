using Agri_Connect.Data;
using Agri_Connect.Models.Entities;
using Agri_Connect.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        // POST: Products/Add
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
                    return Unauthorized(); // Or redirect to an error page
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
                return RedirectToAction(nameof(List)); // Redirect to list of products
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> List(string searchQuery)
        {
            var userId = _userManager.GetUserId(User);

            if (User.IsInRole("Employee"))
            {
                var allProducts = _context.Products
                    .Include(p => p.Farmer)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    allProducts = allProducts.Where(p => p.ProductName.Contains(searchQuery));
                }

                return View(await allProducts.ToListAsync()); // 👈 Returns List<Product>
            }
            // Else assume the user is a Farmer
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);

            if (farmer == null)
            {
                return Unauthorized(); // or handle it appropriately
            }

            var farmerProducts =  _context.Products
                .Where(p => p.FarmerId == farmer.Id)
                .Include(p => p.Farmer)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                farmerProducts = farmerProducts.Where(p => p.ProductName.Contains(searchQuery));
            }

            return View(await farmerProducts.ToListAsync());
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
                return Unauthorized();

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.FarmerId == farmer.Id);

            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

    }
}
