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
    
    public partial class Branch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Branch()
        {
            this.Branch1 = new HashSet<Branch>();
            this.BranchCategories = new HashSet<BranchCategory>();
            this.BranchItems = new HashSet<BranchItem>();
            this.BranchParticipations = new HashSet<BranchParticipation>();
            this.ExtraCosts = new HashSet<ExtraCost>();
            this.InvoiceItems = new HashSet<InvoiceItem>();
            this.Mails = new HashSet<Mail>();
            this.MarketEvaluations = new HashSet<MarketEvaluation>();
            this.Offers = new HashSet<Offer>();
            this.Users = new HashSet<User>();
        }
    
        public int ID { get; set; }
        public Nullable<int> ParentBranchID { get; set; }
        public string BranchNameAr { get; set; }
        public string BranchNameEn { get; set; }
        public string Phone { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<bool> Status { get; set; }
        public string AddressAr { get; set; }
        public string AddressEn { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public Nullable<long> UserLogID { get; set; }
        public Nullable<decimal> DistanceRange { get; set; }
        public string Logo { get; set; }
        public string DeliveryTime { get; set; }
        public Nullable<bool> Opened { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Branch> Branch1 { get; set; }
        public virtual Branch Branch2 { get; set; }
        public virtual Category Category { get; set; }
        public virtual UserLog UserLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BranchCategory> BranchCategories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BranchItem> BranchItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BranchParticipation> BranchParticipations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExtraCost> ExtraCosts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mail> Mails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MarketEvaluation> MarketEvaluations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Offer> Offers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
