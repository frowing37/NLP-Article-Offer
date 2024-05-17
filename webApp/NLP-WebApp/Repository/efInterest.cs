using NLP_WebApp.Abstract;
using NLP_WebApp.Models.Data;
using NLP_WebApp.Models.Entity;

namespace NLP_WebApp.Repository;

public class efInterest : GenericRepository<Interest>, IInterest
{
    public efInterest(Context context) : base(context) { }
}