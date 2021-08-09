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
    
    public partial class Invoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice()
        {
            this.InvoiceItems = new HashSet<InvoiceItem>();
        }
    
        public long ID { get; set; }
        public string InvoiceCode { get; set; }
        public Nullable<long> CustomerID { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<System.TimeSpan> RequestTime { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<long> UserLogID { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> Served { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<bool> Carpet { get; set; }
        public string Address { get; set; }
    
        public virtual Branch Branch { get; set; }
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual UserLog UserLog { get; set; }
    }
}
