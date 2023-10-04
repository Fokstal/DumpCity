using DumpCity.Data;
using Microsoft.AspNetCore.Mvc;
using DumpCity.Utility;
using DumpCity.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DumpCity.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace DumpCity.Controllers
{
	[Authorize(Roles = WC.adminRole)]
    public class CartController : Controller
    {

        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHost;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;

        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; } = new();

        public CartController(AppDbContext db, IWebHostEnvironment webHost, IEmailSender emailSender, IConfiguration config)
        {
            _db = db;
            _webHost = webHost;
            _emailSender = emailSender;
            _config = config;
        }

        public IActionResult Index()
        {
            List<ShoppingCart>? productsFromSession = HttpContext.Session.Get<List<ShoppingCart>>(WC.sessionCart);
            List<Product> products = new();

            if (productsFromSession is not null && productsFromSession.Count > 0)
            {
                List<int> productsIdFromSession = productsFromSession.Select(el => el.ProductId).ToList();

                if (_db.Product is not null)
                {
                    products = _db.Product.Where(el => productsIdFromSession.Contains(el.ID)).ToList();
                }
            }

            return View(products);
        }

        [Authorize]
        [ActionName("Index")]
        [HttpPost]
        public IActionResult IndexPost()
        {
            if (ModelState.IsValid) return RedirectToAction(nameof(Summary));

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<ShoppingCart>? productsFromSession = HttpContext.Session.Get<List<ShoppingCart>>(WC.sessionCart);
            List<Product> products = new();

            if (productsFromSession is not null && productsFromSession.Count > 0)
            {
                List<int> productsIdFromSession = productsFromSession.Select(el => el.ProductId).ToList();

                if (_db.Product is not null)
                {
                    List<Product> productsFromDb = _db.Product.Include(el => el.Category).Include(el => el.AppType).ToList();

                    products = productsFromDb.Where(el => productsIdFromSession.Contains(el.ID)).ToList();
                }
            }

            ProductUserVM = new();

            if (_db.AppUser is not null && claim is not null)
            {
                ProductUserVM.AppUser = _db.AppUser.FirstOrDefault(el => el.Id == claim.Value);
                ProductUserVM.ProductList = products;
            }

            return View(ProductUserVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPost()
        {
            string pathToTemplate = _webHost.WebRootPath + Path.AltDirectorySeparatorChar.ToString() + "templates" + Path.AltDirectorySeparatorChar.ToString() + "Inquiry.html";

            string subject = "New Inquiry";
            string htmlBody = "";

            using (StreamReader sr = System.IO.File.OpenText(pathToTemplate))
            {
                htmlBody = sr.ReadToEnd();
            }

            StringBuilder productListSB = new();

            if (ProductUserVM.ProductList is not null)
            {
                foreach (var prod in ProductUserVM.ProductList)
                {
                    productListSB.Append($" - Name: {prod.Name} <span style='font-size:14px;'>{prod.ID}</span> </br>");
                }
            }

            string messageBody = string.Format(
                htmlBody, 
                ProductUserVM.AppUser!.FullName, 
                ProductUserVM.AppUser.Email, 
                ProductUserVM.AppUser.PhoneNumber, 
                productListSB.ToString());

            _emailSender.SendEmailAsync(_config.GetSection("EmailWorker").GetSection("EmailAdmin").Value, subject, messageBody);

            return RedirectToAction(nameof(InquiryConfirm));
        }

        public IActionResult InquiryConfirm()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult Remove(int? id)
        {
            if (id is null || id == 0) return NotFound();

            List<ShoppingCart>? productsFromSession = HttpContext.Session.Get<List<ShoppingCart>>(WC.sessionCart);

            if (productsFromSession is not null && productsFromSession.Count() != 0)
            {
                ShoppingCart? productToDelete = productsFromSession.SingleOrDefault(el => el.ProductId == id);

                if (productToDelete is not null)
                {
                    productsFromSession.Remove(productToDelete);

                    HttpContext.Session.Set<List<ShoppingCart>>(WC.sessionCart, productsFromSession);
                }
            }

            return RedirectToAction(nameof(Index));
        }

    }
}