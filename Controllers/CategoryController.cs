using DumpCity.Data;
using DumpCity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DumpCity.Controllers
{
	[Authorize(Roles = WC.adminRole)]
	public class CategoryController : Controller
	{
		private readonly AppDbContext _db;

		public CategoryController(AppDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			IEnumerable<Category> categoryList = new List<Category>();

			if (_db.Category is not null)
			{
				categoryList = _db.Category;
			}

			return View(categoryList);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Category category)
		{
			if (ModelState.IsValid)
			{
				_db.Category?.Add(category);
				_db.SaveChanges();

				return RedirectToAction("Index");
			}

			return View(category);
		}

		[HttpGet]
		public IActionResult Edit(int? id)
		{
			if (id is null || id == 0) return NotFound();

			Category? category = new();

			category = _db.Category?.FirstOrDefault(el => el.ID == id);

			if (category is null) return NotFound();

			return View(category);
		}

		[HttpPost]
		public IActionResult Edit(Category category)
		{
			if (ModelState.IsValid)
			{
				_db.Category?.Update(category);
				_db.SaveChanges();

				return RedirectToAction("Index");
			}

			return View(category);
		}

		public IActionResult Delete(int? id)
		{
			if (id is null || id == 0) return NotFound();

			Category? category = new();

			category = _db.Category?.FirstOrDefault(el => el.ID == id);

			if (category is null) return NotFound();

			_db.Category?.Remove(category);
			_db.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
