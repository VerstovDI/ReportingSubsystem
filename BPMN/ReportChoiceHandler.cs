using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camunda.Api.Client.ExternalTask;
using Camunda.Worker;

namespace ReportingSubsystem.BPMN
{
    [HandlerTopics("Report_makeChoice", LockDuration =10_000)]
    public class ReportChoiceHandler : ExternalTaskHandler
    {
        public override Task<IExecutionResult> Process(ExternalTask externalTask)
        {
            throw new NotImplementedException();
        }
    }
}
