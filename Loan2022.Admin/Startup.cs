using Loan2022.Framework;
using Microsoft.AspNetCore.Authorization;

namespace Loan2022.Admin
{
    public class Startup: BaseStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
          
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddRazorPages();
            services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = "/login";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "customers",
                    pattern: "customers",
                    defaults:new {controller="Customer", action="Customers"});
                
                endpoints.MapControllerRoute(
                    name: "customer",
                    pattern: "customers/{id?}",
                    defaults:new {controller="Customer", action="GetCustomer"});    
                
                endpoints.MapControllerRoute(
                    name: "ChangePasswordCustomer",
                    pattern: "customers/change-password/{id?}",
                    defaults:new {controller="Customer", action="ChangePasswordView"});
                
                endpoints.MapControllerRoute(
                    name: "UpdateCustomer",
                    pattern: "customers/update/{id?}",
                    defaults:new {controller="Customer", action="UpdateCustomerView"});
                
                endpoints.MapControllerRoute(
                    name: "employees",
                    pattern: "employees",
                    defaults:new {controller="Employee", action="Employees"});
                
                endpoints.MapControllerRoute(
                    name: "createOrUpdateEmployee",
                    pattern: "employees/create",
                    defaults:new {controller="Employee", action="CreateOrUpdateEmployeeView"});
              
                endpoints.MapControllerRoute(
                    name: "createOrUpdateEmployee",
                    pattern: "employees/{id?}",
                    defaults:new {controller="Employee", action="CreateOrUpdateEmployeeView"});
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Index}");
                
                endpoints.MapControllerRoute(
                    name: "Login",
                    pattern: "login",
                    defaults:new {controller="Account", action="Index"});   
                
                
                endpoints.MapControllerRoute(
                    name: "home",
                    pattern: "home",
                    defaults:new {controller="Home", action="Index"});
                
                
                endpoints.MapControllerRoute(
                    name: "banks",
                    pattern: "banks",
                    defaults:new {controller="Bank", action="Index"});
                endpoints.MapControllerRoute(
                    name: "createOrUpdateBank",
                    pattern: "banks/create",
                    defaults:new {controller="Bank", action="CreateOrUpdateBankView"});
                endpoints.MapControllerRoute(
                    name: "createOrUpdateBank",
                    pattern: "banks/{id?}",
                    defaults:new {controller="Bank", action="CreateOrUpdateBankView"});
                
                endpoints.MapControllerRoute(
                    name: "interests",
                    pattern: "interests",
                    defaults:new {controller="Interest", action="Index"});
                endpoints.MapControllerRoute(
                    name: "createOrUpdateInterest",
                    pattern: "interests/create",
                    defaults:new {controller="Interest", action="CreateOrUpdateInterestView"});
                endpoints.MapControllerRoute(
                    name: "createOrUpdateInterest",
                    pattern: "interests/{id?}",
                    defaults:new {controller="Interest", action="CreateOrUpdateInterestView"});
                
                
                endpoints.MapControllerRoute(
                    name: "settings",
                    pattern: "settings",
                    defaults:new {controller="Setting", action="Index"});
                endpoints.MapControllerRoute(
                    name: "createOrUpdateSetting",
                    pattern: "settings/create",
                    defaults:new {controller="Setting", action="CreateOrUpdateSettingView"});
                endpoints.MapControllerRoute(
                    name: "createOrUpdateSetting",
                    pattern: "settings/{id?}",
                    defaults:new {controller="Setting", action="CreateOrUpdateSettingView"});
                
                
            });
        }
    }
}

