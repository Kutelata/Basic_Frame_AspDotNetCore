using Loan2022.Application.Interfaces.Shared;
using Loan2022.Framework;
using Loan2022.Framework.Services;
using Microsoft.AspNetCore.Authorization;

namespace Loan2022.Client
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
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.Events.OnRedirectToLogin = opt =>
                {
                    opt.HttpContext.Response.Redirect("/");
                    return Task.FromResult(0);
                };
                options.SlidingExpiration = true;
            });
            services.AddTransient<IFileService, FileService>();
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
                    name: "wallet",
                    pattern: "wallet",
                    defaults:new {controller="Home", action="Wallet"});
                endpoints.MapControllerRoute(
                    name: "customer care",
                    pattern: "customer-care",
                    defaults:new {controller="Home", action="CustomerCare"});
                endpoints.MapControllerRoute(
                    name: "profile",
                    pattern: "profile",
                    defaults:new {controller="Home", action="Profile"});
                endpoints.MapControllerRoute(
                    name: "borrow",
                    pattern: "borrow",
                    defaults:new {controller="Home", action="Borrow"});
                endpoints.MapControllerRoute(
                    name: "verify",
                    pattern: "verify",
                    defaults:new {controller="Account", action="Verify"});
                endpoints.MapControllerRoute(
                    name: "register contract",
                    pattern: "register-contract",
                    defaults:new {controller="Home", action="RegisterContract"});
                endpoints.MapControllerRoute(
                    name: "sign contract",
                    pattern: "sign-contract",
                    defaults:new {controller="Home", action="ContractSign"});
                endpoints.MapControllerRoute(
                    name: "withdraw",
                    pattern: "withdraw",
                    defaults: new {controller = "Home", action = "Withdraw"});
                endpoints.MapControllerRoute(
                    name: "logout",
                    pattern: "logout",
                    defaults:new {controller="Account", action="Logout"});
                endpoints.MapControllerRoute(
                    name: "withdraw invoice",
                    pattern: "withdraw-invoice",
                    defaults:new {controller="Home", action="WithdrawInvoice"});
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
            });
        }
    }
}

