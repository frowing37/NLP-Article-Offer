namespace NLP_WebApp.Models.Dtos;

public class InterestforApi
{
    public InterestforApi(string ınterestWord)
    {
        Word = ınterestWord;
    }

    public string Word { get; set; }
}