using SchoolPortal.Data.Entities;
using SchoolPortal.Data.Helpers;
using SchoolPortal.Models.DataModels;
using System.Collections.Generic;


namespace SchoolPortal.Data.Repositories
{
    public interface IRefreshTokenRepository : INewRepository<RefreshToken>
    {
        IEnumerable<RefreshToken> LoadAll(RefreshTokenFilter filter = null);

        IEnumerable<RefreshToken> LoadByPage(int page, int pageSize, RefreshTokenFilter filter = null);
    }
    public class RefreshTokenRepository : NewRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IUnitOfWork dbContext)
           : base(dbContext)
        {

        }

        public IEnumerable<RefreshToken> LoadAll(RefreshTokenFilter filter = null)
        {
            var expression = new RefreshTokenQueryObject(filter).Expression;
            return Fetch(expression);
        }

        public IEnumerable<RefreshToken> LoadByPage(int page, int pageSize, RefreshTokenFilter filter = null)
        {
            var filterExpression = new RefreshTokenQueryObject(filter).Expression;
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 1;
            return Fetch(filterExpression, page, pageSize);
        }
        public class RefreshTokenQueryObject : QueryObject<RefreshToken>
        {
            public RefreshTokenQueryObject(RefreshTokenFilter filter)
            {
                if (filter != null)
                {


                }
            }
        }
    }
}
