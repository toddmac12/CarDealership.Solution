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

        [HttpGet("/cars/new")]
        public ActionResult CreateCar()
        {
            return View();
        }

        [HttpPost("/cars")]
        public ActionResult Create(string carModel, int price, int miles)
        {
            Car myCar = new Car(carModel, price, miles);
            return RedirectToAction("Index");
        }
    }
}

