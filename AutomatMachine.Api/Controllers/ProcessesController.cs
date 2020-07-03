using AutomatMachine.Common.Request;
using AutomatMachine.Common.Response;
using AutomatMachine.Data;
using AutomatMachine.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AutomatMachine.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessesController : ControllerBase
    {
        private readonly IProcessService _processService;
        public ProcessesController(IProcessService processService)
        {
            _processService = processService;
        }

        [HttpGet]
        [Route("")]
        public Process[] GetProcesses()
        {
            return _processService.GetProcesses();
        }

        [HttpGet]
        [Route("{id}")]
        public Process GetProcess(Guid id)
        {
            return _processService.GetProcess(id);
        }

        [HttpPost]
        [Route("{id}")]
        public Process Set(Guid id, [FromBody] SetProcessRequest request)
        {
           return _processService.UpdateProcess(id, request);
        }

        [HttpPost]
        [Route("{id}/payment")]
        public PaymentResponse Payment(Guid id, [FromBody] PaymentRequest request)
        {
            return _processService.Payment(id, request);
        }
    }
}
