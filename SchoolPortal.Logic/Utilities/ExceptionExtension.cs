

using GeneralHelper.Lib.Models;
using System;

namespace SchoolPortal.Logic.Utilities
{

    public static class ExceptionExtension
    {     
        public static string ProcessException(this Exception ex, IErrorService errorService)
        {
            if (ex.InnerException != null)
            {
                ex = ex.InnerException.GetBaseException();
            }

            var message = ex.Message + "/n" + ex.StackTrace;

            errorService.LogError(message);
            return ex.Message;
        }
        
    }
}



