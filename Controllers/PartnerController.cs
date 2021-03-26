using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.KingAttorney.ViewModels;
using API.KingAttorney.Bus;
using API.KingAttorney.Middleware;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.KingAttorney.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class PartnerController : Controller
    {
       
        [HttpPost]
        public async Task<JsonResult> get_investor_person()
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();

            var helper = new AccountHelper();

            var dataItem = await helper.getPartner("ca_nhan",1);
            if (dataItem != null)
            {
                return await Task.Run(() => Json(new { result = 1, time = lTime,  data = dataItem, error = objError }));
            }

            objError.code = 201;
            objError.message = "Dư liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
        }

        [HttpPost]
        public async Task<JsonResult> get_investor_business()
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();

            var helper = new AccountHelper();

            var dataItem = await helper.getPartner("doanh_nghiep", 1);
            if (dataItem != null)
            {
                return await Task.Run(() => Json(new { result = 1, time = lTime, data = dataItem, error = objError }));
            }

            objError.code = 201;
            objError.message = "Dư liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
        }

        [HttpPost]
        public async Task<JsonResult> get_law()
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();

            var helper = new AccountHelper();

            var dataItem = await helper.getPartner("luat_su", 1);
            if (dataItem != null)
            {
                return await Task.Run(() => Json(new { result = 1, time = lTime, data = dataItem, error = objError }));
            }

            objError.code = 201;
            objError.message = "Dư liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
        }
    }
}
