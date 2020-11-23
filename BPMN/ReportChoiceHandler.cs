using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camunda.Api.Client.ExternalTask;
using Camunda.Worker;
using ReportingSubsystem.Models;

namespace ReportingSubsystem.BPMN
{

    [HandlerTopics("Report_makeChoice", LockDuration =10_000)]
    public class ReportChoiceHandler : ExternalTaskHandler
    {
        private readonly BpmnService bpmnService;

        public ReportChoiceHandler(BpmnService bpmnService) 
        {
            this.bpmnService = bpmnService;
        }

        public override Task<IExecutionResult> Process(ExternalTask externalTask)
        {

            throw new NotImplementedException();
        }

        public async Task Handle(Report report)
        {
            var processInstanceId = await bpmnService.StartProcessFor(report);
            //report.AssociateWithProcessInstance(processInstanceId);
        }
    }
}
