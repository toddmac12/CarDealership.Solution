using Microsoft.EntityFrameworkCore;

namespace CarDealership.Models
{
  public class DealershipContext : DbContext
  {
    public DealershipContext(DbContextOptions<DealershipContext> options) : base(options) { }
    public DbSet<Car> Cars { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<Car>().HasData(
        new Car { Id = 1, CarModel = "2019 Subaru Forester", Price = 17500, Miles = 68000 },
        new Car { Id = 2, CarModel = "2021 Toyota Camry", Price = 22900, Miles = 42000 },
        new Car { Id = 3, CarModel = "2020 Honda Civic", Price = 18900, Miles = 51000 },
        new Car { Id = 4, CarModel = "2018 Ford F-150", Price = 24900, Miles = 87000 },
        new Car { Id = 5, CarModel = "2022 Hyundai Tucson", Price = 25900, Miles = 29000 },
        new Car { Id = 6, CarModel = "2020 Chevrolet Equinox", Price = 17900, Miles = 62000 },
        new Car { Id = 7, CarModel = "2021 Mazda CX-5", Price = 23900, Miles = 38000 },
        new Car { Id = 8, CarModel = "2017 Jeep Wrangler", Price = 21900, Miles = 91000 },
        new Car { Id = 9, CarModel = "2022 Kia Soul", Price = 16900, Miles = 33000 },
        new Car { Id = 10, CarModel = "2019 Nissan Altima", Price = 15900, Miles = 72000 },
        new Car { Id = 11, CarModel = "2023 Tesla Model 3", Price = 28900, Miles = 18000 },
        new Car { Id = 12, CarModel = "2018 Volkswagen Golf", Price = 13900, Miles = 83000 }
      );
    }
  }
}
