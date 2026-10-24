using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarDealership.Models
{
  public class InventoryFilter
  {
    [StringLength(120)]
    public string Search { get; set; }
    [Range(0, int.MaxValue)]
    public int? MaxPrice { get; set; }
    [Range(0, int.MaxValue)]
    public int? MaxMiles { get; set; }
    public string Sort { get; set; } = "newest";
  }

  public class InventoryViewModel
  {
    public List<Car> Cars { get; set; }
    public InventoryFilter Filter { get; set; }
    public int TotalCount { get; set; }
  }
}
