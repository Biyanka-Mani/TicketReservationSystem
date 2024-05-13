using Microsoft.AspNetCore.Authentication.Cookies;
using TicketReservation.App.Services;


namespace TicketReservationApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllersWithViews();

            builder.Services.Configure<ApiAddress>(builder.Configuration.GetSection("Api"));
            builder.Services.AddAuthentication("AdminCookies")
                .AddCookie("AdminCookies", options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Forbidden/";
                    options.LoginPath = "/Home/AdminLogin"; // Set the login path for admin
                });

            // For User authentication
            builder.Services.AddAuthentication("UserCookies")
                .AddCookie("UserCookies", options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Forbidden/";
                    options.LoginPath = "/Home/UserLogin";
                    options.ReturnUrlParameter = string.Empty;
                });
          
            
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<CustomerServices>();
            builder.Services.AddScoped<AdminstratorServices>();
            builder.Services.AddScoped<CommonServices>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=UserPage}/{id?}");
            app.MapControllerRoute(
            name: "admin",
            pattern: "admin/{action=AdminPage}",
           defaults: new { controller = "Home" });
            app.Run();
        }
    }
}