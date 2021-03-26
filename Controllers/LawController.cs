using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.KingAttorney.ViewModels;
using WebAdmin_KingAnttorny.Bus;
using API.KingAttorney.Bus;
using API.KingAttorney.Middleware;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.KingAttorney.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class LawController : Controller
    {
        [HttpPost]
        public async Task<JsonResult> getcategory(string name)
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();

            var helper = new GroupHelper();
            var dataItem = await helper.Reads(name,1);
            if (dataItem != null)
            {
                return await Task.Run(() => Json(new { result = 1, time = lTime, title = dataItem.title, data = dataItem.data_items, error = objError }));
            }

            objError.code = 201;
            objError.message = "Dư liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, title=dataItem.title,  data = dataItem.data_items, error = objError }));
        }

        [HttpPost]
        public async Task<JsonResult> getsubcategory(int id, int level =1)
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();

            var helper = new GroupHelper();
            var dataItem = await helper.ReadSubItem(id, 1, level);
            if (dataItem != null)
            {
                return await Task.Run(() => Json(new { result = 1, time = lTime, title = dataItem.title, data = dataItem.data_items, error = objError }));
            }

            objError.code = 201;
            objError.message = "Dư liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, title = dataItem.title, data =  dataItem.data_items, error = objError }));
        }
        [HttpPost]
        public async Task<JsonResult> getcontent(int id)
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();

            var helper = new B_Content();
            var dataItem = await helper.Read(id);
            if (dataItem != null)
            {
                return await Task.Run(() => Json(new { result = 1, time = lTime, data = dataItem, error = objError }));
            }

            objError.code = 201;
            objError.message = "Dư liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = new List<object>(), error = objError }));
        }
    }
}

