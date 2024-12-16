using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace SumCalculatorWebAPI
{
    public interface IRepository<T>
    {
        Task Add(T item);
        Task<T> Get(int id);
        Task Delete(int id);
        Task Update(T item);
        Task<bool> Exists(Expression<Func<T, bool>> predicate);
        Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> predicate);

        //Task<IEnumerable<T>> GetAll(); - TODO
    }
}
