using Croco.Core.Application;
using Croco.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CrocoShop.Web.Logic.Services.Background
{
    public class DequeueIntegrationMessagesBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DequeueIntegrationMessagesBackgroundService> _logger;

        public DequeueIntegrationMessagesBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetRequiredService<ILogger<DequeueIntegrationMessagesBackgroundService>>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!CrocoApp.IsReady())
            {
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = await _serviceProvider.GetTaskForHandlingOneIntegrationMessage();
                    if (result != null)
                    {
                        _logger.LogInformation(JsonConvert.SerializeObject(result));
                        _logger.LogInformation("1 сообщение было обработано из очереди");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка при попытке получить задание");
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }
    }
}