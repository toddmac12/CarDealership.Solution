using System.ComponentModel.DataAnnotations;

namespace CarDealership.Models
{
  public class Car
  {
    public int Id { get; set; }
    [Required, StringLength(120, MinimumLength = 2)]
    public string CarModel { get; set; }
    [Range(0, int.MaxValue)]
    public int Price { get; set; }
    [Range(0, int.MaxValue)]
    public int Miles { get; set; }
  }
}
