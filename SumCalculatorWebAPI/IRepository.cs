using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace SumCalculatorWebAPI
{
    public interface IRepository<T>
    {
        Task Add(T item);
        Task<T> Get(int id);
        Task Delete(int id);
        Task Update(T item);

    }
}
