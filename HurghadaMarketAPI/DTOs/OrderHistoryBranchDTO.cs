using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurghadaMarketAPI.DTOs
{
    public class OrderHistoryBranchDTO
    {
        public string BranchName { get; set; }
        public string Status { get; set; }
        public List<OrderHistoryBranchItemDTO> OrderHistoryBranchItems { get; set; }
    }
}