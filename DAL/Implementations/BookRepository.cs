using DAL.Interfaces;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class BookRepository : BaseRepository<Book>, IBookRepository 
    {
        public BookRepository(LibraryDBContext context, ILogger<BookRepository> logger) : base(context, logger) { }

        public async Task<Book> GetOneAsync(Guid id)
        {
            return await dbContext.Books.FindAsync(id);
        }

        public Task<IEnumerable<Book>> GetAllAsync(int take, int offset, string search, SortOrder sortOrder)
        {
            //stored procedure here
            throw new NotImplementedException();
        }
    }
}
