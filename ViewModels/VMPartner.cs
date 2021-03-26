using System;
namespace API.KingAttorney.ViewModels
{
    public class VMPartner
    {
        public int id { get; set; }
        public string name { get; set; }
        public string business { get; set; }
        public int content_id { get; set; }
        public string description { get; set; }
        public string content_detail { get; set; }
        public int avatar_id { get; set; }
        public string avatar_url { get; set; }
        public string google_plus_url { get; set; }
        public string facebook_fanpage_url { get; set; }
        public string twitter_fanpage_url { get; set; }
        public int telephone_id { get; set; }
        public string telephone { get; set; }
        public string zalo { get; set; }
        public int address_id { get; set; }
        public string address { get; set; }
    }
}
