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
    
    public partial class InvoiceExtraCost
    {
        public long ID { get; set; }
        public Nullable<long> InvoiceID { get; set; }
        public Nullable<int> ExtraCostID { get; set; }
        public Nullable<decimal> ExtraCostValue { get; set; }
    
        public virtual Invoice Invoice { get; set; }
        public virtual ExtraCost ExtraCost { get; set; }
    }
}
