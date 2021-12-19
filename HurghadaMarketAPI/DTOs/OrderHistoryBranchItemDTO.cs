using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurghadaMarketAPI.DTOs
{
    public class OrderHistoryBranchItemDTO
    {
        public string ItemName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Total { get; set; }

    }
}