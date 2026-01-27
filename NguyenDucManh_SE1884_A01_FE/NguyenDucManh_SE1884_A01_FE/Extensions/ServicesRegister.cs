//using Assignmen_PRN232__.Data;
using Assignmen_PRN232__.Models;
using Frontend.Services;
using Frontend.Services.IServices;

namespace UsersApp.Extensions
{
    public static class ServicesRegister
    {
        public static void RegisterCustomServices(this IServiceCollection services)
        {
            var cookieContainer = new System.Net.CookieContainer();
            
            services.AddHttpClient<ITagService, TagService>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    UseCookies = true,
                    CookieContainer = cookieContainer
                });
            
            services.AddHttpClient<ICategoryService, CategoryService>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    UseCookies = true,
                    CookieContainer = cookieContainer
                });
            
            services.AddHttpClient<INewsArticleService, NewsArticleService>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    UseCookies = true,
                    CookieContainer = cookieContainer
                });
            
            services.AddHttpClient<ISystemAccountService, SystemAccountService>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    UseCookies = true,
                    CookieContainer = cookieContainer
                });

            services.AddHttpClient<ILoginService, LoginService>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    UseCookies = true,
                    CookieContainer = cookieContainer
                });

            services.AddHttpClient<IReportService, ReportService>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    UseCookies = true,
                    CookieContainer = cookieContainer
                });
        }
    }
}
