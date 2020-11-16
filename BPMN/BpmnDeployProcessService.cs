using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ReportingSubsystem.BPMN
{   
    public class BpmnProcessDeployService : IHostedService
    {
        // Field with BpmnService to control cooperation with Camunda Engine
        private readonly BpmnService bpmnService;

        // Method for the deployment on the server (localhost)
        public BpmnProcessDeployService(BpmnService bpmnService)
        {
            this.bpmnService = bpmnService;
        }

        // Method to start the deployment
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await bpmnService.DeployProcessDefinition();

            await bpmnService.CleanupProcessInstances();
        }

        // Method to stop 
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
