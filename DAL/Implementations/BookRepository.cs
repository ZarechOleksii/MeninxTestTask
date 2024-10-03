using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private const string GetBooksProcedureName = "GetBooks";

        public BookRepository(LibraryDBContext context, ILogger<BookRepository> logger) : base(context, logger) { }

        public Book GetOne(Guid id)
        {
            return dbContext.Books.Find(id);
        }

        public async Task<Book> GetOneAsync(Guid id)
        {
            return await dbContext.Books.FindAsync(id);
        }

        public async Task<bool> BookExists(Guid id)
        {
            return await dbContext.Books.AnyAsync(q => q.Id == id);
        }

        public Book GetOneWithCategory(Guid id)
        {
            return dbContext.Books.Include(q => q.Category).FirstOrDefault(q => q.Id == id);
        }

        public bool IsISBNAvailable(string ISBN)
        {
            return dbContext.Books.FirstOrDefault(q => q.ISBN == ISBN) == null;
        }

        public async Task<IEnumerable<Book>> GetAllAsync(int take, int offset, string search, string sortColumn, string sortDirection)
        {
            //stored procedure here, it is automaticall converted to dbparameter on interpolation
            return await dbContext.Books
                .FromSqlInterpolated($"[dbo].[GetBooks] @Take = {take}, @Offset = {offset}, @SearchText = {search}, @SortColumn = {sortColumn}, @SortDirection = {sortDirection}")
                .ToListAsync();
        }
    }
}
