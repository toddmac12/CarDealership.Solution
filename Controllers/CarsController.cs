using Microsoft.AspNetCore.Mvc;
using CarDealership.Models;
using System.Collections.Generic;

namespace CarDealership.Controllers
{
  public class CarsController : Controller

  {
    [HttpGet("/cars")]
    public ActionResult Index()
    {
      List<Car> allCars = Car.GetAll();
      return View(allCars);
    }

    [HttpGet("/cars/{id}")]
    public ActionResult Show(int id)
    {
      Car car = Car.Find(id);
      if (car == null)
      {
        return NotFound();
      }
      return View(car);
    }

    [HttpGet("/cars/new")]
    public ActionResult CreateCar()
    {
      return View();
    }

    [HttpPost("/cars")]
    public ActionResult Create(string carModel, int price, int miles)
    {
      Car myCar = new Car(carModel, price, miles);
      return RedirectToAction("Show", new { id = myCar.Id });
    }

    [HttpGet("/cars/{id}/edit")]
    public ActionResult Edit(int id)
    {
      Car car = Car.Find(id);
      if (car == null)
      {
        return NotFound();
      }
      return View(car);
    }

    [HttpPost("/cars/{id}")]
    public ActionResult Update(int id, string carModel, int price, int miles)
    {
      Car car = Car.Find(id);
      if (car == null)
      {
        return NotFound();
      }
      car.Update(carModel, price, miles);
      return RedirectToAction("Show", new { id = id });
    }

    [HttpPost("/cars/{id}/delete")]
    public ActionResult Delete(int id)
    {
      Car.Delete(id);
      return RedirectToAction("Index");
    }
  }
}

