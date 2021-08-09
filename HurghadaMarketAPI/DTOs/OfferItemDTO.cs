using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HurghadaMarketAPI.DTOs
{
    public class OfferItemDTO
    {
        public long? Id { get; set; }
        public string ItemName { get; set; }
        public string PicUrl { get; set; }
        public decimal? Quantity { get; set; }
    }
}
