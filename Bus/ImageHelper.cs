using System;
using System.IO;
using System.Threading.Tasks;
using WebAdmin_KingAnttorny.Bus;
namespace API.KingAttorney.Bus
{
    public class ImageHelper
    {
        public async Task<int> Upload(string imgBase64, string hostname, string description)
        {
            if (string.IsNullOrEmpty(imgBase64)) return 0;
            string imageName = Guid.NewGuid().ToString() + ".jpg";
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads/Images");
            string SavePath = Path.Combine(uploadPath, imageName);
            string fullPath = Path.Combine(hostname, "Uploads/Images", imageName);

            try
            {
                if (!System.IO.Directory.Exists(uploadPath))
                {
                    System.IO.Directory.CreateDirectory(uploadPath); //Create directory if it doesn't exist
                }

                byte[] imageBytes = Convert.FromBase64String(imgBase64);

                File.WriteAllBytes(SavePath, imageBytes);
                return await new B_Image().Create("AppMobile", hostname, description, imageName);
            }
            catch(Exception ex)
            {
                return 0;
            }
           
           
        }
    }
}
