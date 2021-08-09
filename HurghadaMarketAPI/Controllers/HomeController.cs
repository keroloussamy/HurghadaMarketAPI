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
                    newCustomer = new Customer { Status = true, CustomerName = customer.CustomerName, Phone = customer.Phone, CustomerAddresses = new List<CustomerAddress> { new CustomerAddress { Address = customer.Address } } };
                    _context.Customers.Add(newCustomer);
                }
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Item Created Successfully", Id = newCustomer.ID });
            }
            catch (Exception ex)
            {

                return Ok(new { Message = ex.Message });
            }

        }

        // 1+ Branches’ Page ------------------------------------------------------------
        //http://localhost:60785/GetBranches?language=ar
        //http://localhost:60785/GetBranches
        [Route("GetBranches")]
        [HttpGet]
        public async Task<IHttpActionResult> GetBranches(string language = "en")
        {
            var BrancheList = await _context.Branches.Include(b => b.BranchParticipations).ToListAsync();
            List<BranchDTO> FilteredList = new List<BranchDTO>();
            foreach (var item in BrancheList)
            {
                if (item.BranchParticipations.Any(x => x.StartDate <= DateTime.Now.Date && x.EndDate >= DateTime.Now.Date))
                {
                    if (language == "ar")
                    {
                        FilteredList.Add(new BranchDTO { Id = item.ID, BranchName = item.BranchNameAr, Logo = item.Logo });
                    }
                    else
                    {
                        FilteredList.Add(new BranchDTO { Id = item.ID, BranchName = item.BranchNameEn, Logo = item.Logo });
                    }

                }
            }

            return Ok(new JsonResult<BranchDTO>() { Message = "Item Created Successfully", Result = FilteredList });
        }



        // 2 + Categories ------------------------------------------------------------
        //http://localhost:60785/GetOffers?BranchID=1&language=ar
        [HttpGet]
        [Route("GetOffers")]
        public async Task<IHttpActionResult> GetOffers(long BranchID, string language = "en")
        {

            var offersList = await _context.Offers.Where(
                x => x.StartDate <= DbFunctions.TruncateTime(DateTime.Now)
                && x.EndDate >= DbFunctions.TruncateTime(DateTime.Now)
                && x.Status == true
                && x.BranchID == BranchID).ToListAsync();

            List<OfferDTO> OfferDTOList = new List<OfferDTO>();
            foreach (var item in offersList)
            {
                if (language == "ar")
                {
                    OfferDTOList.Add(new OfferDTO { Id = item.ID, OfferName = item.OfferNameAr, OfferPrice = item.OfferPrice, OrPrice = item.OrPrice });
                }
                else
                {
                    OfferDTOList.Add(new OfferDTO { Id = item.ID, OfferName = item.OfferNameEn, OfferPrice = item.OfferPrice, OrPrice = item.OrPrice });
                }
            }

            return Ok(new JsonResult<OfferDTO>() { Message = "Offers Returned Successfully", Result = OfferDTOList });
        }

        //http://localhost:60785/GetCategories?BranchID=1&language=ar
        [HttpGet]
        [Route("GetCategories")]
        public async Task<IHttpActionResult> GetCategories(long BranchID, string Language = "en")
        {

            var categoryIDs = await _context.BranchCategories.Where(x => x.BranchID == BranchID).OrderBy(x => x.OrderID).Select(x => x.CategoryID).ToListAsync();


            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            foreach (var id in categoryIDs)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.ID == id);
                if (category != null && Language == "ar")
                {
                    categoryDTOs.Add(new CategoryDTO { Id = category.ID, CategoryName = category.CategoryNameAr, Pic = category.Pic, BranchID = BranchID });
                }
                else
                {
                    categoryDTOs.Add(new CategoryDTO { Id = category.ID, CategoryName = category.CategoryNameEn, Pic = category.Pic, BranchID = BranchID });
                }
            }

            return Ok(new JsonResult<CategoryDTO>() { Message = "Item Returned Successfully", Result = categoryDTOs });
        }



        // 3+ Items under categoies ------------------------------------------------------------
        //http://localhost:60785/GetOfferItems?CategoryID=1&BranchID=1&language=ar
        [HttpGet]
        [Route("GetBranchItems")]
        public async Task<IHttpActionResult> GetBranchItems(string Language, long CategoryID, long BranchID)
        {
            try
            {
                var items = await _context.BranchItems.Where(x => x.BranchID == BranchID && x.Item.CategoryID == CategoryID).Include(x => x.Item)
                .Select(x => new { x.ItemID, x.Item.ItemNameAr, x.Item.ItemNameEn, x.Item.PicURL, x.Item.DetailsAr, x.Item.DetailsEn, x.Item.Divisible, x.Price, x.OrderID }).OrderBy(x => x.OrderID).ToListAsync();

                if (items == null)
                {
                    return Ok(new JsonResult<BranchItemDTO>() { Message = "There's No Items With This BranchID", Result = null });
                }

                List<BranchItemDTO> BranchItemDTOs = new List<BranchItemDTO>();
                foreach (var item in items)
                {
                    BranchItemDTOs.Add(new BranchItemDTO { Id = item.ItemID, ItemName = Language == "ar" ? item.ItemNameAr : item.ItemNameEn, PicUrl = item.PicURL, Details = Language == "ar" ? item.DetailsAr : item.DetailsEn, Divisible = item.Divisible, Price = item.Price });
                }

                return Ok(new JsonResult<BranchItemDTO>() { Message = "Items Returned Successfully", Result = BranchItemDTOs });
            }
            catch (Exception ex)
            {
                return Ok(new JsonResult<BranchItemDTO>() { Message = ex.Message, Result = null });
            }

        }

        //http://localhost:60785/AddItemToCarpet?CustomerID=1&ID=1&Price=20&Quantity=2
        [HttpGet]
        [Route("AddItemToCarpet")]
        public async Task<IHttpActionResult> AddItemToCarpet(long CustomerID, long ID, decimal Price, decimal Quantity) //ID = itemID
        {
            try
            {
                var invoiceID = await _context.Invoices.Where(x => x.CustomerID == CustomerID && x.Carpet == true).Select(x => x.ID).FirstOrDefaultAsync();
                if (invoiceID == 0) //long default   //No Carpet
                {
                    var newInvoice = new Invoice { CustomerID = CustomerID, RequestDate = DateTime.Now.Date, RequestTime = DateTime.Now.TimeOfDay, Carpet = true };
                    _context.Invoices.Add(newInvoice); //The rest of the values ?
                    await _context.SaveChangesAsync();
                    _context.InvoiceItems.Add(new InvoiceItem { ItemID = ID, Price = Price, Quantity = Quantity, InvoiceID = newInvoice.ID });  //Discount, Notes, OfferID  Should I Put Them ?

                }
                else //There's carpet, so add item
                {
                    var Item = _context.InvoiceItems.FirstOrDefault(x => x.InvoiceID == invoiceID && x.ItemID == ID);
                    if (Item == null)
                    {
                        _context.InvoiceItems.Add(new InvoiceItem { ItemID = ID, Price = Price, Quantity = Quantity, InvoiceID = invoiceID });  //Discount, Notes, OfferID  Should I Put Them ?
                    }
                    else
                    {
                        Item.Price = Price;
                        Item.Quantity += Quantity;
                        _context.Entry(Item).State = EntityState.Modified;
                    }

                }
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Item Added Successfully"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Message = ex.Message
                });
            }

        }



        // 4+ Offer’s Page ------------------------------------------------------------
        //http://localhost:60785/GetOfferItems?OfferID=1&language=ar
        [HttpGet]
        [Route("GetOfferItems")]
        public async Task<IHttpActionResult> GetOfferItems(string Language, long OfferID)
        {
            try
            {
                //var items = await _context.OfferItems.Where(x => x.OfferID == OfferID)
                //.Include(x => x.Item)
                //.ThenInclude(x => x.BranchItems)
                //.Select(x => new { x.ItemID, x.Quantity, x.Item.BranchItems.FirstOrDefault(b => b.ItemId == x.ItemId).OrderId, x.Item.PicUrl, x.Item.ItemNameAr, x.Item.ItemNameEn })
                //.OrderBy(x => x.OrderId).ToListAsync();

                var items = await _context.OfferItems.Where(x => x.OfferID == OfferID)
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

                return Ok(new JsonResult<OfferItemDTO>() { Message = ex.Message, Result = null });
            }

        }

        //http://localhost:60785/AddOfferToCarpet?CustomerID=1&ID=1&Price=20&Quantity=2
        [HttpGet]
        [Route("AddOfferToCarpet")]
        public async Task<IHttpActionResult> AddOfferToCarpet(long CustomerID, long ID, decimal Price, decimal Quantity) //ID = offerID
        {
            try
            {
                var invoiceID = await _context.Invoices.Where(x => x.CustomerID == CustomerID && x.Carpet == true).Select(x => x.ID).FirstOrDefaultAsync();
                if (invoiceID == 0) //long default   //No Carpet
                {
                    var newInvoice = new Invoice { CustomerID = CustomerID, RequestDate = DateTime.Now.Date, RequestTime = DateTime.Now.TimeOfDay, Carpet = true };
                    _context.Invoices.Add(newInvoice);
                    await _context.SaveChangesAsync();
                    _context.InvoiceItems.Add(new InvoiceItem { OfferID = ID, Price = Price, Quantity = Quantity, InvoiceID = newInvoice.ID });

                    await _context.SaveChangesAsync();
                }
                else //There's carpet, so add item
                {
                    var Item = _context.InvoiceItems.FirstOrDefault(x => x.InvoiceID == invoiceID && x.OfferID == ID);
                    if (Item == null)
                    {
                        _context.InvoiceItems.Add(new InvoiceItem { OfferID = ID, Price = Price, Quantity = Quantity, InvoiceID = invoiceID });
                    }
                    else
                    {
                        Item.Price = Price;
                        Item.Quantity += Quantity;
                        _context.Entry(Item).State = EntityState.Modified;
                    }
                    await _context.SaveChangesAsync();
                }
                return Ok(new { Message = "Offer Added Successfully" });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message });
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
                            if (!(item.Offer.StartDate <= DateTime.Now.Date && item.Offer.EndDate >= DateTime.Now.Date)) { ended = true; }

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
                    result.Message = "This Carpet not exist or Confirmed";
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return Ok(result);
            }

        }

        [HttpGet]
        [Route("CarpetConfirmed")]
        public async Task<IHttpActionResult> CarpetConfirmed(long CustomerID, bool status)
        {
            try
            {
                var invoice = await _context.Invoices.FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
                invoice.Carpet = status;
                _context.Entry(invoice).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { return Ok(new { Message = ex.Message }); }

            return Ok(new { Message = "Carpet Confirmed Successfully" });
        }
    }
}
