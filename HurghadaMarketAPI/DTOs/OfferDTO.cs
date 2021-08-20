
namespace HurghadaMarketAPI.DTOs
{
    public class OfferDTO
    {
        public long Id { get; set; }
        public string OfferName { get; set; }
        public decimal? OfferPrice { get; set; }
        public decimal? OrPrice { get; set; }
        public long BranchID { get; set; }
        public string Pic { get; set; }
    }
}
