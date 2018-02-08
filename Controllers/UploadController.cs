using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace play_api.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            System.Diagnostics.Debug.WriteLine(file.FileName);

            // foreach(var formFile in files) {
            //     if (formFile.Length > 0) {
            //         System.Diagnostics.Debug.WriteLine(formFile.FileName);
            //     }
            // }

            return Ok(new { Status = "Good" });
        }
    }
}
