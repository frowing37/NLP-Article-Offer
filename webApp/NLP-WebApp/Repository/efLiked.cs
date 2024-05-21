using NLP_WebApp.Abstract;
using NLP_WebApp.Models.Data;
using NLP_WebApp.Models.Entity;

namespace NLP_WebApp.Repository;

public class efLiked : GenericRepository<Liked>, ILiked
{
    public efLiked(Context context) : base(context)
    {
    }
}