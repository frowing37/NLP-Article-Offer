namespace NLP_WebApp.Abstract;

public interface IGeneric<T> where T: class
{
    void Insert(T t);

    void Update(T t);

    void Delete(T t);

    T GetByID(int ID);

    List<T> GetListAll();
}