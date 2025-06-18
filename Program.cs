using CareerBuilderX.AI.Interfaces;
using CareerBuilderX.AI.Services;
using CareerBuilderX.Data;
using CareerBuilderX.Models;
using CareerBuilderX.Repositories.Interfaces;
using CareerBuilderX.Repositories.Repo;
using CareerBuilderX.Repository.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using System.Runtime.InteropServices;


namespace CareerBuilderX
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddDefaultIdentity<Person>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<IResumeRepository, ResumeRepository>();
            builder.Services.AddTransient<IPortfolioRepository, PortfolioRepository>();
            builder.Services.AddTransient<IServiceRepository, ServiceRepository>();

            // Add Semantic K. 99 
            var OpenAiKey = builder.Configuration["OpenAI:ApiKey"];
            builder.Services.AddSingleton<Kernel>(sp =>
            {
                var kernelBuilder = Kernel.CreateBuilder();
                kernelBuilder.AddOpenAIChatCompletion("gpt-4",OpenAiKey );
                return kernelBuilder.Build();
            });
            builder.Services.AddSingleton<IResumeAiService, ResumeAiService>();
            builder.Services.AddSingleton<IPortfolioAiService, PortfolioAiService>();


            var app = builder.Build();
            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}