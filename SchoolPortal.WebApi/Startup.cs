using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using GeneralHelper.Lib.Models;
using SchoolPortal.WebApi.Extensions;
using SchoolPortal.Data.Entities;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Authorization;
using AutoMapper;
using SchoolPortal.WebApi.AutoMapper;
using System.Threading.Tasks;

namespace SchoolPortal.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            services.AddMvc();

            services.AddDbContext<SchoolPortalContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddIdentity<ApplicationUser, IdentityRole<int>>()
            .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    //Here, we are creating and using JWT within the same application.
                    //In this case, base URL is fine.
                    //If the JWT is created using a web service, then this would be the consumer URL.
                    ValidAudience = Configuration["ApiServerUrl"],
                    //Usually, this is your application base URL
                    ValidIssuer = Configuration["ApiServerUrl"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["ApiSecretKey"])),
                    ValidateIssuerSigningKey = true,                                                
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero


                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("TokenExpired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.ConfigureCorsServices();
            services.ConfigureScopeServices();
            services.ConfigureSingletonServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(name: "v1", new OpenApiInfo { Title = "SchoolPortal API", Version = "v1" });

            });

            //add global authorization
            services.AddMvcCore(options =>
            {
                options.Filters.Add(new AuthorizeFilter());

            });

            // Start Registering and Initializing AutoMapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DataMigration.Initialize(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        public static class DataMigration
        {
            public static void Initialize(IServiceProvider serviceProvider)
            {
                using (var serviceScope = serviceProvider.CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                    // auto migration
                   
                    context.Database.Migrate();

                    // Seed the database.
                    InitializeData(serviceScope);
                }
            }

            private static void InitializeData(IServiceScope serviceScope)
            {
                // init user and roles  
                try
                {
                    //var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    var dBcontext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                    var defaultUserEmail = "admin@gmail.com";
                    var defaultUserPassword = "Admin@123";

                    var user = userManager.FindByEmailAsync(defaultUserEmail).Result;
                    if (user == null)
                    {
                       
                        var defaultUser = new ApplicationUser
                        {
                            UserName = defaultUserEmail,
                            Email = defaultUserEmail,
                            PhoneNumber = "08173294532",
                            FirstName = "Johnson",
                            LastName = "Obama",
                            IsActive = true,                         
                            EmailConfirmed = true,
                            PhoneNumberConfirmed = true,
                        };
                        var result = userManager.CreateAsync(defaultUser, defaultUserPassword).Result;
                        
                    }                   
                }
                catch (Exception ex)
                {


                }
            }
           
            
        }
    }
}
