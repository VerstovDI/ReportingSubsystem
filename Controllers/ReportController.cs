using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReportingSubsystem.BPMN;
using ReportingSubsystem.Models;

namespace ReportingSubsystem.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger;
        private readonly ReportChoiceHandler reportChoiceHandler;
        private readonly BpmnService bpmnService;

        public ReportController(ILogger<ReportController> logger, BpmnService bpmnService)
        {
            _logger = logger;
            this.bpmnService = bpmnService;
        }
        public IActionResult ReportPage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChooseReportType([FromBody] Report report)
        {
            
            return Ok();
        }

        public async Task<IActionResult> ReportType()
        {
            ReportChoiceHandler reportChoiceHandler = new ReportChoiceHandler(bpmnService);
            Report newReport = new Report(1, "MM_Report", DateTime.Now, "CREATED");
            await reportChoiceHandler.Handle(newReport);
            return View();
        }
        // 
        // GET: /HelloWorld/Welcome/ 

        public string Welcome()
        {
            return "This is the Welcome action method...";
        }
    }
}
