using System;
namespace API.KingAttorney.ViewModels
{
    public class VMError
    {
        public int code { get; set; }
        public string message { get; set; }
        public VMError()
        {
            this.code = 200;
            this.message = "";
        }

        public VMError(int statusCode, string msg)
        {
            this.code = statusCode;
            this.message = msg;
        }
    }
}
