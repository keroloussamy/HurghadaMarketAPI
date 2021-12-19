using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurghadaMarketAPI.DTOs
{
    public class OrderHistoeryDTO
    {
        
        public string Date { get; set; }
        public string Time { get; set; }
        public List<OrderHistoryBranchDTO> OrderHistoryBranchs { get; set; }

    }
}