using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.KingAttorney.ViewModels;
using WebAdmin_KingAnttorny.Bus;
using WebAdmin_KingAnttorny.Lib;
using API.KingAttorney.Middleware;
using Microsoft.AspNetCore.Http;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.KingAttorney.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        [HttpPost]
        public async Task<JsonResult> register(string username, string password, string email, string firstName, string lastName, int gender, DateTime birthday, string telephone, string address, int imageId = 0, IFormFile image_file = null)
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(telephone)
                || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                objError.code = 201;
                objError.message = "Data không hợp lệ";
                return await Task.Run(() => Json(new { result = 0, time = lTime, data = new { userId = 0, access_token = "" }, error = objError }));
            }
            int addressId = 0;
            int telephoneId = 0;
            int collectionId = 0;
            if(!string.IsNullOrEmpty(address))
                addressId = await new B_Address().Create(address);
            if (!string.IsNullOrEmpty(telephone))
                telephoneId = await new B_TelePhone().Create(telephone);

            var collection = await new B_Collection().ReadByName("nguoi_dung");
            if (collection != null)
            {
                collectionId = collection.Id;
             
                if (image_file != null)
                {
                    var imgHelper = new B_Image();
                    imageId = await imgHelper.UploaImg(image_file, "https://admin.kingattorney.net", "AppMobile");
                }

                var helper = new B_Account();
                var dataItem = await helper.Create(username, password, email, "nguoi_dung", "", firstName, lastName, gender, birthday, imageId, addressId, telephoneId, 0, collectionId);
                objError.message = dataItem["msg"];
                if (dataItem["result"] == "1")
                    return await Task.Run(() => Json(new { result = 1, time = lTime, data = new { userId = Convert.ToInt32(dataItem["userid"]), access_token = dataItem["access_token"] }, error = objError }));
            }
            objError.code = 201;
            objError.message = "Đăng ký không thành công";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = new { userId = 0, access_token = "" }, error = objError }));
        }

        [HttpPost]

        public async Task<JsonResult> Edit(string access_token, string first_name, string last_name, string email, string telephone, DateTime dob, string address_text, int gender, int status, int imageId=0, IFormFile image_file=null)
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();

            if (string.IsNullOrEmpty(access_token) || string.IsNullOrEmpty(first_name) || string.IsNullOrEmpty(last_name) || string.IsNullOrEmpty(telephone) || string.IsNullOrEmpty(email))
                return await Task.Run(() => Json(new { result = 0, time = lTime, data = 0, error = "Dữ liệu không hợp lệ" }));

            var user = TokenProvider.DecodeToken(access_token);
            if (user == null) return await Task.Run(() => Json(new { result = 0, time = lTime, data = 0, error = "Access Token không hợp lệ" }));

            int addressId = 0;
            int collectionId = 0;
            int telephoneId = 0;
            var helper = new B_Account();
            var item = helper.ReadAccount(user.user.id);
            if (item == null) return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = "User không tồn tại" }));
            if (item.email != email)
            {
                if (!VIConvert.IsValidEmail(email))
                    return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = "Email không hợp lệ " }));
                if (helper.IsExistEmail(email))
                    return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = "Email đã tồn tại" }));
            }

            if (item.phone_id == 0)
            {

                var telHelper = new B_TelePhone();
                if (telHelper.isExist(telephone))
                    return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = "Số điện thoại đã tồn tại" }));
                telephoneId = await telHelper.Create(telephone);
            }
            else
            {
                if (item.phone != telephone)
                {
                    var telHelper = new B_TelePhone();
                    if (telHelper.isExist(telephone))
                        return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = "Số điện thoại đã tồn tại" }));
                    var result = await telHelper.Update(item.phone_id, telephone);
                }
                telephoneId = item.phone_id;
            }


            var addHelper = new B_Address();

            if (item.address_id == 0)
            {
                addressId = await addHelper.Create(address_text);
            }
            else
            {
                await addHelper.Update(item.address_id, address_text);
                addressId = item.address_id;
            }

            var colHelepr = new B_Collection();
            var collectionItem = await colHelepr.ReadByName("nguoi_dung");
            if (collectionItem != null)
                collectionId = collectionItem.Id;

            if (item.img_id != 0)
            {
                if (image_file != null)
                {
                    var imgHelper = new B_Image();
                    await imgHelper.UpdateImg(image_file, "https://admin.kingattorney.net", item.img_id, "AppMobile");
                    imageId = item.img_id;
                }
                
            }
            else
            {
                if (image_file != null)
                {
                    var imgHelper = new B_Image();
                    imageId = await imgHelper.UploaImg(image_file, "https://admin.kingattorney.net", "AppMobile");
                }
            }

            var dataItems = await helper.Update(user.user.id, email, "nguoi_dung", "", first_name, last_name, gender, dob, imageId, addressId, telephoneId, 0, collectionId, status, 0, "", "", "", "", "");

            if (dataItems["result"] == "1")
                return await Task.Run(() => Json(new { result = 1, time = lTime, data = dataItems, error = "" }));
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = dataItems["msg"] }));
        }

        [HttpPost]
        public async Task<JsonResult> change_password(string access_token, string password_old, string password_new, string account_type= "nguoi_dung")
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();

            if (string.IsNullOrEmpty(access_token))
            {
                objError.code = 201;
                objError.message = "Dữ liệu chưa hợp lệ";
                return await Task.Run(() => Json(new { result = 0, time = lTime, data = objError.message, error = objError }));

            }

            var userItem = TokenProvider.DecodeToken(access_token);
            if (userItem == null)
            {
                objError.code = 201;
                objError.message = "Dữ liệu chưa hợp lệ";
                return await Task.Run(() => Json(new { result = 0, time = lTime, data = objError.message, error = objError }));

            }
            var username = userItem.user.user;


            var collection = await new B_Collection().ReadByName(account_type);
            int collectionId = 0;
            if (collection != null)
            {
                collectionId = collection.Id;

                var helper = new B_Account();
                var dataItem = await helper.UpdatePassword(username, password_old, password_new, collectionId);
                if (dataItem > 0)
                {
                    return await Task.Run(() => Json(new { result = 1, time = lTime, data = "Đổi mật khẩu thành công", error = objError }));
                }
            }
            objError.code = 201;
            objError.message = "UserName / Password chưa chính xác";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = objError.message, error = objError }));
        }


        [HttpPost]
        public async Task<JsonResult> reset_password(string access_token, string account_type= "nguoi_dung")
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();
            if(string.IsNullOrEmpty(access_token))
            {
                objError.code = 201;
                objError.message = "Dữ liệu không hợp lệ";
                return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
            }

            var userItem = TokenProvider.DecodeToken(access_token);
            if (userItem == null)
            {
                objError.code = 201;
                objError.message = "Dữ liệu không hợp lệ";
                return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));

            }
            var username = userItem.user.user;
            var collection = await new B_Collection().ReadByName(account_type);
            int collectionId = 0;
            if (collection != null)
            {
                collectionId = collection.Id;

                var helper = new B_Account();
               
               
                var dataItem = await helper.ResetPassword(username,collectionId);
                objError.message = dataItem["msg"];
                if (dataItem["result"] == "1")
                {
                    return await Task.Run(() => Json(new { result = 1, time = lTime, data = new object(), error = objError }));
                }
                else
                    return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
            }
            objError.code = 201;
            objError.message = "Dữ liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
        }

        [HttpPost]
        public async Task<JsonResult> get_user_token(string user_name, string account_type = "nguoi_dung")
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();
            if (string.IsNullOrEmpty(user_name))
            {
                objError.code = 201;
                objError.message = "Dữ liệu không hợp lệ";
                return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));

            }


            var collection = await new B_Collection().ReadByName(account_type);
            int collectionId = 0;
            if (collection != null)
            {
                collectionId = collection.Id;

                var helper = new B_Account();


                var dataItem = await helper.Read(user_name,collectionId);
                if(dataItem ==null)
                    return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
                var accessToken = TokenProvider.getEncode_To_Token(dataItem.Id, dataItem.UserName);
                return await Task.Run(() => Json(new { result = 1, time = lTime, data = new { access_token = accessToken }, error = objError }));
            }
            objError.code = 201;
            objError.message = "Dữ liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
        }


        [HttpPost]
        public async Task<JsonResult> contact_out(string name, string email, string subject,string content)
        {
            VMError objError = new VMError();
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email)
                || !VIConvert.IsValidEmail(email) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(content))
            {
                objError.code = 201;
                objError.message = "Dữ liệu không hợp lệ";
                return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
            }

            var helper = new B_Account();
            var dataItem = helper.sendEmail("info@h2aits.com","System",subject,name, "info@kingattorney.net", content,"0");
            string content2 = "<p>Chúng tôi đã nhận được thông tin của Anh/Chị </p> <p> Chúng tôi sẽ liên hệ với anh chị sớm nhất có thể </p>" +"<hr/> <p> " + content  + " </p>";
            var dataItem2 = helper.sendEmail("info@h2aits.com", "System", subject, name, email, content2, "0");
               
            if (dataItem2)
            {
                objError.message = "Đã gửi email thành công";
                return await Task.Run(() => Json(new { result = 1, time = lTime, data = new object(), error = objError }));
            }
            else
            {
                objError.message = "Gửi email không thành công";
                return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
            }
            
        }


        [HttpPost]
        public async Task<JsonResult> login (string username, string password)
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();
            int collectionId = 0;
            var collection = await new B_Collection().ReadByName("nguoi_dung");
            if (collection != null)
            {
                collectionId = collection.Id;
                var helper = new B_Account();
                var dataItem = await helper.SignIn(username, password, collectionId);
                if (dataItem !=null )
                {
                    return await Task.Run(() => Json(new { result = 1, time = lTime, data = dataItem, error = objError }));
                }
            }

            objError.code = 201;
            objError.message = "Dữ liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
        }

        [HttpPost]
        public async Task<JsonResult> info(string access_token)
        {
            long lTime = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUniversalTime().ToUnixTimeSeconds();
            VMError objError = new VMError();
            if (string.IsNullOrEmpty(access_token))
            {
                objError.code = 201;
                objError.message = "Dữ liệu không hợp lệ";
                return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
            }
            var userItem = TokenProvider.DecodeToken(access_token);
            if(userItem ==null)
            {
                objError.code = 201;
                objError.message = "Dữ liệu không hợp lệ";
                return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
            }

            int collectionId = 0;
            var collection = await new B_Collection().ReadByName("nguoi_dung");
            if (collection != null)
            {
                collectionId = collection.Id;
                var helper = new B_Account();
                var dataItem =  helper.ReadAccount(userItem.user.id);
                if (dataItem != null)
                {
                    return await Task.Run(() => Json(new { result = 1, time = lTime, data = dataItem, error = objError }));
                }
            }

            objError.code = 201;
            objError.message = "Dữ liệu không hợp lệ";
            return await Task.Run(() => Json(new { result = 0, time = lTime, data = new object(), error = objError }));
        }
    }
}
