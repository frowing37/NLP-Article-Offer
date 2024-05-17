using Microsoft.AspNetCore.Mvc;
using NLP_WebApp.Abstract;
using NLP_WebApp.Models.Dtos;
using NLP_WebApp.Models.Entity;

namespace NLP_WebApp.Controllers;

public class ArticleController : Controller
{
    private readonly IInterest _interestService;

    public ArticleController(IInterest interestService) => _interestService = interestService;
    public IActionResult InterestArticle()
    {
        return View();
    }
    
    public IActionResult AllArticle()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult SetInterest()
    {
        var userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var allInterest = _interestService.GetListAll();
        var selfInterest = allInterest.Where(x => x.UserID == Convert.ToUInt32(userId));
        List<InterestDto> dtos = new List<InterestDto>();
        foreach (var interest in selfInterest)
        {
            dtos.Add(new InterestDto()
            {
                ID = interest.ID,
                Word = interest.Word
            });
        }
        return View(dtos);
    }

    [HttpPost]
    public IActionResult SetInterest(InterestDto interest)
    {
        var userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        _interestService.Insert(new Interest()
        {
            UserID = Convert.ToInt32(userId),
            Word = interest.Word
        });

        return View();
    }
    
    public IActionResult DeleteInterest(int ID)
    {
        var interest = _interestService.GetByID(ID);
        _interestService.Delete(interest);

        return RedirectToAction("SetInterest", "Article");
    }
}