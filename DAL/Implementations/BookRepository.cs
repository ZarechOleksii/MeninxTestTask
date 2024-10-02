using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class BookRepository : BaseRepository<Book>, IBookRepository 
    {
        public BookRepository(LibraryDBContext context, ILogger<BookRepository> logger) : base(context, logger) { }

        public Book GetOne(Guid id)
        {
            return dbContext.Books.Find(id);
        }
        public Book GetOneWithCategory(Guid id)
        {
            return dbContext.Books.Include(q => q.Category).FirstOrDefault(q => q.Id == id);
        }

        public bool IsISBNAvailable(string ISBN)
        {
            return dbContext.Books.FirstOrDefault(q => q.ISBN == ISBN) == null;
        }

        public Task<IEnumerable<Book>> GetAllAsync(int take, int offset, string search, SortOrder sortOrder)
        {
            //stored procedure here
            throw new NotImplementedException();
        }
    }
}
