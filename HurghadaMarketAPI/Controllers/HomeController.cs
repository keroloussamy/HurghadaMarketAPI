using HurghadaMarketAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Http.Description;
using HurghadaMarketAPI.DTOs;
using System.Web.Services.Description;
using System.Data.Entity.Core.Objects;
using System.Web.Http.Cors;


namespace HurghadaMarketAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HomeController : ApiController
    {
        private HurghadaMarketEntities _context = new HurghadaMarketEntities();

        // Register
        [HttpPost]
        [Route("PostCustomer")]
        public async Task<IHttpActionResult> PostCustomer(PostCustomerDTO customer)
        {
            try
            {
                Customer newCustomer;
                if (_context.Customers.Any(x => x.Phone == customer.Phone))
                {
                    newCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Phone == customer.Phone);
                    newCustomer.CustomerName = customer.CustomerName;
                    _context.Entry(newCustomer).State = EntityState.Modified;
                }
                else
                {
                    newCustomer = new Customer { Status = true, CustomerName = customer.CustomerName, Phone = customer.Phone, 
                        CustomerAddresses = new List<CustomerAddress> { new CustomerAddress { Address = customer.Address } } 
                    };
                    _context.Customers.Add(newCustomer);
                }
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Item Created Successfully", Id = newCustomer.ID });
            }
            catch (Exception ex)
            { return BadRequest(ex.Message); }

        }

        // 1+ Branches’ Page ------------------------------------------------------------
        //http://localhost:60785/GetBranches?language=ar
        //http://localhost:60785/GetBranches
        [Route("GetBranches")]
        [HttpGet]
        public async Task<IHttpActionResult> GetBranches(string language = "en")
        {
            try
            {
                var startDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));
                var endDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));

                var BrancheList = await _context.Branches.Where(x => x.BranchParticipations
                .Where(bp => bp.StartDate <= startDate && bp.EndDate >= endDate && bp.Status == true)
                .Count() > 0).Include(b => b.BranchParticipations).ToListAsync();

                List<BranchDTO> FilteredList = new List<BranchDTO>();
                foreach (var item in BrancheList)
                {

                    if (language == "ar")
                    {
                        FilteredList.Add(new BranchDTO { Id = item.ID, BranchName = item.BranchNameAr, Logo = item.Logo, DeliveryTime = item.DeliveryTime, Opened = item.Opened });
                    }
                    else
                    {
                        FilteredList.Add(new BranchDTO { Id = item.ID, BranchName = item.BranchNameEn, Logo = item.Logo, DeliveryTime = item.DeliveryTime, Opened = item.Opened });
                    }

                }
                return Ok(new JsonResult<BranchDTO>() { Message = "Item Created Successfully", Result = FilteredList });
            }
            catch (Exception ex)
            { return BadRequest(ex.Message); }

        }



        // 2 + Categories ------------------------------------------------------------
        //http://localhost:60785/GetOffers?BranchID=1&language=ar
        [HttpGet]
        [Route("GetOffers")]
        public async Task<IHttpActionResult> GetOffers(long BranchID, string language = "en")
        {
            try
            {
                var startDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));
                var endDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));

                var offersList = await _context.Offers.Where(x => x.StartDate <= startDate && x.EndDate >= endDate && x.Status == true && x.BranchID == BranchID).ToListAsync();

                List<OfferDTO> OfferDTOList = new List<OfferDTO>();
                foreach (var item in offersList)
                {
                    if (language == "ar")
                    {
                        OfferDTOList.Add(new OfferDTO { Id = item.ID, OfferName = item.OfferNameAr, OfferPrice = item.OfferPrice, OrPrice = item.OrPrice, Pic = item.Pic, BranchID = BranchID });
                    }
                    else
                    {
                        OfferDTOList.Add(new OfferDTO { Id = item.ID, OfferName = item.OfferNameEn, OfferPrice = item.OfferPrice, OrPrice = item.OrPrice, Pic = item.Pic, BranchID = BranchID });
                    }
                }

                return Ok(new JsonResult<OfferDTO>() { Message = "Offers Returned Successfully", Result = OfferDTOList });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //http://localhost:60785/GetCategories?BranchID=1&language=ar
        [HttpGet]
        [Route("GetCategories")]
        public async Task<IHttpActionResult> GetCategories(long BranchID, string Language = "en")
        {
            try
            {
                var categoryIDs = await _context.BranchCategories.Where(x => x.BranchID == BranchID && x.Status == true && x.Category.Status ==true)
                .OrderBy(x => x.OrderID).Select(x => x.CategoryID).ToListAsync();

                List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
                foreach (var id in categoryIDs)
                {
                    var category = await _context.Categories.FirstOrDefaultAsync(x => x.ID == id);
                    var itemlist = _context.BranchItems.Where(x => x.Item.CategoryID == category.ID && x.BranchID == BranchID && x.Status == true).ToList();
                    if (itemlist.Count != 0)
                    {
                        if (category != null && Language == "ar")
                        {
                            categoryDTOs.Add(new CategoryDTO { Id = category.ID, CategoryName = category.CategoryNameAr, Pic = category.Pic, BranchID = BranchID });
                        }
                        else
                        {
                            categoryDTOs.Add(new CategoryDTO { Id = category.ID, CategoryName = category.CategoryNameEn, Pic = category.Pic, BranchID = BranchID });
                        }
                    }
                }

                return Ok(new JsonResult<CategoryDTO>() { Message = "Item Returned Successfully", Result = categoryDTOs });
            }
            catch (Exception ex)
            { return BadRequest(ex.Message); }
        }



        // 3+ Items under categoies ------------------------------------------------------------
        //http://localhost:60785/GetOfferItems?CategoryID=1&BranchID=1&language=ar
        [HttpGet]
        [Route("GetBranchItems")]
        public async Task<IHttpActionResult> GetBranchItems(string Language, long CategoryID, long BranchID)
        {
            try
            {
                var items = await _context.BranchItems.Where(x => x.BranchID == BranchID && x.Item.CategoryID == CategoryID && x.Status == true && x.Item.Status == true)
                .Select(x => new { x.ItemID, x.Item.ItemNameAr, x.Item.ItemNameEn, x.Item.PicURL, x.Notes, x.Item.DetailsEn, x.Item.Divisible, x.Price, x.OrderID }).OrderBy(x => x.OrderID).ToListAsync();

                if (items == null)
                {
                    return Ok(new JsonResult<BranchItemDTO>() { Message = "There's No Items With This BranchID", Result = null });
                }

                List<BranchItemDTO> BranchItemDTOs = new List<BranchItemDTO>();
                foreach (var item in items)
                {
                    BranchItemDTOs.Add(new BranchItemDTO { Id = item.ItemID, ItemName = Language == "ar" ? item.ItemNameAr : item.ItemNameEn, PicUrl = item.PicURL, Details = Language == "ar" ? item.Notes : item.Notes, Divisible = item.Divisible, Price = item.Price });
                }

                return Ok(new JsonResult<BranchItemDTO>() { Message = "Items Returned Successfully", Result = BranchItemDTOs });
            }
            catch (Exception ex)
            { return BadRequest(ex.Message); }

        }

        //http://localhost:60785/AddItemToCarpet?CustomerID=1&ID=1&Price=20&Quantity=2
        [HttpGet]
        [Route("AddItemToCarpet")]
        public async Task<IHttpActionResult> AddItemToCarpet(long CustomerID, long ID, decimal Price, decimal Quantity, Int32 BranchID) //ID = itemID, Check for BranchID == 0 when the item will updated not added.
        {
            try
            {
                var invoiceID = await _context.Invoices.Where(x => x.CustomerID == CustomerID && x.Carpet == true).Select(x => x.ID).FirstOrDefaultAsync();
                if (invoiceID == 0) //long default   //No Carpet
                {
                    var newInvoice = new Invoice { CustomerID = CustomerID, 
                        RequestDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")), 
                        RequestTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")).TimeOfDay,
                        InvoiceCode = System.IO.Path.GetRandomFileName(),
                        Carpet = true };
                    _context.Invoices.Add(newInvoice);
                    await _context.SaveChangesAsync();
                    _context.InvoiceItems.Add(new InvoiceItem { ItemID = ID, Price = Price, Quantity = Quantity, InvoiceID = newInvoice.ID, BranchID = BranchID });  //Discount, Notes, OfferID  Should I Put Them ?
                }
                else //There's carpet, so add item
                {
                    var Item = _context.InvoiceItems.FirstOrDefault(x => x.InvoiceID == invoiceID && x.ItemID == ID);
                    if (Item == null)
                    {
                        _context.InvoiceItems.Add(new InvoiceItem { ItemID = ID, Price = Price, Quantity = Quantity, InvoiceID = invoiceID, BranchID = BranchID });  //Discount, Notes, OfferID  Should I Put Them ?
                    }
                    else
                    {
                        Item.Price = Price;
                        Item.Quantity += Quantity;
                        _context.Entry(Item).State = EntityState.Modified;
                    }
                }
                await _context.SaveChangesAsync();
                return Ok("Item Added Successfully");
            }
            catch (Exception ex)
            { return BadRequest(ex.Message); }

        }



        // 4+ Offer’s Page ------------------------------------------------------------
        //http://localhost:60785/GetOfferItems?OfferID=1&language=ar
        [HttpGet]
        [Route("GetOfferItems")]
        public async Task<IHttpActionResult> GetOfferItems(string Language, long OfferID)
        {
            try
            {

                var items = await _context.OfferItems.Where(x => x.OfferID == OfferID && x.Status == true)
               .Include(x => x.Item)
               .Select(x => new { x.ItemID, x.Quantity, x.Item.BranchItems.FirstOrDefault(b => b.ItemID == x.ItemID).OrderID, x.Item.PicURL, x.Item.ItemNameAr, x.Item.ItemNameEn })
               .OrderBy(x => x.OrderID).ToListAsync();

                List<OfferItemDTO> OfferItemDTOs = new List<OfferItemDTO>();
                var name = string.Empty;
                foreach (var item in items)
                {
                    OfferItemDTOs.Add(new OfferItemDTO { Id = item.ItemID, ItemName = Language == "ar" ? item.ItemNameAr : item.ItemNameEn, PicUrl = item.PicURL, Quantity = item.Quantity });
                }

                return Ok(new JsonResult<OfferItemDTO>() { Message = "Offer Items Returned Successfully", Result = OfferItemDTOs });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //http://localhost:60785/AddOfferToCarpet?CustomerID=1&ID=1&Price=20&Quantity=2
        [HttpGet]
        [Route("AddOfferToCarpet")]
        public async Task<IHttpActionResult> AddOfferToCarpet(long CustomerID, long ID, decimal Price, decimal Quantity, Int32 BranchID) //ID = offerID, Check for BranchID == 0 when the item will updated not added.
        {
            try
            {
                var invoiceID = await _context.Invoices.Where(x => x.CustomerID == CustomerID && x.Carpet == true).Select(x => x.ID).FirstOrDefaultAsync();
                if (invoiceID == 0) //long default   //No Carpet
                {
                    var newInvoice = new Invoice { CustomerID = CustomerID, 
                        RequestDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")),
                        RequestTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")).TimeOfDay,
                        InvoiceCode = System.IO.Path.GetRandomFileName(),
                        Carpet = true };
                    _context.Invoices.Add(newInvoice);
                    await _context.SaveChangesAsync();
                    _context.InvoiceItems.Add(new InvoiceItem { OfferID = ID, Price = Price, Quantity = Quantity, InvoiceID = newInvoice.ID, BranchID = BranchID });

                    await _context.SaveChangesAsync();
                }
                else //There's carpet, so add item
                {
                    var Item = _context.InvoiceItems.FirstOrDefault(x => x.InvoiceID == invoiceID && x.OfferID == ID);
                    if (Item == null)
                    {
                        _context.InvoiceItems.Add(new InvoiceItem { OfferID = ID, Price = Price, Quantity = Quantity, InvoiceID = invoiceID, BranchID = BranchID });
                    }
                    else
                    {
                        Item.Price = Price;
                        Item.Quantity += Quantity;
                        _context.Entry(Item).State = EntityState.Modified;
                    }
                    await _context.SaveChangesAsync();
                }
                return Ok("Offer Added Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        // 5+ Carpet’s Page ------------------------------------------------------------
        //http://localhost:60785/CarpetItems?CustomerID=1&language=ar
        [HttpGet]
        [Route("CarpetItems")]
        public async Task<IHttpActionResult> CarpetItems(string Language, long CustomerID)
        {
            var result = new JsonResult<CarpetItemsDTO>();
            try
            {
                var invoiceID = await _context.Invoices.Where(x => x.CustomerID == CustomerID && x.Carpet == true).Select(x => x.ID).FirstOrDefaultAsync();
                if (invoiceID != 0)
                {
                    var invoiceItems = _context.InvoiceItems.Where(x => x.InvoiceID == invoiceID).Include(x => x.Offer).Include(x => x.Item).ToList();

                    foreach (var item in invoiceItems)
                    {
                        var status = false;
                        var name = String.Empty;
                        var priceUpdated = false;
                        var ended = false;

                        if (item.ItemID != null)
                        {
                            if (item.Item.Status == true) { status = true; }
                            if (Language == "ar") { name = item.Item.ItemNameAr; }
                            else { name = item.Item.ItemNameEn; }
                            var realPrice = _context.BranchItems.Where(x => x.ItemID == item.ItemID).Select(x => x.Price).FirstOrDefault();
                            if (item.Price != realPrice) { priceUpdated = true; }

                            var carpetItemsDTO = new CarpetItemsDTO
                            {
                                ID = item.ItemID,
                                ItemType = "ItemID",
                                Name = name,
                                Quantity = item.Quantity,
                                Available = status,
                                Price = realPrice,
                                PriceUpdated = priceUpdated,
                                Divisible = item.Item.Divisible,
                                PicUrl = item.Item.PicURL,
                                Ended = ended
                            };
                            result.Result.Add(carpetItemsDTO);

                            if (!status) //if stutus = false then delete from invoice
                            {
                                var itemStutusFalse = _context.InvoiceItems.FirstOrDefault(x => x.ID == item.ID);
                                _context.InvoiceItems.Remove(itemStutusFalse);
                                await _context.SaveChangesAsync();
                            }
                        }
                        if (item.OfferID != null)
                        {
                            if (item.Offer.Status == true) { status = true; }
                            if (Language == "ar") { name = item.Offer.OfferNameAr; }
                            else { name = item.Offer.OfferNameEn; }
                            if (item.Price != item.Offer.OfferPrice) { priceUpdated = true; }
                            if (!(item.Offer.StartDate <= TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")) &&
                                item.Offer.EndDate >= TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")))) 
                            { ended = true; }

                            result.Result.Add(new CarpetItemsDTO
                            {
                                ID = item.OfferID,
                                ItemType = "OfferID",
                                Name = name,
                                Quantity = item.Quantity,
                                Available = status,
                                Price = item.Offer.OfferPrice,
                                PriceUpdated = priceUpdated,
                                PicUrl = item.Offer.Pic,
                                Divisible = false,
                                Ended = ended
                            });//there is no Divisible so it's = false always 

                            if (!status) //if stutus = false then delete from invoice
                            {
                                var itemStutusFalse = _context.InvoiceItems.FirstOrDefault(x => x.ID == item.ID);
                                _context.InvoiceItems.Remove(itemStutusFalse);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    result.Message = "Items Returned Successfully";
                    return Ok(result);
                }
                else
                {
                    return BadRequest("This Carpet not exist or Confirmed");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        
        [HttpGet]
        [Route("CarpetConfirmed")]
        public async Task<IHttpActionResult> CarpetConfirmed(long CustomerID, decimal totalPrice, string notes)
        {
            try
            {
                //Random newR = new Random();
                //Int64 RandomeCode = newR.Next(100000, 9999999);
                var invoice = await _context.Invoices.FirstOrDefaultAsync(x => x.CustomerID == CustomerID && x.Carpet == true);
                if (invoice != null)
                {
                    //invoice.Notes = notes;
                    invoice.Carpet = false;
                    invoice.RequestTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")).TimeOfDay;
                    invoice.RequestDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));
                    invoice.TotalPrice = totalPrice;
                    _context.Entry(invoice).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    //Call GetExtraCostsList and set in invoiceextracost
                    var extraCostsList = GetExtraCostsList("en", invoice.ID);
                    if (extraCostsList != null)
                    {
                        foreach (var item in extraCostsList)
                        {
                            var newInvoiceExtraCost = new InvoiceExtraCost
                            {
                                InvoiceID = invoice.ID,
                                ExtraCostID = item.ID,
                                ExtraCostValue = item.ExtraValue
                            };
                            _context.InvoiceExtraCosts.Add(newInvoiceExtraCost);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else { return BadRequest("Carpet alrady Confirmed"); }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }

            return Ok(new { Message = "Carpet Confirmed Successfully" });
        }

        
        [HttpGet]
        [Route("DeleteItemFromCarpet")]
        public async Task<IHttpActionResult> DeleteItemFromCarpet(long ItemId, long CustomerID)//ItemId is ItemId or OfferId
        {
            var invoiceID = await _context.Invoices.Where(x => x.CustomerID == CustomerID && x.Carpet == true).Select(x => x.ID).FirstOrDefaultAsync();
            if (invoiceID != 0)
            {
                var invoiceItem = _context.InvoiceItems.Where(x => x.ItemID == ItemId || x.OfferID == ItemId).FirstOrDefault();
                if (invoiceItem == null)
                { return NotFound(); }

                _context.InvoiceItems.Remove(invoiceItem);
                await _context.SaveChangesAsync();
                return Ok("Item Deleted Successfully.");
            }
            else
            { return BadRequest("No Carpet For This User."); }
        }

        [HttpGet]
        [Route("GetExtraCost")]
        public async Task<IHttpActionResult> GetExtraCost(string Language, long CustomerID)
        {
            try
            {
                var invoiceID = await _context.Invoices.Where(x => x.CustomerID == CustomerID && x.Carpet == true).Select(x => x.ID).FirstOrDefaultAsync();
                if (invoiceID != 0)
                {
                    var result = GetExtraCostsList(Language, invoiceID);
                    if (result != null)
                    {
                        return Ok(new JsonResult<ExtraCostDTO> { Message = "Items Returned Successfully", Result = result });
                    }
                    else { return BadRequest("No BranchIDs For This User."); }
                }
                else
                { return BadRequest("No Carpet For This User."); }
            }
            catch (Exception ex)
            { return BadRequest(ex.Message); }

        }

        [HttpGet]
        [Route("SetFeedback")]
        public async Task<IHttpActionResult> SetFeedback(int BranchID, double EvalDegree, int CustomerID, string Comment)
        {
            try
            {
                var BranchId = _context.Branches.Where(x => x.ID == BranchID).Select(x => x.ID).FirstOrDefault();
                if (BranchId == 0) { return BadRequest("This Branch Is Not Exist."); }
                var CustomerId = _context.Customers.Where(x => x.ID == CustomerID).Select(x => x.ID).FirstOrDefault();
                if (CustomerId == 0) { return BadRequest("This Client Is Not Exist."); }

                var newCustomerFeedback = new CustomerFeedback
                {
                    TableName = "Branch",
                    TableID = BranchID,
                    Approved = true,
                    Comment = Comment,
                    EvalDegree = EvalDegree,
                    CommentDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")),
                    CustomerID = CustomerID,
                    Status = true
                };
                _context.CustomerFeedbacks.Add(newCustomerFeedback);

                await _context.SaveChangesAsync();
                return Ok("Evaluation Added Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetFeedback")]
        public  IHttpActionResult GetFeedback(int BranchID)
        {
            var BranchId = _context.Branches.Where(x => x.ID == BranchID).Select(x => x.ID).FirstOrDefault();
            if (BranchId == 0) { return BadRequest("This Branch Is Not Exist."); }
            var feedbackList = _context.CustomerFeedbacks.Where(x => x.TableID == BranchID).Select(x => new { x.Customer.CustomerName, x.EvalDegree, x.Comment, CommentDate = x.CommentDate.ToString() }).OrderByDescending(x=>x.CommentDate).ToList();
            if(feedbackList.Count > 0)
            {
                return Ok(feedbackList);
            }
            else { return Ok("There's No Feedback For This Branch."); }
        }




        private List<ExtraCostDTO> GetExtraCostsList(string Language, long invoiceID)
        {

            var BranchIDs = _context.InvoiceItems.Where(x => x.InvoiceID == invoiceID).Select(x => x.BranchID).Distinct().ToList();
            if (BranchIDs != null)
            {
                var extraCostsDTOList = new List<ExtraCostDTO>();
                foreach (var branchID in BranchIDs)
                {
                    var extraCostList = _context.ExtraCosts.Where(x => x.BranchID == branchID && x.Status == true).ToList();
                    if (extraCostList != null)
                    {
                        foreach (var extraCost in extraCostList)
                        {
                            extraCostsDTOList.Add(new ExtraCostDTO
                            {
                                BranchName = Language == "ar" ? extraCost.Branch.BranchNameAr : extraCost.Branch.BranchNameEn,
                                ExtraTitle = Language == "ar" ? extraCost.ExtraTitleAr : extraCost.ExtraTitleEn,
                                ExtraValue = extraCost.ExtraValue,
                                ID = extraCost.ID
                            });
                        }

                    }
                }
                return extraCostsDTOList;
            }
            else { return null; }
        }
        
    }
}
