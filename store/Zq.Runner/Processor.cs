using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zq.Runner.Handlers;

namespace Zq.Runner
{
    public class Processor : BackgroundService
    {
        private readonly IList<IHandler> _handlers;
        private readonly IServiceProvider _rootServiceProvider;

        public Processor(IServiceProvider rootServiceProvider, IHandler[] handlers)
        {
            this._handlers = handlers.ToList();
            this._rootServiceProvider = rootServiceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                foreach (var item in _handlers)
                {
                    await item.Handle();
                }
            }
            catch (Exception ex)
            {
                _rootServiceProvider.GetService<ILogger>().LogError(ex, "Main");
                throw;
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var item in _handlers)
            {
                item.Dispose();
            }

            return base.StopAsync(cancellationToken);
        }
    }
}
