using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;

namespace Identity.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public ImagesController(IWebHostEnvironment env)
        {
            
            _env = env;
        }

        //[Authorize]
        [HttpGet]
        [Route("images/{fileName}.{ext?}")]
        public IActionResult GetImage(string fileName, string ext)
        {
            if (ext == null)
                return NotFound();

            var user = HttpContext.User.Identity.Name;
            string clientPath;

            // TODO: Tennat ID
            switch(user) {
                case "user@clienta.com":
                    clientPath = "ClientA";
                    break;
                case "user@clientb.com":
                    clientPath = "ClientB";
                    break;
                default:
                    clientPath = "default";
                    break;
            }

            var filePath = Path.Combine(
                _env.ContentRootPath, "BrandedContent", clientPath, "images", $"{fileName}.{ext}");

            PhysicalFileResult file;

            try
            {
                file = PhysicalFile(filePath, "image/jpeg");
            }
            catch (Exception)
            {
                return NotFound();
            }

            //const int durationInSeconds = 60;
            //Response.Headers[HeaderNames.CacheControl] =
            //    "public,max-age=" + durationInSeconds;

            Response.Headers[HeaderNames.CacheControl] =
                "no-store,no-cache";

            return file;
        }
    }
}