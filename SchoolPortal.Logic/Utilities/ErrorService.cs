using GeneralHelper.Lib.Models;
using NLog;
using System;
using System.Threading.Tasks;

namespace SchoolPortal.Logic.Utilities
{

    public interface IErrorService
    {
        Task LogError(string message);
        Task<ErrorModel> GetErrors();
    }
    public class ErrorService : IErrorService
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public ErrorService()
        {

        }

        public Task<ErrorModel> GetErrors()
        {
            throw new NotImplementedException();
        }

        public async Task LogError(string message)
        {
            logger.Error(message);
            await Task.CompletedTask;
        }


    }
}
