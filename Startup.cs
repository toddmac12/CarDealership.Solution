using System.IO;
using CarDealership.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarDealership
{
  public class Startup
  {
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    { _configuration = configuration; _environment = environment; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllersWithViews(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
      services.AddDbContext<DealershipContext>(options =>
      {
        var file = _configuration["Database:FilePath"] ?? Path.Combine(_environment.ContentRootPath, "App_Data", "dealership.db");
        file = Path.GetFullPath(file, _environment.ContentRootPath);
        Directory.CreateDirectory(Path.GetDirectoryName(file));
        options.UseSqlite(new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder
        { DataSource = file, ForeignKeys = true }.ToString());
      });
    }

    public void Configure(IApplicationBuilder app)
    {
      using (var scope = app.ApplicationServices.CreateScope())
        scope.ServiceProvider.GetRequiredService<DealershipContext>().Database.Migrate();
      if (_environment.IsDevelopment()) app.UseDeveloperExceptionPage();
      else app.UseExceptionHandler("/error");
      app.UseStaticFiles();
      app.UseRouting();
      app.UseEndpoints(routes => routes.MapControllers());
    }
  }
}
