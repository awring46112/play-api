using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using Microsoft.Azure; //Namespace for CloudConfigurationManager
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace play_api.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi=true)]
    public class UploadController : Controller
    {
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            var Configuration = builder.Build();


            System.Diagnostics.Debug.WriteLine(file.FileName);


            // Parse the connection string and return a reference to the storage account.


            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                Configuration["StorageConnectionString"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(
                new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("myblob");

            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = file.OpenReadStream())
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }



            // foreach(var formFile in files) {
            //     if (formFile.Length > 0) {
            //         System.Diagnostics.Debug.WriteLine(formFile.FileName);
            //     }
            // }

            return Ok(new { Status = "Good" });
        }
    }
}
