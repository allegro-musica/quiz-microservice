using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Quizzer.Infrastructure.Extensions
{
    public static class EventBusExtensions
    {
        public static IServiceCollection AddBus(this IServiceCollection services, string connection)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(connection);
                });
            });

            return services;
        }
    }
}
