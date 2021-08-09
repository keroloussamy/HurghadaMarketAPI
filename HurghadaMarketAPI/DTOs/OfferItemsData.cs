using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurghadaMarketAPI.DTOs
{
    public class OfferItemsData
    {
        public long ID { get; set; }
        public decimal? Quantity { get; set; }
        public int? OrderID { get; set; }
        public string PicURL { get; set; }
        public string ItemNameAr { get; set; }
        public string ItemNameEn { get; set; }
    }
}