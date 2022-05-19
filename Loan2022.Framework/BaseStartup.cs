using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Loan2022.Application.Extensions;
using Loan2022.Framework.Extensions;
using Loan2022.Framework.Permission;
using Loan2022.Infrastructure.CacheRepository.Extensions;
using Loan2022.Infrastructure.Extensions;
using Loan2022.Service.Extensions;

namespace Loan2022.Framework
{
    public abstract class BaseStartup
    {
        public BaseStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddInfrastructure(_configuration);
            services.AddPersistenceContexts(_configuration);
            services.AddRepositories();
            services.AddServices();
            services.AddApplicationLayer();
            services.AddCacheRepositories();
            services.AddSharedInfrastructure(_configuration);
        }
    }
}