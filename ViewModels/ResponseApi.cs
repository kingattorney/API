using System;
namespace API.KingAttorney.ViewModels
{
    public class ResponseApi
    {
        public ResponseApi()
        {
            StatusCode = 200;
            IsError = false;
            Message = "";
            Data = null;
        }
        public int StatusCode { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

    }
}
