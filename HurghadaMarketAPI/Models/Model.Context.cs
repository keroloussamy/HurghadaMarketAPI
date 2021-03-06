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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HurghadaMarketEntities : DbContext
    {
        public HurghadaMarketEntities()
            : base("name=HurghadaMarketEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<BranchCategory> BranchCategories { get; set; }
        public virtual DbSet<BranchItem> BranchItems { get; set; }
        public virtual DbSet<BranchParticipation> BranchParticipations { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public virtual DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<ItemEval> ItemEvals { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Mail> Mails { get; set; }
        public virtual DbSet<MarketEvaluation> MarketEvaluations { get; set; }
        public virtual DbSet<OfferItem> OfferItems { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserLog> UserLogs { get; set; }
        public virtual DbSet<ExtraCost> ExtraCosts { get; set; }
        public virtual DbSet<InvoiceBranchServed> InvoiceBranchServeds { get; set; }
        public virtual DbSet<InvoiceExtraCost> InvoiceExtraCosts { get; set; }
    }
}
