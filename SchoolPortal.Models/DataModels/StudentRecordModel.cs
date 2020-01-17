using Newtonsoft.Json;
using System;

namespace SchoolPortal.Models.DataModels
{
    public class StudentRecordModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }


    }
    public partial class StudentRecordItem : StudentRecordModel
    {



    }
   
    public class StudentRecordDetails : StudentRecordItem
    {

    }
    public partial class StudentRecordFilter : StudentRecordItem
    {

        public static StudentRecordFilter Deserilize(string whereCondition)
        {
            StudentRecordFilter filter = null;
            if (whereCondition != null)
            {
                filter = JsonConvert.DeserializeObject<StudentRecordFilter>(whereCondition);
            }
            return filter;
        }
    }
}
