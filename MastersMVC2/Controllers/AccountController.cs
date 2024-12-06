using MastersMVC2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MastersMVC2.DAL.Contexts;
using MastersMVC2.DTO.UserDTO;
using System;

namespace FrontToBack.Controllers
{
    public class AccountController : Controller
    {
        readonly AppDbContext _context;
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        public AccountController(AppDbContext appDBContext, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = appDBContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createUserDto);
            }

            AppUser appUser = new AppUser();
            appUser.FirstName = createUserDto.FirstName;
            appUser.UserName = createUserDto.UserName;
            appUser.LastName = createUserDto.LastName;
            appUser.Email = createUserDto.Email;
            var result = await _userManager.CreateAsync(appUser, createUserDto.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
                return View(createUserDto);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            AppUser? user = (AppUser)_context.Users.FirstOrDefault(u => u.Email == loginUserDto.EmailOrUserName || u.UserName == loginUserDto.EmailOrUserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or password is incorrect.");
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
