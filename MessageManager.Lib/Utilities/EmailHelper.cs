using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace MessageManager.Lib.Utilities
{
    public interface IEmailHelper
    {
        string GetEmailTemplate(string fileName);
        string GetApplicationBaseUrl();
        ClaimsPrincipal GetCurentUser();

        string GetCompanyLogoUrl(string fileName);

    }
    public class EmailHelper : IEmailHelper
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private IHostingEnvironment _hostingEnvironment;
        public EmailHelper(IHttpContextAccessor httpAccessor, IHostingEnvironment hostingEnvironment)
        {
            _httpAccessor = httpAccessor;
            _hostingEnvironment = hostingEnvironment;

        }
        public string GetApplicationBaseUrl()
        {
            var scheme = _httpAccessor.HttpContext.Request.Scheme;
            var host = _httpAccessor.HttpContext.Request.Host;

            return string.Format("{0}://{1}", scheme, host);
        }
        public ClaimsPrincipal GetCurentUser()
        {
            return _httpAccessor.HttpContext.User;
        }
        public string GetEmailTemplate(string fileName)
        {
            string template = "";

            //Get TemplateFile located at EmailTemplates  
            var templateDir = _hostingEnvironment.ContentRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplates"
                    + Path.DirectorySeparatorChar.ToString()
                    + fileName;

            //Get TemplateFile located at wwwroot/EmailTemplates  
            //var templateDir = _hostingEnvironment.WebRootPath
            //        + Path.DirectorySeparatorChar.ToString()
            //        + "EmailTemplates"
            //        + Path.DirectorySeparatorChar.ToString()
            //        + fileName;

            if (!File.Exists(templateDir))
                return template;

            //using streamreader for reading my htmltemplate
            using (StreamReader reader = new StreamReader(templateDir))
            {
                template = reader.ReadToEnd();
            }
            return template;
        }
        public string GetCompanyLogoUrl(string fileName)
        {
            //Get logogurl located at Images 
            var logoDir = _hostingEnvironment.ContentRootPath
                   + Path.DirectorySeparatorChar.ToString()
                   + "Images"
                   + Path.DirectorySeparatorChar.ToString()
                   + fileName;

            //Get logogurl located at wwwroot/Images 
            //var logoDir = _hostingEnvironment.WebRootPath
            //        + Path.DirectorySeparatorChar.ToString()
            //        + "Images"
            //        + Path.DirectorySeparatorChar.ToString()
            //        + fileName;

            if (!File.Exists(logoDir))
                return "";


            return logoDir;
        }
    }
}
