using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CFM.Infrastructure.Interfaces
{
    public interface IBaseService<TObject> where TObject : class
    {
        ICollection<TObject> GetAll();
        Task<ICollection<TObject>> GetAllAsync();
        TObject Get(int id);
        TObject Get(int id, params Expression<Func<TObject, object>>[] includeProperties);
        Task<TObject> GetAsync(int id);
        Task<TObject> GetAsync(int id, params Expression<Func<TObject, object>>[] includeProperties);
        TObject Find(Expression<Func<TObject, bool>> match);
        TObject Find(Expression<Func<TObject, bool>> match, params Expression<Func<TObject, object>>[] includeProperties);
        Task<TObject> FindAsync(Expression<Func<TObject, bool>> match);
        Task<TObject> FindAsync(Expression<Func<TObject, bool>> match, params Expression<Func<TObject, object>>[] includeProperties);
        ICollection<TObject> FindAll(Expression<Func<TObject, bool>> match);
        ICollection<TObject> FindAll(Expression<Func<TObject, bool>> match, params Expression<Func<TObject, object>>[] includeProperties);
        Task<ICollection<TObject>> FindAllAsync(Expression<Func<TObject, bool>> match);
        Task<ICollection<TObject>> FindAllAsync(Expression<Func<TObject, bool>> match, params Expression<Func<TObject, object>>[] includeProperties);
        TObject Add(TObject t);
        TObject Update(TObject updated, int key);
        void Delete(TObject t);
        void Delete(int key);
        int Count();
        Task<int> CountAsync();
    }
}