using DumpCity.Data;
using DumpCity.Models;
using DumpCity.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using DumpCity.Utility;

namespace DumpCity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db, ILogger<HomeController> logger)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Load()
        {
            return View();
        }

        public IActionResult Index()
        {
            List<DetailsVM> products = new();
            List<Product> productsFromDb = new();
            List<ShoppingCart>? productsFromSession = HttpContext.Session.Get<List<ShoppingCart>>(WC.sessionCart);

            if (_db.Product is not null)
            {
                productsFromDb = _db.Product.Include(el => el.Category).Include(el => el.AppType).ToList();
            }

            if (productsFromSession is not null)
            {
                int countProdSession = productsFromSession.Count();
                int j = 0;

                for (int i = 0; i < productsFromDb.Count(); i++)
                {

                    DetailsVM detailsVM = new()
                    {
                        Product = productsFromDb[i],
                        IsExistsProductInCart = false
                    };

                    if (j < countProdSession)
                    {
                        if (productsFromDb[i].ID == productsFromSession[j].ProductId)
                        {
                            detailsVM.IsExistsProductInCart = true;
                            j++;
                        }
                    }

                    products.Add(detailsVM);
                }
            }

            if (productsFromSession is null)
            {
                products = productsFromDb.Select(el => new DetailsVM() { Product = el, IsExistsProductInCart = false }).ToList();
            }

            HomeVM homeVM = new()
            {
                Products = products,
                Categories = new List<Category>(),
            };

            if (_db.Category is not null)
            {
                homeVM.Categories = _db.Category;
            }

            return View(homeVM);
        }

        public IActionResult Details(int? id)
        {
            if (id is null && id == 0) return NotFound();

            List<ShoppingCart> cart = new();
            List<ShoppingCart>? cartFromSession = HttpContext.Session.Get<List<ShoppingCart>>(WC.sessionCart);

            if (cartFromSession is not null && cartFromSession.Count() > 0)
            {
                cart = cartFromSession;
            }

            DetailsVM detailsVM = new()
            {
                Product = new(),
                IsExistsProductInCart = false
            };

            if (_db.Product is not null)
            {
                detailsVM.Product = _db.Product.Include(el => el.Category).Include(el => el.AppType).FirstOrDefault(el => el.ID == id) ?? new();
            }

            if (cart.Select(el => el.ProductId).ToList().Contains(Convert.ToInt32(id)))
            {
                detailsVM.IsExistsProductInCart = true;
            }

            return View(detailsVM);
        }

        [ActionName("Details")]
        [HttpPost]
        public IActionResult DetailsPost(int? id)
        {
            if (id is null || id == 0) return NotFound();

            List<ShoppingCart> cart = new();
            List<ShoppingCart>? cartFromSession = HttpContext.Session.Get<List<ShoppingCart>>(WC.sessionCart);

            if (cartFromSession is not null && cartFromSession.Count() > 0)
            {
                cart = cartFromSession;
            }

            cart.Add(new() { ProductId = Convert.ToInt32(id) });

            HttpContext.Session.Set<List<ShoppingCart>>(WC.sessionCart, cart);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddToCart(int? id)
        {
            if (id is null || id == 0) return NotFound();

            List<ShoppingCart> cart = new();
            List<ShoppingCart>? cartFromSession = HttpContext.Session.Get<List<ShoppingCart>>(WC.sessionCart);

            if (cartFromSession is not null && cartFromSession.Count() > 0)
            {
                cart = cartFromSession;
            }

            cart.Add(new() { ProductId = Convert.ToInt32(id) });

            HttpContext.Session.Set<List<ShoppingCart>>(WC.sessionCart, cart);

            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoveFromCart(int? id)
        {
            if (id is null || id == 0) return NotFound();

            List<ShoppingCart> cart = new();
            List<ShoppingCart>? cartFromSession = HttpContext.Session.Get<List<ShoppingCart>>(WC.sessionCart);

            if (cartFromSession is not null && cartFromSession.Count() > 0)
            {
                cart = cartFromSession;
            }

            var productToDel = cart.SingleOrDefault(el => el.ProductId == id);

            if (productToDel is not null)
            {
                cart.Remove(productToDel);
            }

            HttpContext.Session.Set<List<ShoppingCart>>(WC.sessionCart, cart);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}