using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HurghadaMarketAPI.DTOs
{
    public class CarpetItemsDTO
    {
        public long? ID { get; set; }
        public string ItemType { get; set; }
        public string Name { get; set; }
        public decimal? Quantity { get; set; }
        public string PicUrl { get; set; }
        public bool? Divisible { get; set; }
        public decimal? Price { get; set; }
        public bool Available { get; set; }
        public bool PriceUpdated { get; set; }
        public bool Ended { get; set; }
    }
}
