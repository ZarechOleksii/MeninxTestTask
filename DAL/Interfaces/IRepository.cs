using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllNoTrackingAsync();
        IEnumerable<T> GetAllNoTracking();

        bool Create(T obj);

        bool Delete(T obj);

        bool Update(T obj);

        bool UpdateUntracked(T obj);
    }
}
