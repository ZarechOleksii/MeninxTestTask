using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Book GetOne(Guid id);

        Book GetOneWithCategory(Guid id);

        Task<Book> GetOneAsync(Guid id);

        Task<bool> BookExistsAsync(Guid id);

        bool IsISBNAvailable(string ISBN);

        Task<IEnumerable<Book>> GetAllAsync(int take, int offset, string search, string sortColumn, string sortDirection);
    }
}
