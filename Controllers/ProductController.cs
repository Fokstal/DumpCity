using DumpCity.Data;
using DumpCity.Models;
using DumpCity.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DumpCity.Controllers
{
	public class ProductController : Controller
	{
		private readonly AppDbContext _db;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProductController(AppDbContext db, IWebHostEnvironment webHostEnvironment)
		{
			_db = db;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			IEnumerable<Product> productList = new List<Product>();

			if (_db.Product is not null)
			{
				productList = _db.Product.Include(el => el.Category).Include(el => el.AppType);
			}

			return View(productList);
		}

		[HttpGet]
		public IActionResult Upsert(int? id)
		{
			ProductVM productVM = new();

			if (_db.Category is not null)
			{
				productVM.CategorySelectList = _db.Category.Select(el => new SelectListItem
				{
					Text = el.Name,
					Value = el.ID.ToString()
				});
			}

			if (_db.AppType is not null)
			{
				productVM.AppTypeSelectList = _db.AppType.Select(el => new SelectListItem
				{
					Text = el.Name,
					Value = el.ID.ToString()
				});
			}

			if (id is not null)
			{
				if (_db.Product is not null)
				{
					productVM.Product = _db.Product.FirstOrDefault(el => el.ID == id) ?? new();

					if (productVM.Product is null) return NotFound();
				}
			}

			return View(productVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(ProductVM productVM)
		{
			if (ModelState.IsValid)
			{
				var files = HttpContext.Request.Form.Files;
				string webRootPath = _webHostEnvironment.WebRootPath;

				if (productVM.Product.ID == 0)
				{
					string upload = webRootPath + WC.imageProductFolderPath;
					string fileName = Guid.NewGuid().ToString();
					string extension = Path.GetExtension(files[0].FileName);

					using (FileStream fileStream = new(Path.Combine(upload, fileName + extension), FileMode.Create))
					{
						files[0].CopyTo(fileStream);
					}

					productVM.Product.ImageURL = fileName + extension;

					_db.Product?.Add(productVM.Product);
				}

				if (productVM.Product.ID != 0)
				{
					Product productFromDb = _db.Product?.AsNoTracking().FirstOrDefault(el => el.ID == productVM.Product.ID) ?? new();

					if (files.Count > 0)
					{
						string upload = webRootPath + WC.imageProductFolderPath;
						string fileName = Guid.NewGuid().ToString();
						string extension = Path.GetExtension(files[0].FileName);

						var oldFile = Path.Combine(upload, productFromDb.ImageURL ?? "");

						if (System.IO.File.Exists(oldFile))
						{
							System.IO.File.Delete(oldFile);
						}

						using (FileStream fileStream = new(Path.Combine(upload, fileName + extension), FileMode.Create))
						{
							files[0].CopyTo(fileStream);
						}

						productVM.Product.ImageURL = fileName + extension;
					}

					if (files.Count == 0)
					{
						productVM.Product.ImageURL = productFromDb.ImageURL;
					}

					_db.Product?.Update(productVM.Product);
				}

				_db.SaveChanges();

				return RedirectToAction("Index");
			}

			if (_db.Category is not null)
			{
				productVM.CategorySelectList = _db.Category.Select(el => new SelectListItem
				{
					Text = el.Name,
					Value = el.ID.ToString()
				});
			}

			if (_db.AppType is not null)
			{
				productVM.AppTypeSelectList = _db.AppType.Select(el => new SelectListItem
				{
					Text = el.Name,
					Value = el.ID.ToString()
				});
			}

			return View(productVM);
		}

		public IActionResult Delete(int? id)
		{
			if (id is null || id == 0) return NotFound();

			Product? product = _db.Product?.FirstOrDefault(el => el.ID == id);

			if (product is null) return NotFound();

			string webRootPath = _webHostEnvironment.WebRootPath;
			string upload = webRootPath + WC.imageProductFolderPath;

			var oldFile = Path.Combine(upload, product.ImageURL ?? "");

			if (System.IO.File.Exists(oldFile))
			{
				System.IO.File.Delete(oldFile);
			}

			_db.Product?.Remove(product);
			_db.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
