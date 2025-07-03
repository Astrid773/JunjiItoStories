using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JunjiIto.Areas.Identity.Data;
using JunjiIto.Data;
namespace JunjiIto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("JunjiItoContextConnection")
            ?? throw new InvalidOperationException("Connection string 'JunjiItoContextConnection' not found.");

            builder.Services.AddDbContext<LibroContext>(options => 
                options.UseSqlServer(connectionString));

            builder.Services.AddDbContext<JunjiItoContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<JunjiItoUser>(options => 
                options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<JunjiItoContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
