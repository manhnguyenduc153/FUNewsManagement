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
            services.AddScoped<CachingHandler>();
            services.AddScoped<IJwtHelperService, JwtHelperService>();

            services.AddHttpClient<ITagService, TagService>()
                .AddHttpMessageHandler<CachingHandler>()
                .AddHttpMessageHandler<JwtAuthenticationHandler>();

            services.AddHttpClient<ICategoryService, CategoryService>()
                .AddHttpMessageHandler<CachingHandler>()
                .AddHttpMessageHandler<JwtAuthenticationHandler>();

            services.AddHttpClient<INewsArticleService, NewsArticleService>()
                .AddHttpMessageHandler<CachingHandler>()
                .AddHttpMessageHandler<JwtAuthenticationHandler>();

            services.AddHttpClient<ISystemAccountService, SystemAccountService>()
                .AddHttpMessageHandler<CachingHandler>()
                .AddHttpMessageHandler<JwtAuthenticationHandler>();

            services.AddHttpClient<ILoginService, LoginService>();

            services.AddHttpClient<IReportService, ReportService>()
                .AddHttpMessageHandler<CachingHandler>()
                .AddHttpMessageHandler<JwtAuthenticationHandler>();

            services.AddHttpClient<IAuditLogService, AuditLogService>()
                .AddHttpMessageHandler<CachingHandler>()
                .AddHttpMessageHandler<JwtAuthenticationHandler>();

            services.AddHttpClient<IDashboardService, DashboardService>()
                .AddHttpMessageHandler<CachingHandler>()
                .AddHttpMessageHandler<JwtAuthenticationHandler>();

            services.AddHttpClient<IAIService, AIService>()
                .AddHttpMessageHandler<JwtAuthenticationHandler>();

            services.AddSingleton<IOfflineDetectionService, OfflineDetectionService>();
            
            services.AddHostedService<DataCachingBackgroundService>();
        }
    }
}
