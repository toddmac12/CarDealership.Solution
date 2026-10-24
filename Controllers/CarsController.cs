using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarDealership.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Controllers
{
  public class CarsController : Controller
  {
    private readonly DealershipContext _db;
    public CarsController(DealershipContext db) { _db = db; }

    [HttpGet("/cars")]
    public async Task<IActionResult> Index(InventoryFilter filter)
    {
      filter.Search = filter.Search?.Trim();
      if (!new[] { "newest", "model", "price", "price-desc", "miles" }.Contains(filter.Sort))
        filter.Sort = "newest";
      var query = _db.Cars.AsNoTracking().AsQueryable();
      var total = await query.CountAsync();
      if (!string.IsNullOrEmpty(filter.Search))
        query = query.Where(car => EF.Functions.Like(car.CarModel, "%" + EscapeLike(filter.Search) + "%", "\\"));
      if (filter.MaxPrice.HasValue) query = query.Where(car => car.Price <= filter.MaxPrice);
      if (filter.MaxMiles.HasValue) query = query.Where(car => car.Miles <= filter.MaxMiles);
      query = filter.Sort switch
      {
        "model" => query.OrderBy(car => car.CarModel).ThenBy(car => car.Id),
        "price" => query.OrderBy(car => car.Price).ThenBy(car => car.Id),
        "price-desc" => query.OrderByDescending(car => car.Price).ThenBy(car => car.Id),
        "miles" => query.OrderBy(car => car.Miles).ThenBy(car => car.Id),
        _ => query.OrderByDescending(car => car.Id)
      };
      return View(new InventoryViewModel { Filter = filter, TotalCount = total,
        Cars = ModelState.IsValid ? await query.ToListAsync() : new List<Car>() });
    }

    private static string EscapeLike(string value) =>
      value.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_");

    [HttpGet("/cars/{id:int}")]
    public async Task<IActionResult> Show(int id)
    {
      var car = await _db.Cars.AsNoTracking().FirstOrDefaultAsync(car => car.Id == id);
      if (car == null) return NotFound();
      return View(car);
    }

    [HttpGet("/cars/new")]
    public IActionResult CreateCar() => View(new CarForm());

    private void ValidateName(CarForm form)
    {
      form.CarModel = form.CarModel?.Trim();
      if (string.IsNullOrEmpty(form.CarModel) || form.CarModel.Length < 2)
        ModelState.AddModelError(nameof(form.CarModel), "Enter at least two non-space characters.");
    }

    [HttpPost("/cars")]
    public async Task<IActionResult> Create(CarForm form)
    {
      ValidateName(form);
      if (!ModelState.IsValid) return View("CreateCar", form);
      var car = new Car { CarModel = form.CarModel, Price = form.Price.Value, Miles = form.Miles.Value };
      _db.Cars.Add(car);
      await _db.SaveChangesAsync();
      TempData["Notice"] = "Listing added.";
      return RedirectToAction(nameof(Show), new { id = car.Id });
    }

    [HttpGet("/cars/{id:int}/edit")]
    public async Task<IActionResult> Edit(int id)
    {
      var car = await _db.Cars.AsNoTracking().FirstOrDefaultAsync(car => car.Id == id);
      if (car == null) return NotFound();
      return View(new CarForm { Id = car.Id, CarModel = car.CarModel, Price = car.Price, Miles = car.Miles });
    }

    [HttpPost("/cars/{id:int}")]
    public async Task<IActionResult> Update(int id, CarForm form)
    {
      var car = await _db.Cars.FindAsync(id);
      if (car == null) return NotFound();
      form.Id = id;
      ValidateName(form);
      if (!ModelState.IsValid) return View("Edit", form);
      car.CarModel = form.CarModel; car.Price = form.Price.Value; car.Miles = form.Miles.Value;
      await _db.SaveChangesAsync();
      TempData["Notice"] = "Listing updated.";
      return RedirectToAction(nameof(Show), new { id });
    }

    [HttpGet("/cars/{id:int}/delete")]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
      var car = await _db.Cars.AsNoTracking().FirstOrDefaultAsync(car => car.Id == id);
      if (car == null) return NotFound();
      return View("Delete", car);
    }

    [HttpPost("/cars/{id:int}/delete")]
    public async Task<IActionResult> Delete(int id)
    {
      var car = await _db.Cars.FindAsync(id);
      if (car == null) return NotFound();
      _db.Cars.Remove(car);
      await _db.SaveChangesAsync();
      TempData["Notice"] = "Listing deleted.";
      return RedirectToAction(nameof(Index));
    }
  }
}
