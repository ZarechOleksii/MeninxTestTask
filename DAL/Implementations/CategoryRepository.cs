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
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(LibraryDBContext context, ILogger<CategoryRepository> logger) : base(context, logger) { }

        public async Task<Category> GetOneAsync(Guid id)
        {
            return await dbContext.Categories.FindAsync(id);
        }

        public override IEnumerable<Category> GetAllNoTracking()
        {
            return dbContext.Categories.OrderBy(q => q.Name).AsNoTracking().ToList();
        }
    }
}
