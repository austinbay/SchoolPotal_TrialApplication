using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SchoolPortal.Data.Helpers
{
    public interface INewRepository<T> where T : class
    {
        IEnumerable<T> Get();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T Find(object id);
    }
    public class NewRepository<T> : INewRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly DbSet<T> _dbSet;

        public NewRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbSet = _unitOfWork.Context.Set<T>();

        }
        public void Add(T entity)
        {
            _unitOfWork.Context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _unitOfWork.Context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> Get()
        {
            return _unitOfWork.Context.Set<T>().AsEnumerable<T>();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _unitOfWork.Context.Set<T>().Where(predicate).AsEnumerable<T>();
        }

        public void Update(T entity)
        {
            _unitOfWork.Context.Set<T>().Update(entity);
        }
        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, int? page = null, int? pageSize = null)
        {
            IQueryable<T> query = _dbSet;

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
        public virtual T Find(object id)
        {
            return _unitOfWork.Context.Set<T>().Find(id);
        }
       
    }
}
