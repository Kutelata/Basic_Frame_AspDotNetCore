using Loan2022.Application.Interfaces.Shared;
using Loan2022.Application.Settings;
using Loan2022.Framework.Services;
using Loan2022.Infrastructure.DbContexts;
using Loan2022.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Thinktecture;

namespace Loan2022.Framework.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddPersistenceContexts(configuration);
            services.AddAuthenticationScheme(configuration);
        }

        private static void AddAuthenticationScheme(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc(o =>
            {
                //Add Authentication to all Controllers by default.
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            });
        }
        private static void AddPersistenceContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection("ConnectionStrings").Get<SqlSettings>();
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(config.ApplicationConnection));
            services.AddDbContext<ApplicationDbContext>(builder => builder
                .UseSqlServer(config.ApplicationConnection, sqlOptions =>
                {
                    sqlOptions.AddRowNumberSupport();
                }));

            //services.AddDbContext<IdentityContext>(options => options.UseSqlServer(config.IdentityConnection));
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
        }
        
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthenticatedUserService, AuthenticatedUserService>();
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        }
    }
}