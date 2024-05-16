using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLP_WebApp.Models.Entity;

namespace NLP_WebApp.ViewComponents;

public class _LayoutSidebarComponent : ViewComponent
{
    private readonly UserManager<AppUser> _userManager;

    public _LayoutSidebarComponent(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.FindByIdAsync(userId);
        
        return View(user);
    }
}