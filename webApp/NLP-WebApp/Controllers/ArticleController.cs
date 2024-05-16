using Microsoft.AspNetCore.Mvc;

namespace NLP_WebApp.Controllers;

public class ArticleController : Controller
{
    public IActionResult InterestArticle()
    {
        return View();
    }
    
    public IActionResult AllArticle()
    {
        return View();
    }
    
    public IActionResult SetInterest()
    {
        return View();
    }
}