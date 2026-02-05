
using NguyenDucManh_SE1884_A01_BE.Models;
using NguyenDucManh_SE1884_A01_BE.Repositories;
using NguyenDucManh_SE1884_A01_BE.Repositories.IRepositories;
using NguyenDucManh_SE1884_A01_BE.Services;
using NguyenDucManh_SE1884_A01_BE.Services.IServices;

namespace UsersApp.Extensions
{
    public static class ServicesRegister
    {
        public static void RegisterCustomServices(this IServiceCollection services)
        {
            

            services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();

            
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddScoped<ITagService, TagService>();

            
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            
            services.AddTransient<INewsArticleRepository, NewsArticleRepository>();
            services.AddScoped<INewsArticleService, NewsArticleService>();

            
            services.AddTransient<ISystemAccountRepository, SystemAccountRepository>();
            services.AddScoped<ISystemAccountService, SystemAccountService>();

            
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddScoped<IReportService, ReportService>();

            // JWT Service
            services.AddScoped<IJwtService, JwtService>();
        }
    }
}
