using SchoolPortal.Data.Entities;
using SchoolPortal.Data.Helpers;
using SchoolPortal.Models.DataModels;
using System.Collections.Generic;


namespace SchoolPortal.Data.Repositories
{
    public interface IUserRoleRepository : INewRepository<AspNetRoles>
    {
        IEnumerable<AspNetRoles> LoadAll(UserRoleFilter filter = null);

        IEnumerable<AspNetRoles> LoadByPage(int page, int pageSize, UserRoleFilter filter = null);
    }
    public class UserRoleRepository : NewRepository<AspNetRoles>, IUserRoleRepository
    {
        public UserRoleRepository(IUnitOfWork dbContext)
           : base(dbContext)
        {

        }

        public IEnumerable<AspNetRoles> LoadAll(UserRoleFilter filter = null)
        {
            var expression = new UserRoleQueryObject(filter).Expression;
            return Fetch(expression);
        }

        public IEnumerable<AspNetRoles> LoadByPage(int page, int pageSize, UserRoleFilter filter = null)
        {
            var filterExpression = new UserRoleQueryObject(filter).Expression;
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 1;
            return Fetch(filterExpression, page, pageSize);
        }
        public class UserRoleQueryObject : QueryObject<AspNetRoles>
        {
            public UserRoleQueryObject(UserRoleFilter filter)
            {
                if (filter != null)
                {


                }
            }
        }
    }
}
