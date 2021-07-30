using CrocoShop.Web.Logic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CrocoShop.Web.Logic
{
    public static class CourtRecordsRegistrator
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<CourtRecordService>();
        }
    }
}