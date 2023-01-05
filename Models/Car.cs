using System.Collections.Generic;

namespace CarDealership.Models
{
  public class Car
  {
    public string CarModel { get; set; }
    public int Price { get; set; }
    public int Miles { get; set; }
    public int Id { get; }
    private static List<Car> CarList = new List<Car> {};

    public Car(string carModel, int price, int miles)
    {
      CarModel = carModel;
      Price = price;
      Miles = miles;
      CarList.Add(this);
      Id = CarList.Count;
    }

    public static List<Car> GetAll()
    {
      return CarList;
    }
  }
}
