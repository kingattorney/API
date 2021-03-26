using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.KingAttorney.Bus;
using API.KingAttorney.ViewModels;
using WebAdmin_KingAnttorny.Bus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API.KingAttorney.Middleware;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.KingAttorney.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthenticationController : Controller
    {
        /*
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAdminBo _adminBo;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAdminBo adminBo)
        {
            _logger = logger;
            _adminBo = adminBo;
        }
        */
        [HttpPost]
        public async Task<JsonResult> Authentication(string userName, string publicKey)
        {

            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();

            var helper = new B_Encryption();
            var dataItem = await helper.getToken(userName, publicKey);
            if (!string.IsNullOrEmpty(dataItem))
            {
                return await Task.Run(() => Json(new { result = 1, time = lTime, data = dataItem, error = objError }));
            }

            objError.code = 201;
            objError.message = "Dữ liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
        }


            [HttpPost]
        public async Task<JsonResult> get_token(string userName, string publicKey)
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();

            var helper = new B_Encryption();
            var dataItem = await helper.getToken(userName, publicKey);
            if (!string.IsNullOrEmpty(dataItem))
            {
                return await Task.Run(() => Json(new { result = 1, time = lTime, data = dataItem, error = objError }));
            }

            objError.code = 201;
            objError.message = "Dữ liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
        }
    }
}
