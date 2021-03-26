using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.KingAttorney.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using API.KingAttorney.Bus;
using API.KingAttorney.Middleware;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.KingAttorney.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ImageBase64Controller : Controller
    {
        [HttpPost]
        public async Task<JsonResult> UploadImage(string imgBase64)
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();
            if(string.IsNullOrEmpty(imgBase64))
            {
                objError.code = 201;
                objError.message = "image is empty";
                return await Task.Run(() => Json(new { result = 0, time = lTime, data =  new { imageId = 0 }, error = objError }));
            }

            int imgId = await new ImageHelper().Upload(imgBase64, "https://admin.kingattorney.net", "Image Content");
            if (imgId ==0)
            {
                objError.code = 201;
                objError.message = "upload fail";
                return await Task.Run(() => Json(new { result = 0, time = lTime, data = new { imageId = 0 }, error = objError }));
            }
            return await Task.Run(() => Json(new { result = 1, time = lTime, data = new { imageId = imgId }, error = objError }));
          
        }

        

    }
}
