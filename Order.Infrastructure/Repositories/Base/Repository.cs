using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities.Base;
using Order.Domain.Repositories.Base;
using Order.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly OrderContext _orderContext;
        public Repository(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _orderContext.Set<T>().AddAsync(entity);
            await _orderContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _orderContext.Set<T>().Remove(entity);
            await _orderContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _orderContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await _orderContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disabledTracking = true)
        {
            IQueryable<T> query = _orderContext.Set<T>();
            if(disabledTracking) query = query.AsNoTracking();

            if(!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

            if(predicate != null) query = query.Where(predicate);

            if(orderBy != null)
                return await orderBy(query).ToListAsync();
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _orderContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _orderContext.Entry(entity).State = EntityState.Modified;
            await _orderContext.SaveChangesAsync();
        }
    }
}
