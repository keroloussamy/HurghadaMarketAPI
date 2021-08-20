using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HurghadaMarketAPI.DTOs
{
    public class ExtraCostDTO
    {
        public string BranchName { get; set; }
        public decimal? ExtraValue { get; set; }
        public string ExtraTitle { get; set; }
        public int ID { get; set; }
    }
}