namespace NLP_WebApp.Models.Dtos;

public class ResultSuggestion
{
    public int ArticleID { get; set; }
    public string Header { get; set; }
    public string Content { get; set; }
    public string Source { get; set; }
    public string CSRate { get; set; }
}