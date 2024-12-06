using MastersMVC2.DAL.Contexts;
using MastersMVC2.DTO.UserDTO;
using MastersMVC2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MastersMVC2.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AccountsController : Controller
	{
		readonly AppDbContext _context;
		readonly UserManager<AppUser> _userManager;
		readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public AccountsController(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_context = context;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_userManager = userManager;
		}
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginUserDto loginUserDto)
		{
			AppUser? user = (AppUser?)_context.Users.FirstOrDefault(u => u.Email == loginUserDto.EmailOrUserName || u.UserName == loginUserDto.EmailOrUserName);
			if (user == null)
			{
				ModelState.AddModelError("","Username or password is incorrect.");
				return View();
			}
			await _signInManager.SignInAsync(user, isPersistent: true);
			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
    }
}
