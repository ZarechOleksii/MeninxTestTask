using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Book GetOne(Guid id);

        Book GetOneWithCategory(Guid id);

        Task<Book> GetOneAsync(Guid id);

        Task<bool> BookExists(Guid id);

        bool IsISBNAvailable(string ISBN);

        Task<IEnumerable<Book>> GetAllAsync(int take, int offset, string search, SortOrder sortOrder);
    }
}
