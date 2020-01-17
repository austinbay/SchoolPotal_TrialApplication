using Newtonsoft.Json;


namespace SchoolPortal.Models.DataModels
{
    public class UserRoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }


    }
    public partial class UserRoleItem : UserRoleModel
    {



    }
   
    public class UserRoleDetails : UserRoleItem
    {

    }
    public partial class UserRoleFilter : UserRoleItem
    {

        public static UserRoleFilter Deserilize(string whereCondition)
        {
            UserRoleFilter filter = null;
            if (whereCondition != null)
            {
                filter = JsonConvert.DeserializeObject<UserRoleFilter>(whereCondition);
            }
            return filter;
        }
    }
}
