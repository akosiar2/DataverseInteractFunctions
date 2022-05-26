using Microsoft.PowerPlatform.Dataverse.Client;

namespace DataverseInteractFunctions.Repository
{
    public static class DataverseRepository
    {
        public static ServiceClient DataverseConnection()
        {
            string crmurl = "https://orga8781aa6.crm.dynamics.com/", username = "ArvinAgramon@RentReadyTest228.onmicrosoft.com", password = "wangbO0$.0987";
            var connectionString = $"AuthType=OAuth;Username={username};Password={password};Url={crmurl};AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;RedirectUri=http://localhost;TokenCacheStorePath=c:\\MyTokenCache;LoginPrompt=Auto";

            var service = new ServiceClient(connectionString);
            return service;
        }
    }
}
