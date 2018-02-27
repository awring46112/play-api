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
using TodoApi.Parsers;
using TodoApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;

namespace play_api.Controllers
{
    [Route("api/[controller]")]
    public class GetSDFileInfoController : Controller
    {
        [HttpPost("GetSDFileInfo")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<SDFileItem>))]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            var Configuration = builder.Build();

            var filePath = Path.GetTempFileName();

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                SDFileParser parser = new SDFileParser(filePath);
                var items = parser.Parse().ToList();

                foreach (var item in items)
                {
                    item.SVG = GetSVG(item);
                }

                return Ok(items);
            }
            finally
            {
                System.IO.File.Delete(filePath);
            }
        }

        string GetSVG(SDFileItem item)
        {

            var filePath = Path.GetTempFileName();
            System.IO.File.WriteAllText(filePath, item.CTab);

            try
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "molconvert",
                        Arguments = $"svg:w100headless \"{filePath}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }

                };

                process.Start();
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return result;
            }
            finally
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
