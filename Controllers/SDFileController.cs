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
using System.Diagnostics;

namespace play_api.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SDFileController : Controller
    {
        [HttpPost("UploadSDFile")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            var Configuration = builder.Build();


            System.Diagnostics.Debug.WriteLine(file.FileName);

            var filePath = Path.GetTempFileName();

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "standardize",
                        Arguments = $"-c /tmp/das-rules.xml \"{filePath}\" -f sdf -rp standardize-output",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }

                };

                process.Start();
                string result = await process.StandardOutput.ReadToEndAsync();
                process.WaitForExit();
                return Ok(result);
            }
            finally
            {
                System.IO.File.Delete(filePath);
            }


        }
    }
}
