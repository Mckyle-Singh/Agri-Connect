using Agri_Connect.Data;
using Agri_Connect.Models.Entities;
using Agri_Connect.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agri_Connect.Controllers
{
    [Authorize(Roles = "Employee")]
    public class FarmersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public FarmersController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterFarmerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Farmer");

                    var farmer = new Farmer
                    {
                        FullName = model.FullName,
                        FarmName = model.FarmName,
                        Location = model.Location,
                        UserId = user.Id
                    };

                    _context.Farmers.Add(farmer);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home"); 
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> List(string searchQuery, string location)
        {
            var query = _context.Farmers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(f => f.FullName.Contains(searchQuery));
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                query = query.Where(f => f.Location == location);
            }

            var farmers = await query.ToListAsync();
            return View(farmers);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Id == id);

            if (farmer == null)
            {
                return NotFound();
            }

            var viewModel = new EditFarmerViewModel
            {
                Id = farmer.Id,
                Email = await _userManager.FindByIdAsync(farmer.UserId) is IdentityUser user ? user.Email : null,
                FullName = farmer.FullName,
                FarmName = farmer.FarmName,
                Location = farmer.Location

            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditFarmerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var farmer = await _context.Farmers.FindAsync(model.Id);

            if (farmer == null)
            {
                return NotFound();
            }

            farmer.FullName = model.FullName;
            farmer.FarmName = model.FarmName;
            farmer.Location = model.Location;

            _context.Entry(farmer).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToAction("List", "Farmers");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var farmer = await _context.Farmers
                .Include(f => f.Products)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (farmer == null)
            {
                TempData["ErrorMessage"] = "Farmer not found.";
                return RedirectToAction("List", "Farmers");
            }

            if (farmer.Products.Any())
            {
                TempData["ErrorMessage"] = "Cannot delete farmer who has existing products.";
                return RedirectToAction("List", "Farmers");
            }


            _context.Farmers.Remove(farmer);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(farmer.UserId);
            if (user != null)
            {
                var userResult = await _userManager.DeleteAsync(user);
                if (!userResult.Succeeded)
                {
                    TempData["ErrorMessage"] = "Farmer deleted, but failed to delete associated user.";
                    return RedirectToAction("List", "Farmers");
                }
            }

            TempData["SuccessMessage"] = $"Farmer '{farmer.FullName}' deleted successfully!";
            return RedirectToAction("List", "Farmers");
        }
    }
}
