using MastersMVC2.DAL.Contexts;
using MastersMVC2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MastersMVC2.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class OrdersController : Controller
    {
        readonly AppDbContext _context;
        public OrdersController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Order> order = await _context.Orders.ToListAsync();
            return View(order);
        }
        public async Task<IActionResult> Info(int Id)
        {
            Order? order = await _context.Orders.FirstOrDefaultAsync(m => m.Id == Id);
            if (order == null)
            {
                return NotFound("Something went wrong");
            }
            return View(order);
        }
        public IActionResult Create()
        {
            ViewBag.Services = _context.Services;
            ViewBag.Masters = _context.Masters;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }
            order.CreatedAt = DateTime.Now;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Orders");
        }

        public async Task<IActionResult> SoftDelete(int Id)
        {
            Order? deletedOrder = await _context.Orders.FirstOrDefaultAsync(m => m.Id == Id);
            if (deletedOrder == null)
            {
                return NotFound("Something went wrong");
            }
            deletedOrder.isActive = false;
            deletedOrder.DeletedAt = DateTime.Now;
            _context.Orders.Update(deletedOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Orders");
        }

        public async Task<IActionResult> HardDelete(int Id)
        {
            Order? deletedOrder = await _context.Orders.FirstOrDefaultAsync(m => m.Id == Id);
            if (deletedOrder == null)
            {
                return NotFound("Something went wrong");
            }
            _context.Orders.Remove(deletedOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Orders");
        }

        public async Task<IActionResult> Edit(int Id)
        {
            Order? updatedOrder = await _context.Orders.FirstOrDefaultAsync(m => m.Id == Id);
            if (updatedOrder == null)
            {
                return NotFound("Something went wrong");
            }
            ViewBag.Services = _context.Services;
            return View(updatedOrder);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(Order updatedOrder)
        {
            Order? currentOrder = await _context.Orders.FirstOrDefaultAsync(m => m.Id == updatedOrder.Id);
            if (currentOrder == null)
            {
                return NotFound("Not found");
            }
            if (!currentOrder.isActive)
            {
                return BadRequest("Item can't edited.");
            }
            updatedOrder.CreatedAt = currentOrder.CreatedAt;
            updatedOrder.UpdatedAt = DateTime.Now;
            _context.Update(updatedOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Orders");
        }

        public async Task<IActionResult> RevertSoftDelete(int Id)
        {
            Order? order = await _context.Orders.FirstOrDefaultAsync(m => m.Id == Id);
            if (order == null)
            {
                return NotFound("Not found");
            }
            if (!order.isActive)
            {
                order.UpdatedAt = DateTime.Now;
                order.isActive = true;
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Orders");
        }
    }
}
