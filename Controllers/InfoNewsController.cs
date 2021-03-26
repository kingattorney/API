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
    public class InfoNewsController : Controller
    {
        public async Task<JsonResult> get_news(int size, int page_number)
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();

            var helper = new InfoNewsHelper();
            var dataItem = await helper.GetNews("tin_tuc", size, page_number);
            if (dataItem != null)
            {
                return await Task.Run(() => Json(new { result = 1, time = lTime, data = dataItem, error = objError }));
            }

            objError.code = 201;
            objError.message = "Dữ liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = dataItem, error = objError }));
        }

        public async Task<JsonResult> get_app_info()
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();

            var helper = new InfoNewsHelper();
            var items = await helper.GetNews("thong_tin_ung_dung");
            if (items != null && items.Any())
               
            if (items != null)
            {
                var dataItem = items.OrderByDescending(o => o.id).FirstOrDefault();
                    return await Task.Run(() => Json(new { result = 1, time = lTime, data = dataItem, error = objError }));
            }

            objError.code = 201;
            objError.message = "Dữ liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
        }

        public async Task<JsonResult> get_info(string keyword, int records)
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();

            var helper = new InfoNewsHelper();
            var items = await helper.GetNews(keyword, records);
            if (items != null && items.Any())

                if (items != null)
                {
                    var dataItem = items.OrderByDescending(o => o.id).FirstOrDefault();
                    return await Task.Run(() => Json(new { result = 1, time = lTime, data = dataItem, error = objError }));
                }

            objError.code = 201;
            objError.message = "Dữ liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
        }



        public async Task<JsonResult> get_promotion()
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();

            var helper = new InfoNewsHelper();
            var dataItem = await helper.GetNews("chuong_trinh_khuyen_mai");
            if (dataItem != null)
            {
                return await Task.Run(() => Json(new { result = 1, time = lTime, data = dataItem, error = objError }));
            }

            objError.code = 201;
            objError.message = "Dữ liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = dataItem, error = objError }));
        }
    }
}
