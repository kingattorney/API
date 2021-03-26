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
    public class SearchController : Controller
    {
        
        [HttpPost]
        public async Task<JsonResult> search_document(string content, string type, int page_number=1, int page_size=10)
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();

            var helper = new B_Content();
            var dataItem = await helper.search(content,type, page_number, page_size);
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
