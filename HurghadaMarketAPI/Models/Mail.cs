//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HurghadaMarketAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mail
    {
        public long ID { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<long> CustomerID { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public Nullable<bool> Readed { get; set; }
        public Nullable<System.DateTime> SendDate { get; set; }
        public Nullable<System.DateTime> ViewDate { get; set; }
        public Nullable<bool> FromAdmin { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<bool> Sent { get; set; }
    
        public virtual Branch Branch { get; set; }
        public virtual Customer Customer { get; set; }
    }
}