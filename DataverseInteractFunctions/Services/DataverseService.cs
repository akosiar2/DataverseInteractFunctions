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
using DataverseInteractFunctions.Dto;
using DataverseInteractFunctions.Repository;
using DataverseInteractFunctions.Helpers;

namespace DataverseInteractFunctions.Services
{
    public static class DataverseService
    {
        public static async Task<List<Guid>> PostTimeEntry(DateTime startOn, DateTime endOn)
        {
            using var serviceClient = DataverseRepository.DataverseConnection();

            var time_entries = await serviceClient.RetrieveMultipleAsync(new QueryExpression("msdyn_timeentry")
            {
                ColumnSet = new ColumnSet("msdyn_start", "msdyn_end")
            });
            var date_entry = time_entries.Entities.Select(x => $"{x.GetAttributeValue<DateTime>("msdyn_start").ToShortDateString()}").ToList();

            List<Guid> return_id = new List<Guid>();
            foreach (DateTime date in Utility.AllDatesBetween(startOn, endOn))
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

            return return_id;
        }
    }
}
