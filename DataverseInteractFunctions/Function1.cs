using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;
using System.Collections.Generic;
using DataverseInteractFunctions.dto;

namespace DataverseInteractFunctions
{
    public static class Function1
    {
        [FunctionName("TestDataverse")]
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

            using var serviceClient = DataverseConnection();

            var time_entries = await serviceClient.RetrieveMultipleAsync(new QueryExpression("msdyn_timeentry")
            {
                ColumnSet = new ColumnSet("msdyn_start", "msdyn_end" )
            });
            var date_entry = time_entries.Entities.Select(x => $"{x.GetAttributeValue<DateTime>("msdyn_start").ToShortDateString()}").ToList();

            List<Guid> return_id = new List<Guid>();
            foreach (DateTime date in AllDatesBetween(startOn, endOn))
            {
                
                if (!date_entry.Contains(date.ToShortDateString()))
                {
                    Entity time_entry = new Entity("msdyn_timeentry");
                    {
                        time_entry["msdyn_start"] = date;
                        //time_entry["msdyn_end"] = endOn;
                        time_entry["msdyn_date"] = date;
                        time_entry["msdyn_duration"] = 1;

                    }

                    return_id.Add(await serviceClient.CreateAsync(time_entry));
                }
            }

            string responseMessage = $"Time Entries Interacted";
            
            return new OkObjectResult(responseMessage);
        }

        static IEnumerable<DateTime> AllDatesBetween(DateTime start, DateTime end)
        {
            for (var day = start.Date; day <= end; day = day.AddDays(1))
                yield return day;
        }

        private static ServiceClient DataverseConnection()
        {
            string crmurl = "https://orga8781aa6.crm.dynamics.com/", username = "ArvinAgramon@RentReadyTest228.onmicrosoft.com", password = "wangbO0$.0987";
            var connectionString = $"AuthType=OAuth;Username={username};Password={password};Url={crmurl};AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;TokenCacheStorePath=c:\\MyTokenCache;LoginPrompt=Auto";

            var service = new ServiceClient(connectionString);
            return service;
        }
    }
}
