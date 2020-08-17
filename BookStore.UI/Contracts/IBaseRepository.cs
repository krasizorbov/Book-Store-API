namespace BookStore.UI.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Get(string url, int id);
        Task<IList<T>> Get(string url);
        Task<bool> Create(string url, T obj);
        Task<bool> Delete(string url, int id);
        Task<bool> Update(string url, T obj);
    }
}
