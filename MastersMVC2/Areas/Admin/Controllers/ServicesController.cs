using MastersMVC2.DAL.Contexts;
using MastersMVC2.DTO.ServiceDTO;
using MastersMVC2.Models;
using MastersMVC2.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MastersMVC2.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class ServicesController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _webHostEnvironment;
        public ServicesController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Service> service = await _context.Services.ToListAsync();
            return View(service);
        }
        public async Task<IActionResult> Info(int Id)
        {
            Service? service = await _context.Services.FirstOrDefaultAsync(m => m.Id == Id);
            if (service == null)
            {
                return NotFound("Something went wrong");
            }
            return View(service);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            string? imagePath = null;

            if (dto.ImageFile != null)
            {
                try
                {
                    imagePath = await dto.ImageFile.SaveFileAsync(_webHostEnvironment, "uploads/services", 5 * 1024 * 1024);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ImageFile", ex.Message);
                    return View(dto);
                }
            }

            var service = new Service
            {
                Title = dto.Title,
                Description = dto.Description,
                ImagePath = imagePath,
                CreatedAt = DateTime.Now
            };

            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Services");
        }

        public async Task<IActionResult> SoftDelete(int Id)
        {
            Service? deletedService = await _context.Services.FirstOrDefaultAsync(m => m.Id == Id);
            if (deletedService == null)
            {
                return NotFound("Something went wrong!");
            }

            bool hasOrders = await _context.Orders.AnyAsync(o=>o.ServiceId == Id);
            bool hasMasters = _context.Orders.Any(o => o.ServiceId == Id && o.MasterId != null);
            if (hasOrders || hasMasters)
            {
                return BadRequest("Can't delete this service.");
            }

            deletedService.isActive = false;
            deletedService.DeletedAt = DateTime.Now;
            _context.Services.Update(deletedService);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Services");
        }

        public async Task<IActionResult> HardDelete(int Id)
        {
            Service? deletedService = await _context.Services.FirstOrDefaultAsync(m => m.Id == Id);
            if (deletedService == null)
            {
                return NotFound("Something went wrong");
            }

            bool hasOrders = await _context.Orders.AnyAsync(o => o.ServiceId == Id);
            bool hasMasters = _context.Orders.Any(o => o.ServiceId == Id && o.MasterId != null);
            if (hasOrders || hasMasters)
            {
                return BadRequest("Can't delete this service.");
            }

            _context.Services.Remove(deletedService);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Services");
        }

        public async Task<IActionResult> DeleteImage(int id)
        {
            Service? service = await _context.Services.FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound("Service not found.");
            }
            if (!string.IsNullOrEmpty(service.ImagePath))
            {
                string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, service.ImagePath);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                service.ImagePath = null;
                service.UpdatedAt = DateTime.Now;

                _context.Services.Update(service);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Services");
        }

        public async Task<IActionResult> Edit(int Id)
        {
            Service? updatedService = await _context.Services.FirstOrDefaultAsync(m => m.Id == Id);
            if (updatedService == null)
            {
                return NotFound("Something went wrong");
            }
            ViewBag.Services = _context.Services;
            UpdateServiceDTO dto = new()
            {
                Id = updatedService.Id,
                Title = updatedService.Title,
                Description = updatedService.Description,
                ImagePath = updatedService.ImagePath
            };
            return View(dto);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(Service updatedService)
        {
            Service? currentService = await _context.Services.FirstOrDefaultAsync(m => m.Id == updatedService.Id);
            if (currentService == null)
            {
                return NotFound("Not found");
            }
            if (!currentService.isActive)
            {
                return BadRequest("Item cannot edited.");
            }
            updatedService.CreatedAt = currentService.CreatedAt;
            updatedService.UpdatedAt = DateTime.Now;
            _context.Update(updatedService);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Services");
        }

        public async Task<IActionResult> RevertSoftDelete(int Id)
        {
            Service? service = await _context.Services.FirstOrDefaultAsync(m => m.Id == Id);
            if (service == null)
            {
                return NotFound("Not found");
            }
            if (!service.isActive)
            {
                service.UpdatedAt = DateTime.Now;
                service.isActive = true;
                _context.Update(service);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Services");
        }
    }
}
