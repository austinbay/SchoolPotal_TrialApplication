using GeneralHelper.Lib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolPortal.Logic.Utilities
{
    public interface IAuditTrailService
    {

        void Create(int userId, string actionPerformed, DateTime dateAndTime);
        Task<IEnumerable<AuditTrailModel>> GetAll();
    }
    public class AuditTrailService : IAuditTrailService
    {
       
        public AuditTrailService()
        {

        }

        public void Create(int userId, string actionPerformed, DateTime dateAndTime)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AuditTrailModel>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
