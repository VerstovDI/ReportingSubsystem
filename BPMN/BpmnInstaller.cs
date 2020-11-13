using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ReportingSubsystem.BPMN
{
    public static class BpmnInstaller
    {   /*
        public static IServiceCollection AddCamunda(this IServiceCollection services, string camundaRestApiUri)
        {
            services.AddSingleton(_ => new BpmnService(camundaRestApiUri));
            services.AddHostedService<BpmnProcessDeployService>();

            services.AddCamundaWorker(options =>
            {
                options.BaseUri = new Uri(camundaRestApiUri);
                options.WorkerCount = 1;
            })
            .AddHandler<ReportChoiceHandler>();
            
            return services;
        }*/
    }
}
