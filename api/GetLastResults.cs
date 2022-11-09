using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using System.Text;

namespace Amp.Test
{
    public static class GetLastResults
    {
        [FunctionName("GetLastResults")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string connectionString = Environment.GetEnvironmentVariable("OhStorageConStr", EnvironmentVariableTarget.Process);
            

            var blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("mydefaultcontainer");
            BlobClient blobClient = containerClient.GetBlobClient("Test");

            
            string result = string.Empty;
            using (Stream stream = new MemoryStream())
            {
                
                using (StreamReader reader = new StreamReader(blobClient.OpenRead()))
                {
                    result = reader.ReadToEnd();
                }
            }
            return new OkObjectResult(result);
        }
    }
}
