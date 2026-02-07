using Assignmen_PRN232__.Models;
using Frontend.Handlers;
using Frontend.Services;
using Frontend.Services.IServices;

namespace UsersApp.Extensions
{
    public static class ServicesRegister
    {
        public static void RegisterCustomServices(this IServiceCollection services)
        {
            services.AddTransient<JwtAuthenticationHandler>();
            services.AddScoped<IJwtHelperService, JwtHelperService>();
            
            services.AddHttpClient<ITagService, TagService>()
                .AddHttpMessageHandler<JwtAuthenticationHandler>();
            
            services.AddHttpClient<ICategoryService, CategoryService>()
                .AddHttpMessageHandler<JwtAuthenticationHandler>();
            
            services.AddHttpClient<INewsArticleService, NewsArticleService>()
                .AddHttpMessageHandler<JwtAuthenticationHandler>();
            
            services.AddHttpClient<ISystemAccountService, SystemAccountService>()
                .AddHttpMessageHandler<JwtAuthenticationHandler>();

            services.AddHttpClient<ILoginService, LoginService>();

            services.AddHttpClient<IReportService, ReportService>()
                .AddHttpMessageHandler<JwtAuthenticationHandler>();

            services.AddHttpClient<IAuditLogService, AuditLogService>()
                .AddHttpMessageHandler<JwtAuthenticationHandler>();

            services.AddHttpClient<IDashboardService, DashboardService>()
                .AddHttpMessageHandler<JwtAuthenticationHandler>();
        }
    }
}
