using Microsoft.EntityFrameworkCore;
using SchoolPortal.Data.Entities;
using SchoolPortal.Data.Helpers;
using SchoolPortal.Models.DataModels;
using System.Collections.Generic;


namespace SchoolPortal.Data.Repositories
{
    public interface IStudentRecordRepository : INewRepository<StudentRecord>
    {
        IEnumerable<StudentRecord> LoadAll(StudentRecordFilter filter = null);

        IEnumerable<StudentRecord> LoadByPage(int page, int pageSize, StudentRecordFilter filter = null);
    }
    public class StudentRecordRepository : NewRepository<StudentRecord>, IStudentRecordRepository
    {
        public StudentRecordRepository(IUnitOfWork dbContext)
           : base(dbContext)
        {

        }

        public IEnumerable<StudentRecord> LoadAll(StudentRecordFilter filter = null)
        {
            var expression = new StudentRecordQueryObject(filter).Expression;
            return Fetch(expression).Include(x => x.CreatedByNavigation);
        }

        public IEnumerable<StudentRecord> LoadByPage(int page, int pageSize, StudentRecordFilter filter = null)
        {
            var filterExpression = new StudentRecordQueryObject(filter).Expression;
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 1;
            return Fetch(filterExpression, page, pageSize).Include(x => x.CreatedByNavigation);
        }
        public class StudentRecordQueryObject : QueryObject<StudentRecord>
        {
            public StudentRecordQueryObject(StudentRecordFilter filter)
            {
                if (filter != null)
                {


                }
            }
        }
    }
}
