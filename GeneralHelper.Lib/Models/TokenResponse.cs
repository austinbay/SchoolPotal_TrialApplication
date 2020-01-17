

using System;

namespace GeneralHelper.Lib.Models
{
   
    public class TokenData
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string FirstName { get; set; }

        public string TenantCode { get; set; }
        public DateTime TokenExpiration { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
      
    }
}
