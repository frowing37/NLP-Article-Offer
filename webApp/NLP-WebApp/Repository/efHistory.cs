using NLP_WebApp.Abstract;
using NLP_WebApp.Models.Data;
using NLP_WebApp.Models.Entity;

namespace NLP_WebApp.Repository;

public class efHistory : GenericRepository<History>, IHistory
{
    public efHistory(Context context) : base(context)
    {
    }
}