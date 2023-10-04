using DumpCity.Data;
using DumpCity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DumpCity.Controllers
{
	[Authorize(Roles = WC.adminRole)]
	public class AppTypeController : Controller
	{
		private readonly AppDbContext _db;

		public AppTypeController(AppDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			IEnumerable<AppType> appTypeList = new List<AppType>();

			if (_db.AppType is not null) appTypeList = _db.AppType;

			return View(appTypeList);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(AppType appType)
		{
			if (ModelState.IsValid)
			{
				_db.AppType?.Add(appType);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(appType);
		}

		[HttpGet]
		public IActionResult Edit(int? id)
		{
			if (id is null || id == 0) return NotFound();

			AppType? appType = new();

			appType = _db.AppType?.FirstOrDefault(el => el.ID == id);

			if (appType is null) return NotFound();

			return View(appType);
		}

		[HttpPost]
		public IActionResult Edit(AppType appType)
		{
			if (ModelState.IsValid)
			{
				_db.AppType?.Update(appType);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(appType);
		}

		public IActionResult Delete(int? id)
		{
			if (id is null || id == 0) return NotFound();

			AppType? appType = new();

			appType = _db.AppType?.FirstOrDefault(el => el.ID == id);

			if (appType is null) return NotFound();

			_db.AppType?.Remove(appType);
			_db.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
