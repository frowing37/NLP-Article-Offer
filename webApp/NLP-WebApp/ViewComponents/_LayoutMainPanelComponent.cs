using Microsoft.AspNetCore.Mvc;

namespace NLP_WebApp.ViewComponents;

public class _LayoutMainPanelComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}