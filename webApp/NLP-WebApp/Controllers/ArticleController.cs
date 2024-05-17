using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLP_WebApp.Abstract;
using NLP_WebApp.Models.Dtos;
using NLP_WebApp.Models.Entity;

namespace NLP_WebApp.Controllers;

public class ArticleController : Controller
{
    private readonly IInterest _interestService;
    private readonly IHttpClientFactory _httpClientFactory;

    public ArticleController(IInterest interestService, IHttpClientFactory httpClientFactory)
    {
        _interestService = interestService;
        _httpClientFactory = httpClientFactory;
    }
    public IActionResult InterestArticle()
    {
        return View();
    }
    
    public async Task<IActionResult> AllArticle()
    {
        var client = _httpClientFactory.CreateClient();
        var responseMessage = await client.GetAsync("http://localhost:8000/api/getAllArticle");

        if(responseMessage.IsSuccessStatusCode)
        {
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ArticleDto>>(jsonData);

            var articles = values.Take(10).ToList();
            
            return View(articles);
        }
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
    public IActionResult SetInterest([FromBody]InterestDto interest)
    {
        var userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        _interestService.Insert(new Interest()
        {
            UserID = Convert.ToInt32(userId),
            Word = interest.Word
        });
        
        var redirectUrl = "/Article/SetInterest";

        return Json(new { redirectUrl });
    }
    
    public IActionResult DeleteInterest(int ID)
    {
        var interest = _interestService.GetByID(ID);
        _interestService.Delete(interest);

        return RedirectToAction("SetInterest", "Article");
    }
}