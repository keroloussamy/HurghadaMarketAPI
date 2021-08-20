using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HurghadaMarketAPI.DTOs
{
    public class BranchDTO
    {
        public int Id { get; set; }
        public string BranchName { get; set; }
        public string Logo { get; set; }
        public string DeliveryTime { get; set; }
        public bool? Opened { get; set; }

    }
}
