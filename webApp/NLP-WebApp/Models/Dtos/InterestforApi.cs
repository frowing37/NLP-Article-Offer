namespace NLP_WebApp.Models.Dtos;

public class InterestforApi
{
    public InterestforApi(string ınterestWord)
    {
        InterestWord = ınterestWord;
    }

    public string InterestWord { get; set; }
}