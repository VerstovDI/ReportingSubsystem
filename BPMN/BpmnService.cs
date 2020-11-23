using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camunda.Api.Client;
using Camunda.Api.Client.Deployment;
using Camunda.Api.Client.Message;
using Camunda.Api.Client.ProcessDefinition;
using Camunda.Api.Client.ProcessInstance;
using Camunda.Api.Client.UserTask;
using ReportingSubsystem.Models;

namespace ReportingSubsystem.BPMN
{
    public class BpmnService : IServiceProvider
    { 
        private readonly CamundaClient camunda;

        public BpmnService()
        {
        }

        public BpmnService(string camundaRestApiUri)
        {
            this.camunda = CamundaClient.Create(camundaRestApiUri);
        }

        public async Task DeployProcessDefinition()
        {
            var bpmnResourceStream = this.GetType()
                .Assembly
                .GetManifestResourceStream("ReportingSubsystem.BPMN.reports.bpmn");

            try
            {
                await camunda.Deployments.Create(
                    "Reporting subsystem deployment",
                    true,
                    true,
                    null,
                    null,
                    new ResourceDataContent(bpmnResourceStream, "reports.bpmn"));
            }
            catch (Exception e)
            {
                throw new ApplicationException("Failed to deploy process definition", e);
            }
        }

        public async Task<string> StartProcessFor(Report report)
        {
            var processParams = new StartProcessInstance()
                .SetVariable("reportId", VariableValue.FromObject(report.ReportId.ToString()))
                .SetVariable("reportName", VariableValue.FromObject(report.ReportName.ToString()))
                .SetVariable("reportStatus", VariableValue.FromObject(report.ReportStatus.ToString()))
                .SetVariable("reportDate", VariableValue.FromObject(report.ReportDateAndTime.ToString()));

            processParams.BusinessKey = report.ReportId.ToString();

            var processStartResult = await
                camunda.ProcessDefinitions.ByKey("Report_Creation_Def_Key").StartProcessInstance(processParams);
            return processStartResult.Id;
        }

        public async Task<UserTaskInfo> ClaimTask(string taskId, string user)
        {
            await camunda.UserTasks[taskId].Claim(user);
            var task = await camunda.UserTasks[taskId].Get();
            return task;
        }

        public async Task CleanupProcessInstances()
        {
            var instances = await camunda.ProcessInstances
                .Query(new ProcessInstanceQuery
                {
                    ProcessDefinitionKey = "Report_Creation_Def_Key"
                })
                .List();

            if (instances.Count > 0)
            {
                await camunda.ProcessInstances.Delete(new DeleteProcessInstances
                {
                    ProcessInstanceIds = instances.Select(i => i.Id).ToList()
                });
            }
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }

    public interface IBpmnService
    {
    }
}
