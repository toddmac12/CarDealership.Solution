using System.Collections.Generic;

namespace CarDealership.Models
{
  public class Car
  {
    public string CarModel { get; set; }
    public int Price { get; set; }
    public int Miles { get; set; }
    public int Id { get; }
    private static List<Car> _carList = new List<Car>();
    private static int _currentId = 1;

    public Car(string carModel, int price, int miles)
    {
      CarModel = carModel;
      Price = price;
      Miles = miles;
      Id = _currentId;
      _currentId++;
      _carList.Add(this);
    }

    public static List<Car> GetAll()
    {
      return _carList;
    }

    public static Car Find(int id)
    {
      return _carList.Find(car => car.Id == id);
    }

    public void Update(string carModel, int price, int miles)
    {
      CarModel = carModel;
      Price = price;
      Miles = miles;
    }

    public static void Delete(int id)
    {
      Car car = Find(id);
      if (car != null)
      {
        _carList.Remove(car);
      }
    }

    public static void ClearAll()
    {
      _carList.Clear();
      _currentId = 1;
    }
  }
}
