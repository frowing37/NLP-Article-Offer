using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLP_WebApp.Models;
using NLP_WebApp.Models.Dtos;
using NLP_WebApp.Models.Entity;

namespace NLP_WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult LogAndReg()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Mail);

        if (user is null)
            return RedirectToAction("LogAndReg", "Home");

        var result = _signInManager.PasswordSignInAsync(user, loginDto.Password, false,false);

        if (result.Result.Succeeded)
        {
            var redirectUrl = "/Article/SetInterest";

            return Json(new { redirectUrl });
        }
        else
        {
            var redirectUrl = "/Home/Error";

            return Json(new { redirectUrl });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Mail);
        
            if(user is not null)
                return RedirectToAction("LogAndReg", "Home");

            AppUser newUser = new AppUser()
            {
                UserName = registerDto.Name,
                Email = registerDto.Mail
            };
            
            var passwordHasher = new PasswordHasher<AppUser>();
            newUser.PasswordHash = passwordHasher.HashPassword(newUser, registerDto.Password);

            var result = await _userManager.CreateAsync(newUser);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, false);
                return RedirectToAction("SetInterest", "Article");
            }
            else
            {
                return RedirectToAction("LogAndReg", "Home");
            }
        }

        return RedirectToAction("Error", "Home");
    }

    public async Task<IActionResult> ProfilDetail()
    {
        
        
        return View();
    }

    public IActionResult Logout()
    {
        _signInManager.SignOutAsync();
        return RedirectToAction("LogAndReg", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
