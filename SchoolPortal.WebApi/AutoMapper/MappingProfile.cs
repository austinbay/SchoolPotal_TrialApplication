using SchoolPortal.Data.Entities;
using SchoolPortal.Models.DataModels;
using AutoMapper;

namespace SchoolPortal.WebApi.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            UserAccountMapper();
            UserRoleMapper();
            StudentRecordMapper();

        }
      
        public void UserAccountMapper()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<AspNetUsers, AspNetUserModel>()
                 .ForMember(d => d.FullName, map => map.MapFrom(c => c.LastName + " " + c.FirstName));

            CreateMap<AspNetUserModel, AspNetUsers>();
            CreateMap<AspNetUsers, AspNetUserDetails>()
             .ForMember(d => d.FullName, map => map.MapFrom(c => c.LastName + " " + c.FirstName));

            CreateMap<AspNetUsers, AspNetUserItem>()
                  .ForMember(d => d.FullName, map => map.MapFrom(c => c.LastName + " " + c.FirstName));

        }
        public void UserRoleMapper()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<AspNetRoles, UserRoleModel>();
               
            CreateMap<UserRoleModel, AspNetRoles>();
            CreateMap<AspNetRoles, UserRoleDetails>();

            CreateMap<AspNetRoles, UserRoleItem>();
                 
        }
        public void StudentRecordMapper()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<StudentRecord, StudentRecordModel>();

            CreateMap<StudentRecordModel, StudentRecord>();
            CreateMap<StudentRecord, StudentRecordDetails>();

            CreateMap<StudentRecord, StudentRecordItem>()
                 .ForMember(d => d.CreatedByName, map => map.MapFrom(c => c.CreatedByNavigation.LastName + " " + c.CreatedByNavigation.FirstName));

        }
    }
}
