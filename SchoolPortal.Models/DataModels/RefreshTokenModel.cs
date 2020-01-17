using Newtonsoft.Json;
using System;

namespace SchoolPortal.Models.DataModels
{
    public class RefreshTokenModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RefreshToken1 { get; set; }
        public DateTime TokenExpiration { get; set; }


    }
    public partial class RefreshTokenItem : RefreshTokenModel
    {



    }
   
    public class RefreshTokenDetails : RefreshTokenItem
    {

    }
    public partial class RefreshTokenFilter : RefreshTokenItem
    {

        public static RefreshTokenFilter Deserilize(string whereCondition)
        {
            RefreshTokenFilter filter = null;
            if (whereCondition != null)
            {
                filter = JsonConvert.DeserializeObject<RefreshTokenFilter>(whereCondition);
            }
            return filter;
        }
    }
}
