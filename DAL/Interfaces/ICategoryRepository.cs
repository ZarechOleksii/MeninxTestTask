using Models;
using System;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetOneAsync(Guid id);
    }
}
