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

namespace Amp.Test
{
    public static class Results
    {
        [FunctionName("Results")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string connectionString = "DefaultEndpointsProtocol=https;AccountName=ohconnectdevstorage;AccountKey=U9ZP7uaNh5nyHaGUa4GsZRBG8rjzbaWvTdtbf9v1Wdu1839JAqg3O6nJIW0mGZT6LOIqrMtwytVO+ASt1DTFuA==;EndpointSuffix=core.windows.net";

            // Create a BlobServiceClient object 
            var blobServiceClient = new BlobServiceClient(connectionString);
            //   var blobServiceClient = new BlobServiceClient(
            //new Uri("https://<storage-account-name>.blob.core.windows.net"),
            //new DefaultAzureCredential());
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("mydefaultcontainer");
            using (var stream = new MemoryStream())
            {

                stream.Position = 0;
                using (var sw = new StreamWriter(stream))
                {
                    sw.Write("K");
                }

                containerClient.UploadBlob("Test", stream);
            }
            return new OkObjectResult("ok");
        }
    }
}
