using Newtonsoft.Json;

namespace NLP_WebApp.Models.Dtos;

public class RootDto
{
    [JsonProperty("list")]
    public List<SuggestionArticleDto> Articles { get; set; }
}