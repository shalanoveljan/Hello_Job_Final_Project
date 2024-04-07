using Entities.Common;
using HelloJob.Data.DAL.Interfaces;
using HelloJob.Data.DBContexts.SQLSERVER;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Data.DAL.Implementations
{
    public class EFRepositoryBase<T>:IRepositoryBase<T> where T : BaseEntity
    {
        readonly HelloJobDbContext _context;

        public EFRepositoryBase(HelloJobDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, params string[] Includes)
        {
            var query = _context.Set<T>().Where(expression);

            if (Includes != null)
            {
                foreach (string include in Includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public IQueryable<T> GetQuery(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public async Task RemoveAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
           await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }

}
