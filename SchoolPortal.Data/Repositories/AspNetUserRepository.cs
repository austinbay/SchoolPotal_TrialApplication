using SchoolPortal.Data.Entities;
using SchoolPortal.Data.Helpers;
using SchoolPortal.Models.DataModels;
using System.Collections.Generic;


namespace SchoolPortal.Data.Repositories
{
    public interface IAspNetUserRepository : INewRepository<AspNetUsers>
    {
        IEnumerable<AspNetUsers> LoadAll(AspNetUserFilter filter);

        IEnumerable<AspNetUsers> LoadByPage(int page, int pageSize, AspNetUserFilter filter);
    }

    public class AspNetUserRepository : NewRepository<AspNetUsers>, IAspNetUserRepository
    {
        public AspNetUserRepository(IUnitOfWork dbContext)
            : base(dbContext)
        {

        }
        public IEnumerable<AspNetUsers> LoadAll(AspNetUserFilter filter)
        {
            var expression = new AspNetUserQueryObject(filter).Expression;
            return Fetch(expression);
        }
        public IEnumerable<AspNetUsers> LoadByPage(int page, int pageSize, AspNetUserFilter filter)
        {          
            var filterExpression = new AspNetUserQueryObject(filter).Expression;
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 1;
            return Fetch(filterExpression, page, pageSize);
        }
        public class AspNetUserQueryObject : QueryObject<AspNetUsers>
        {
            public AspNetUserQueryObject(AspNetUserFilter filter)
            {
                if (filter != null)
                {


                }
            }
        }
    }
}
