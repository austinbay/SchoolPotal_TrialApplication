using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralHelper.Lib.Models
{
    public class ServiceResponseData
    {

        private string _errorMessage;
        public bool IsSuccess;


        public ServiceResponseData()
        {
            IsSuccess = true;
        }


        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                IsSuccess = string.IsNullOrWhiteSpace(ErrorMessage) ? true : false;
            }
        }


    }

    public class ServiceResponseData<T> : ServiceResponseData
    {
        public T Data { get; set; }

        public ServiceResponseData()
        {
            IsSuccess = true;
        }
    }
}
