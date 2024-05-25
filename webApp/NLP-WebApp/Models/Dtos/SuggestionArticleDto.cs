using Newtonsoft.Json;

namespace NLP_WebApp.Models.Dtos;

public class SuggestionArticleDto
{
    [JsonProperty("articleID")]
    public string ArticleID { get; set; }
    [JsonProperty("samerate")]
    public decimal Samerate { get; set; }
    [JsonProperty("source")]
    public string Source { get; set; }
}