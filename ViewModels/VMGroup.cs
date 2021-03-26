using System;
using System.Collections.Generic;
namespace API.KingAttorney.ViewModels
{
    public class VMGroup
    {
        public int id { get; set; }
        public int collectionId { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public int order { get; set; }

    }


    public class VMGroupTitle
    {
        public string title { get; set; }
        public List<VMGroup> data_items { get; set; }
    }
}
