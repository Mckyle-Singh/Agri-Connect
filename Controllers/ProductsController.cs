using Agri_Connect.Data;
using Agri_Connect.Models.Entities;
using Agri_Connect.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agri_Connect.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // POST: Products/Add
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
                return RedirectToAction(nameof(Add)); // Redirect to list of products
            }

            return View(model);
        }



    }
}
