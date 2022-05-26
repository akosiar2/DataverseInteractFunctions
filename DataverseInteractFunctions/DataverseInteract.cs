using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;
using System.Collections.Generic;
using DataverseInteractFunctions.Dto;
using DataverseInteractFunctions.Services;

namespace DataverseInteractFunctions
{
    public static class DataverseInteract
    {
        
        [FunctionName("PostData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, // TimeEntry timeEntry,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if (string.IsNullOrEmpty(requestBody))
                return new BadRequestObjectResult("Please provide a model in the request body");

            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string[] dateRange = data?.required.ToObject<string[]>();
            DateTime startOn = Convert.ToDateTime(dateRange[0]);
            DateTime endOn = Convert.ToDateTime(dateRange[1]);
            //DateTime startOn = Convert.ToDateTime(timeEntry.required[0]);
            //DateTime endOn = Convert.ToDateTime(timeEntry.required[1]);

            var ids = await DataverseService.PostTimeEntry(startOn, endOn);

            string responseMessage = $"Time Entries Interacted";
            
            return new OkObjectResult(responseMessage);
        }

        
    }
}
