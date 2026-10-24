using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarDealership.Models
{
  public class CarForm
  {
    [BindNever]
    public int Id { get; set; }
    [Required(ErrorMessage = "Enter a car model."), StringLength(120, MinimumLength = 2)]
    public string CarModel { get; set; }
    [Required(ErrorMessage = "Enter a price."), Range(0, int.MaxValue)]
    public int? Price { get; set; }
    [Required(ErrorMessage = "Enter mileage."), Range(0, int.MaxValue)]
    public int? Miles { get; set; }
  }
}
