using SchoolPortal.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolPortal.Data.Helpers
{
    public interface IRepository<TEntity> where TEntity : class
    {
        int SaveChanges();
        TEntity Find(params object[] keyValues);
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        void Insert(TEntity entity);

        void InsertRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Delete(object id);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Queryable();
        IQueryable<TEntity> Table { get; }


    }

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Private Fields

        private readonly SchoolPortalContext _dbContext;

        protected readonly DbSet<TEntity> _dbSet;

        #endregion Private Fields

        public Repository(SchoolPortalContext dbContext)
        {

            _dbContext = dbContext;

            _dbSet = _dbContext.Set<TEntity>();

        }
        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Fetch(predicate).Count();
        }


        public virtual IQueryable<TEntity> Fetch(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null, int? page = null, int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        public virtual IQueryable<TEntity> Table
        {
            get { return Queryable(); }
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Table.SingleOrDefault(predicate);
        }

        public virtual TEntity Find(object id)
        {
            return _dbSet.Find(id);
        }


        public virtual void Insert(TEntity entity)
        {
            
            _dbContext.Set<TEntity>().Add(entity);
        }

        public virtual void InserRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }

        }


        public virtual void Update(TEntity entity)
        {
            
            _dbContext.Set<TEntity>().Update(entity);
        }
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }
        public void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);

        }

        public virtual void Delete(TEntity entity)
        {            
            _dbContext.Set<TEntity>().Remove(entity);
        }
        public virtual async Task<TEntity> GetAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public virtual async Task<TEntity> GetAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(cancellationToken, keyValues);
        }

        public virtual async Task<bool> DeleteAsync(params object[] keyValues)
        {
            return await DeleteAsync(CancellationToken.None, keyValues);
        }

        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = await GetAsync(cancellationToken, keyValues);

            if (entity == null)
            {
                return false;
            }
            _dbContext.Set<TEntity>().Remove(entity);

            return true;
        }
        public virtual Task<TEntity> FindAsync(params object[] keyValues)
        {
            return FindAsync(CancellationToken.None, keyValues);
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(cancellationToken, keyValues);
        }



        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);

        }
        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);

        }

        public virtual IQueryable<TEntity> Queryable()
        {
            return _dbSet;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }


    }
}
