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
    
    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            this.BranchItems = new HashSet<BranchItem>();
            this.InvoiceItems = new HashSet<InvoiceItem>();
            this.OfferItems = new HashSet<OfferItem>();
        }
    
        public long ID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string ItemNameAr { get; set; }
        public string ItemNameEn { get; set; }
        public string PicURL { get; set; }
        public string DetailsAr { get; set; }
        public string DetailsEn { get; set; }
        public Nullable<bool> Divisible { get; set; }
        public Nullable<long> UserLogID { get; set; }
        public Nullable<bool> Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BranchItem> BranchItems { get; set; }
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual UserLog UserLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferItem> OfferItems { get; set; }
    }
}