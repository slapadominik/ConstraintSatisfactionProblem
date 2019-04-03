using CSP.Entities;

namespace CSP
{
    public interface IDataLoader<T> where T : class 
    {
        T LoadFromFile(string path);
    }
}