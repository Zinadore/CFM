using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            return await ContextLocator.Get<CfmDbContext>().Set<TObject>()
                                                            .ToListAsync()
                                                            .ConfigureAwait(false);
        }

        public TObject Get(int id)
        {
            var test = ContextLocator.Get<CfmDbContext>();
            return test.Set<TObject>().Find(id);
        }

        public async Task<TObject> GetAsync(int id)
        {
            return await ContextLocator.Get<CfmDbContext>().Set<TObject>()
                                                            .FindAsync(id)
                                                            .ConfigureAwait(false);
        }

        public TObject Find(Expression<Func<TObject, bool>> match)
        {
            return ContextLocator.Get<CfmDbContext>().Set<TObject>().SingleOrDefault(match);
        }

        public async Task<TObject> FindAsync(Expression<Func<TObject, bool>> match)
        {
            return await ContextLocator.Get<CfmDbContext>().Set<TObject>()
                                                            .SingleOrDefaultAsync(match)
                                                            .ConfigureAwait(false);
        }

        public ICollection<TObject> FindAll(Expression<Func<TObject, bool>> match)
        {
            return ContextLocator.Get<CfmDbContext>().Set<TObject>().Where(match).ToList();
        }

        public async Task<ICollection<TObject>> FindAllAsync(Expression<Func<TObject, bool>> match)
        {
            return await ContextLocator.Get<CfmDbContext>().Set<TObject>()
                                                            .Where(match)
                                                            .ToListAsync()
                                                            .ConfigureAwait(false);
        }

        public TObject Add(TObject t)
        {
            ContextLocator.Get<CfmDbContext>().Set<TObject>().Add(t);
            return t;
        }

        public async Task<TObject> AddAsync(TObject t)
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

        public async Task<TObject> UpdateAsync(TObject updated, int key)
        {
            if (updated == null)
                return null;

            TObject existing = await ContextLocator.Get<CfmDbContext>().Set<TObject>().FindAsync(key).ConfigureAwait(false);
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
            Delete(t);
        }

        public async Task DeleteAsync(TObject t)
        {
            ContextLocator.Get<CfmDbContext>().Set<TObject>().Remove(t);
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