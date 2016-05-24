using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CFM.Data;
using CFM.Infrastructure.Interfaces;
using Mehdime.Entity;

namespace CFM.Infrastructure.Base
{
    public class BaseService<TObject> : IBaseService<TObject> where TObject : class
    {
        protected readonly IAmbientDbContextLocator ContextLocator;

        public BaseService(IAmbientDbContextLocator contextLocator)
        {
            ContextLocator = contextLocator;
        }

        public ICollection<TObject> GetAll()
        {
            return ContextLocator.Get<CfmDbContext>().Set<TObject>().ToList();
        }

        public async Task<ICollection<TObject>> GetAllAsync()
        {
            return await ContextLocator.Get<CfmDbContext>().Set<TObject>().ToListAsync().ConfigureAwait(false);
        }

        public TObject Get(int id)
        {
            return ContextLocator.Get<CfmDbContext>().Set<TObject>().Find(id);
        }

        public TObject Get(int id, params Expression<Func<TObject, object>>[] includeProperties)
        {
            foreach (var property in includeProperties)
            {
                ContextLocator.Get<CfmDbContext>().Set<TObject>().Include(property);
            }
            return ContextLocator.Get<CfmDbContext>().Set<TObject>().Find(id);
        }

        public async Task<TObject> GetAsync(int id)
        {
            return await ContextLocator.Get<CfmDbContext>().Set<TObject>().FindAsync(id).ConfigureAwait(false);
        }

        public async Task<TObject> GetAsync(int id, params Expression<Func<TObject, object>>[] includeProperties)
        {
            var query = ContextLocator.Get<CfmDbContext>().Set<TObject>();
            IQueryable<TObject> includes = null;
            query.Find(id);
            foreach (var property in includeProperties)
            {
                includes = query.Include(property);
            }
            return await includes.FirstAsync().ConfigureAwait(false);
        }

        public TObject Find(Expression<Func<TObject, bool>> match)
        {
            return ContextLocator.Get<CfmDbContext>().Set<TObject>().FirstOrDefault(match);
        }

        public TObject Find(Expression<Func<TObject, bool>> match, params Expression<Func<TObject, object>>[] includeProperties)
        {
            foreach (var property in includeProperties)
            {
                ContextLocator.Get<CfmDbContext>().Set<TObject>().Include(property);
            }
            return ContextLocator.Get<CfmDbContext>().Set<TObject>().FirstOrDefault(match);
        }

        public async Task<TObject> FindAsync(Expression<Func<TObject, bool>> match)
        {
            return await ContextLocator.Get<CfmDbContext>().Set<TObject>().FirstOrDefaultAsync(match).ConfigureAwait(false);
        }

        public async Task<TObject> FindAsync(Expression<Func<TObject, bool>> match, params Expression<Func<TObject, object>>[] includeProperties)
        {
            var query = ContextLocator.Get<CfmDbContext>().Set<TObject>().AsQueryable();
            query = includeProperties.Aggregate(query, (current, property) => current.Include(property));
            return await query.FirstOrDefaultAsync(match);
        }

        public ICollection<TObject> FindAll(Expression<Func<TObject, bool>> match)
        {
            return ContextLocator.Get<CfmDbContext>().Set<TObject>().Where(match).ToList();
        }

        public ICollection<TObject> FindAll(Expression<Func<TObject, bool>> match, params Expression<Func<TObject, object>>[] includeProperties)
        {
            foreach (var property in includeProperties)
            {
                ContextLocator.Get<CfmDbContext>().Set<TObject>().Include(property);
            }
            return ContextLocator.Get<CfmDbContext>().Set<TObject>().Where(match).ToList();
        }

        public async Task<ICollection<TObject>> FindAllAsync(Expression<Func<TObject, bool>> match)
        {
            return await ContextLocator.Get<CfmDbContext>().Set<TObject>().Where(match).ToListAsync().ConfigureAwait(false);
        }

        public async Task<ICollection<TObject>> FindAllAsync(Expression<Func<TObject, bool>> match, params Expression<Func<TObject, object>>[] includeProperties)
        {
            foreach (var property in includeProperties)
            {
                ContextLocator.Get<CfmDbContext>().Set<TObject>().Include(property);
            }
            return await ContextLocator.Get<CfmDbContext>().Set<TObject>().Where(match).ToListAsync().ConfigureAwait(false);
        }

        public TObject Add(TObject t)
        {
            ContextLocator.Get<CfmDbContext>().Set<TObject>().Add(t);
            return t;
        }

        public TObject Update(TObject updated, int key)
        {
            if (updated == null)
                return null;

            TObject existing = ContextLocator.Get<CfmDbContext>().Set<TObject>().Find(key);
            if (existing != null)
            {
                ContextLocator.Get<CfmDbContext>().Entry(existing).CurrentValues.SetValues(updated);
            }
            return existing;
        }

        public void Delete(TObject t)
        {
            ContextLocator.Get<CfmDbContext>().Set<TObject>().Remove(t);
        }

        public void Delete(int key)
        {
            var t = ContextLocator.Get<CfmDbContext>().Set<TObject>().Find(key);
            if (t != null)
                Delete(t);
        }

        public int Count()
        {
            return ContextLocator.Get<CfmDbContext>().Set<TObject>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await ContextLocator.Get<CfmDbContext>().Set<TObject>().CountAsync().ConfigureAwait(false);
        }
    }
}