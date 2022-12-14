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
    public static class Results
    {
        [FunctionName("Results")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string connectionString = Environment.GetEnvironmentVariable("OhStorageConStr", EnvironmentVariableTarget.Process);

            // Create a BlobServiceClient object 
            var blobServiceClient = new BlobServiceClient(connectionString);
            //   var blobServiceClient = new BlobServiceClient(
            //new Uri("https://<storage-account-name>.blob.core.windows.net"),
            //new DefaultAzureCredential());
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("mydefaultcontainer");
            BlobClient blobClient = containerClient.GetBlobClient("Test");
            string test = "Testing 1-2-3";

            // convert string to stream
            byte[] byteArray = Encoding.ASCII.GetBytes(test);
            MemoryStream stream = new MemoryStream(byteArray);

            blobClient.DeleteIfExists(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.None);
            blobClient.Upload(stream);
            return new OkObjectResult("ok");
        }
    }
}
