using Microsoft.PowerPlatform.Dataverse.Client;
using System;

namespace DataverseInteractFunctions.Repository
{
    public static class DataverseRepository
    {
        public static ServiceClient DataverseConnection()
        {
            string crmurl = Environment.GetEnvironmentVariable("crmurl"), 
                username = Environment.GetEnvironmentVariable("usr"), 
                password = Environment.GetEnvironmentVariable("pwd"), 
                appId = Environment.GetEnvironmentVariable("appId"), 
                redirectUri = Environment.GetEnvironmentVariable("redirectUri");

            var connectionString = $"AuthType=OAuth;Username={username};Password={password};Url={crmurl};AppId={appId};RedirectUri={redirectUri};TokenCacheStorePath=c:\\MyTokenCache;LoginPrompt=Auto";

            var service = new ServiceClient(connectionString);
            return service;
        }
    }
}
