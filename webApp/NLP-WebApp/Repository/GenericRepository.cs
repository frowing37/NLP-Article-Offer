using NLP_WebApp.Abstract;
using NLP_WebApp.Models.Data;

namespace NLP_WebApp.Repository;

public class GenericRepository<T> : IGeneric<T> where T : class
{
    private readonly Context _context;

    public GenericRepository(Context context) => _context = context;
    
    public void Insert(T t)
    {
        _context.Add(t);
        _context.SaveChanges();
    }

    public void Update(T t)
    {
        _context.Update(t);
        _context.SaveChanges();
    }

    public void Delete(T t)
    {
        _context.Remove(t);
        _context.SaveChanges();
    }

    public T GetByID(int ID)
    {
        return _context.Set<T>().Find(ID);
    }

    public List<T> GetListAll()
    {
        return _context.Set<T>().ToList();
    }
}