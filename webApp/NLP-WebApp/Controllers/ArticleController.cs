using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLP_WebApp.Abstract;
using NLP_WebApp.Models.Dtos;
using NLP_WebApp.Models.Entity;

namespace NLP_WebApp.Controllers;

public class ArticleController : Controller
{
    private readonly IInterest _interestService;
    private readonly IHistory _historyService;
    private readonly ILiked _likedService;
    private readonly IHttpClientFactory _httpClientFactory;

    public ArticleController(IInterest interestService,
        IHistory historyService,
        ILiked likedService,
        IHttpClientFactory httpClientFactory)
    {
        _interestService = interestService;
        _historyService = historyService;
        _likedService = likedService;
        _httpClientFactory = httpClientFactory;
    }
    
    [HttpGet]
    public async Task<IActionResult> InterestArticle()
    {
        var userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var myInterests = _interestService.GetListAll().Where(x=>x.UserID == Convert.ToInt32(userId)).Select(x=> x.Word).ToList();
        var myInterest = " ";
        foreach (var iword in myInterests)
        {
            myInterest = myInterest + " " + iword;
        }
        InterestforApi interest = new InterestforApi(myInterest);
        var client = _httpClientFactory.CreateClient();
        var responseforInterestFastText = await client.PostAsJsonAsync("http://localhost:8000/api/forInterestFastText",interest);
        var responseforInterestScibert = await client.PostAsJsonAsync("http://localhost:8000/api/forInterestScibert",interest);

        var jsonDataFastText = await responseforInterestFastText.Content.ReadAsStringAsync();
        var jsonDataScibert = await responseforInterestScibert.Content.ReadAsStringAsync();
        var valuesFastText = JsonConvert.DeserializeObject<RootDto>(jsonDataFastText);
        var valuesScibert = JsonConvert.DeserializeObject<RootDto>(jsonDataScibert);

        List<SuggestionArticleDto> allSuggestions = new List<SuggestionArticleDto>();
        foreach (var sugarticle in valuesScibert.Articles)
        {
            allSuggestions.Add(sugarticle);
        }
        foreach (var sugarticle in valuesFastText.Articles)
        {
            allSuggestions.Add(sugarticle);
        }

        List<ResultSuggestion> results = new List<ResultSuggestion>();

        foreach (var suggestion in allSuggestions)
        {
            var articleID = suggestion.ArticleID;
            var responseArticle = await client.GetAsync($"http://localhost:8000/api/getArticleByID/{articleID}");
            var jsonArticle = await responseArticle.Content.ReadAsStringAsync();
            var valuesArticle = JsonConvert.DeserializeObject<ArticleDto>(jsonArticle);

            var index = 0;
            for (int i = 0; i < valuesArticle.Content.Length; i++)
            {
                if (valuesArticle.Content[i] == '-')
                {
                    if (valuesArticle.Content[i + 1] == '-')
                    {
                        if (valuesArticle.Content[i + 2] == 'A')
                        {
                            index = i;
                            break;
                        }  
                    }
                }
            }

            results.Add(new ResultSuggestion()
            {
                ArticleID = Convert.ToInt32(suggestion.ArticleID),
                Header = valuesArticle.Content.Substring(0, index),
                Content = valuesArticle.Content.Substring(index+3, valuesArticle.Content.Length - index -3),
                CSRate = suggestion.Samerate.ToString(),
                Source = suggestion.Source
            });
        }
        
        return View(results);
    }
    
