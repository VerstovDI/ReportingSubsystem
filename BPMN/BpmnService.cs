﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camunda.Api.Client;
using Camunda.Api.Client.Deployment;
using Camunda.Api.Client.Message;
using Camunda.Api.Client.ProcessDefinition;
using Camunda.Api.Client.ProcessInstance;
using Camunda.Api.Client.UserTask;

namespace ReportingSubsystem.BPMN
{
    public class BpmnService
    { 
        private readonly CamundaClient camunda;

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

        /*public async Task<string> StartProcessFor(Order order)
        {
            var processParams = new StartProcessInstance()
                .SetVariable("orderId", VariableValue.FromObject(order.Id.Value.ToString()))
                .SetVariable("orderStatus", VariableValue.FromObject(order.Status.ToString()))
                .SetVariable("customerCode", VariableValue.FromObject(order.Customer.Code));

            processParams.BusinessKey = order.Id.Value.ToString();

            var processStartResult = await
                camunda.ProcessDefinitions.ByKey("Process_Hire_Hero").StartProcessInstance(processParams);

            return processStartResult.Id;
        }*/

        public async Task<List<UserTaskInfo>> GetTasksForCandidateGroup(string group, string user)
        {
            var groupTaskQuery = new TaskQuery
            {
                ProcessDefinitionKeys = { "Process_Hire_Hero" },
                CandidateGroup = group
            };
            var groupTasks = await camunda.UserTasks.Query(groupTaskQuery).List();

            if (user != null)
            {
                var userTaskQuery = new TaskQuery
                {
                    ProcessDefinitionKeys = { "Process_Hire_Hero" },
                    Assignee = user
                };
                var userTasks = await camunda.UserTasks.Query(userTaskQuery).List();

                groupTasks.AddRange(userTasks);
            }

            return groupTasks;
        }

        public async Task<UserTaskInfo> ClaimTask(string taskId, string user)
        {
            await camunda.UserTasks[taskId].Claim(user);
            var task = await camunda.UserTasks[taskId].Get();
            return task;
        }

        /*public async Task<UserTaskInfo> CompleteTask(string taskId, Order order)
        {
            var task = await camunda.UserTasks[taskId].Get();
            var completeTask = new CompleteTask()
                .SetVariable("orderStatus", VariableValue.FromObject(order.Status.ToString()));
            await camunda.UserTasks[taskId].Complete(completeTask);
            return task;
        }

        public async Task SendMessageInvoicePaid(Order order)
        {
            await camunda.Messages.DeliverMessage(new CorrelationMessage
            {
                BusinessKey = order.Id.Value.ToString(),
                MessageName = "Message_InvoicePaid"
            });
        }*/

        public async Task CleanupProcessInstances()
        {
            var instances = await camunda.ProcessInstances
                .Query(new ProcessInstanceQuery
                {
                    ProcessDefinitionKey = "Process_Hire_Hero"
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
    }
}
