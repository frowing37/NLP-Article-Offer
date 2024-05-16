using Microsoft.AspNetCore.Mvc;

namespace NLP_WebApp.ViewComponents;

public class _LayoutScriptComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}