    public async Task<IActionResult> AllArticle()
    {
        var client = _httpClientFactory.CreateClient();
        var responseMessage = await client.GetAsync("http://localhost:8000/api/getAllArticle");

        if(responseMessage.IsSuccessStatusCode)
        {
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ArticleDto>>(jsonData);

            var articles = values.Take(50).ToList();
            List<ResultArticle> results = new List<ResultArticle>();

            foreach (var value in values)
            {
                var index = 0;
                for (int i = 0; i < value.Content.Length; i++)
                {
                    if (value.Content[i] == '-')
                    {
                        if (value.Content[i + 1] == '-')
                        {
                            if (value.Content[i + 2] == 'A')
                            {
                                index = i;
                                break;
                            }  
                        }
                    }
                }

                var indexid = 0;
                for (int i = 0; i < value.FileName.Length; i++)
                {
                    if (value.FileName[i] == '.')
                    {
                        indexid = i;
                        break;
                    }
                }
                
                
                results.Add(new ResultArticle()
                {
                    ArticleID = value.FileName.Substring(0,indexid),
                    Header = value.Content.Substring(0,index),
                    Content = value.Content.Substring(index+2,value.Content.Length-index-2)
                });
            }
            
            return View(results);
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
    public IActionResult SetInterest([FromBody] InterestDto interest)
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

    [HttpGet]
    public async Task<IActionResult> SuggestionArticle()
    {
        var userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var articleHistory = _historyService.GetListAll().Where(x => x.UserID == Convert.ToInt32(userId))
            .Select(x => x.ArticleID).ToList();
        var articlesLiked = _likedService.GetListAll().Where(x => x.UserID == Convert.ToInt32(userId))
            .Select(x => x.ArticleID).ToList();

        var articlesReferences = new List<string>();
        articlesReferences.AddRange(articleHistory);
        articlesReferences.AddRange(articlesLiked);
        string json = JsonConvert.SerializeObject(articlesReferences);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var client = _httpClientFactory.CreateClient();
        var responseFastText = await client.PostAsync("http://localhost:8000/api/forLikedFastText", content);
        var responseScibert = await client.PostAsync("http://localhost:8000/api/forLikedScibert", content);

        var jsonFastText = await responseFastText.Content.ReadAsStringAsync();
        var jsonScibert = await responseScibert.Content.ReadAsStringAsync();

        var valuesFastText = JsonConvert.DeserializeObject<RootDto>(jsonFastText);
        var valuesScibert = JsonConvert.DeserializeObject<RootDto>(jsonScibert);
        
        List<SuggestionArticleDto> allSuggestions = new List<SuggestionArticleDto>();
        foreach (var sugarticle in valuesScibert.Articles)
        {
            allSuggestions.Add(sugarticle);
        }
        foreach (var sugarticle in valuesFastText.Articles)
        {
            allSuggestions.Add(sugarticle);
        }

        List<ResultSuggestion> results = new List<ResultSuggestion>();

        foreach (var suggestion in allSuggestions)
        {
            var articleID = suggestion.ArticleID;
            var responseArticle = await client.GetAsync($"http://localhost:8000/api/getArticleByID/{articleID}");
            var jsonArticle = await responseArticle.Content.ReadAsStringAsync();
            var valuesArticle = JsonConvert.DeserializeObject<ArticleDto>(jsonArticle);

            var index = 0;
            for (int i = 0; i < valuesArticle.Content.Length; i++)
            {
                if (valuesArticle.Content[i] == '-')
                {
                    if (valuesArticle.Content[i + 1] == '-')
                    {
                        if (valuesArticle.Content[i + 2] == 'A')
                        {
                            index = i;
                            break;
                        }  
                    }
                }
            }

            results.Add(new ResultSuggestion()
            {
                ArticleID = Convert.ToInt32(suggestion.ArticleID),
                Header = valuesArticle.Content.Substring(0, index),
                Content = valuesArticle.Content.Substring(index+3, valuesArticle.Content.Length - index -3),
                CSRate = suggestion.Samerate.ToString(),
                Source = suggestion.Source
            });
        }
        
        return View(results);
    }
    
    public IActionResult DeleteInterest(int id)
    {
        var interest = _interestService.GetByID(id);
        _interestService.Delete(interest);

        return RedirectToAction("SetInterest", "Article");
    }

    public async Task<IActionResult> DetailArticle(int id)
    {
        var userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var control = _historyService.GetListAll()
            .Where(x=>x.UserID.ToString() == userId && x.ArticleID == id.ToString()).ToList();
        if (control.Count == 0)
        {
            History history = new History()
            {
                ArticleID = id.ToString(),
                UserID = Convert.ToInt32(userId)
            };
            _historyService.Insert(history);
        }
        
        var client = _httpClientFactory.CreateClient();
        var body = id.ToString();
        var response = await client.GetAsync($"http://localhost:8000/api/getArticleByID/{body}");
        var json = await response.Content.ReadAsStringAsync();
        var value = JsonConvert.DeserializeObject<ArticleDto>(json);
        
        var index = 0;
        for (int i = 0; i < value.Content.Length; i++)
        {
            if (value.Content[i] == '-')
            {
                if (value.Content[i + 1] == '-')
                {
                    if (value.Content[i + 2] == 'A')
                    {
                        index = i;
                        break;
                    }  
                }
            }
        }

        var indexid = 0;
        for (int i = 0; i < value.FileName.Length; i++)
        {
            if (value.FileName[i] == '.')
            {
                indexid = i;
                break;
            }
        }

        var result = new ResultArticle()
        {
            ArticleID = value.FileName.Substring(0, indexid),
            Header = value.Content.Substring(0, index),
            Content = value.Content.Substring(index, value.Content.Length - index)
        };
        
        
        return View(result);
    }
    public IActionResult LikedArticle(int id)
    {
        var userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var control = _likedService.GetListAll()
            .Where(x => x.UserID.ToString() == userId && x.ArticleID == id.ToString()).ToList();
        if (control.Count == 0)
        {
            Liked liked = new Liked()
            {
                ArticleID = id.ToString(),
                UserID = Convert.ToInt32(userId)
            };
            _likedService.Insert(liked);
        }
        
        return RedirectToAction("InterestArticle","Article");
    }
}