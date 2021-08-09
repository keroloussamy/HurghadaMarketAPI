using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HurghadaMarketAPI.DTOs
{
    public class JsonResult<T> where T : class
    {
        public JsonResult()
        {
            Result = new List<T>();
        }
        public string Message { get; set; }
        public List<T> Result { get; set; }
    }
}
