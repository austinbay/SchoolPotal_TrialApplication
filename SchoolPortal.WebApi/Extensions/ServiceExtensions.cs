using SchoolPortal.Data.Helpers;
using SchoolPortal.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using SchoolPortal.Logic.Utilities;
using SchoolPortal.Logic.BusinessLogic;
using MessageManager.Lib.Services;
using MessageManager.Lib.Utilities;

namespace SchoolPortal.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCorsServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }
        public static void ConfigureScopeServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailManager, EmailManager>();
            services.AddScoped<IEmailHelper, EmailHelper>();
            services.AddScoped<ISendGridEmailService, SendGridEmailService>();
            services.AddScoped<ISmtpEmailService, SmtpEmailService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IErrorService, ErrorService>();
            services.AddScoped<IAuditTrailService, AuditTrailService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();           
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<IAspNetUserRepository, AspNetUserRepository>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IStudentRecordRepository, StudentRecordRepository>();
            services.AddScoped<IStudentRecordService, StudentRecordService>();
        }
        
        public static void ConfigureSingletonServices(this IServiceCollection services)
        {
            //ervices.AddSingleton<ILogService, LogService>();
        }
    }
}
