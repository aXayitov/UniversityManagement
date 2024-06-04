using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data;
using UniversityManagement.Interfaces;
using UniversityManagement.Services;
using UniversityManagement.TestDataCreator;

namespace UniversityManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Local storage
            //builder.Services.AddDbContext<UniversityDbContext>(options => 
            //    options.UseSqlite("DataSource = Storage\\UniversityManagementSystem.db"));
            
            // SQL Server
            builder.Services.AddDbContext<UniversityDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IInstructorService, InstructorsService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ICourseAssigmentService, CourseAssigmentService>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (app.Environment.IsDevelopment())
            {
                var scope = app.Services.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<UniversityDbContext>();
                DataBaseSeeder.SeedDataBase(context);
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
