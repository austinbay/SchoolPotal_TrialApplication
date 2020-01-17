using SchoolPortal.Data.Entities;
using System;

namespace SchoolPortal.Data.Helpers
{

    public interface IUnitOfWork : IDisposable
    {
        SchoolPortalContext Context { get; }
        void Commit();
    }
    public class UnitOfWork : IUnitOfWork
    {
        public SchoolPortalContext Context { get; }

        public UnitOfWork(SchoolPortalContext context)
        {
            Context = context;
        }
        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();

        }
    }
}
