using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly ILogger log;
        protected readonly LibraryDBContext dbContext;

        public BaseRepository(LibraryDBContext context, ILogger logger)
        {
            dbContext = context;
            log = logger;
        }

        protected bool SaveChangesLogExceptionWrapper(Action action)
        {
            try
            {
                action.Invoke();
                dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
            }

            return false;
        }

        public async virtual Task<IEnumerable<T>> GetAllNoTrackingAsync()
        {
            return await dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public virtual IEnumerable<T> GetAllNoTracking()
        {
            return dbContext.Set<T>().AsNoTracking().ToList();
        }

        public virtual bool Create(T obj)
        {
            return SaveChangesLogExceptionWrapper(() => dbContext.Set<T>().Add(obj));
        }

        public virtual bool Delete(T obj)
        {
            return SaveChangesLogExceptionWrapper(() => dbContext.Set<T>().Remove(obj));
        }

        public virtual bool Update(T obj)
        {
            return SaveChangesLogExceptionWrapper(() => dbContext.Set<T>().Update(obj));
        }

        public virtual bool UpdateUntracked(T obj)
        {
            return SaveChangesLogExceptionWrapper(() => dbContext.Entry(obj).State = EntityState.Modified);
        }
    }
}
