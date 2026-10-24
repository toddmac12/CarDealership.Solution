using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CarDealership;
using CarDealership.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class InventoryTests : IDisposable
{
  private readonly string _directory = Path.Combine(Path.GetTempPath(), "dealership-test-" + Guid.NewGuid().ToString("N"));
  private string FilePath => Path.Combine(_directory, "dealership.db");
  private WebApplicationFactory<Program> Factory() =>
    new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
      builder.UseEnvironment("Testing").ConfigureAppConfiguration((_, config) =>
        config.AddInMemoryCollection(new Dictionary<string,string> { ["Database:FilePath"] = FilePath })));
  private static HttpClient Client(WebApplicationFactory<Program> factory) =>
    factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
  private static async Task<HttpResponseMessage> Post(HttpClient client, string form, string action, Dictionary<string,string> fields)
  {
    var html = await client.GetStringAsync(form);
    var match = Regex.Match(html, @"name=""__RequestVerificationToken""[^>]*value=""([^""]+)""");
    Assert.True(match.Success);
    fields["__RequestVerificationToken"] = WebUtility.HtmlDecode(match.Groups[1].Value);
    return await client.PostAsync(action, new FormUrlEncodedContent(fields));
  }

  [Fact]
  public async Task SeededInventoryAndEditsPersistAcrossRestartWithoutDuplicateSeeds()
  {
    using (var factory = Factory())
    using (var client = Client(factory))
    {
      Assert.Contains("Toyota Camry", await client.GetStringAsync("/cars"));
      using var scope = factory.Services.CreateScope();
      var db = scope.ServiceProvider.GetRequiredService<DealershipContext>();
      Assert.Equal(12, await db.Cars.CountAsync());
      Assert.False(db.Database.HasPendingModelChanges());
      var response = await Post(client, "/cars/1/edit", "/cars/1", new Dictionary<string,string>
      { ["CarModel"]="Updated Subaru", ["Price"]="17000", ["Miles"]="69000" });
      Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
      Assert.Equal(HttpStatusCode.Redirect, (await Post(client, "/cars/12/delete", "/cars/12/delete",
        new Dictionary<string,string>())).StatusCode);
    }
    using (var factory = Factory())
    using (var client = Client(factory))
    {
      Assert.Contains("Updated Subaru", await client.GetStringAsync("/cars/1"));
      using var scope = factory.Services.CreateScope();
      Assert.Equal(11, await scope.ServiceProvider.GetRequiredService<DealershipContext>().Cars.CountAsync());
      Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync("/cars/12")).StatusCode);
    }
  }

  [Theory]
  [InlineData("", "100", "200")]
  [InlineData(" a ", "100", "200")]
  [InlineData("Valid Car", "-1", "200")]
  [InlineData("Valid Car", "100", "-1")]
  [InlineData("Valid Car", "", "200")]
  [InlineData("Valid Car", "abc", "200")]
  [InlineData("Valid Car", "100", "2147483648")]
  public async Task InvalidCreateDoesNotAddInventory(string model, string price, string miles)
  {
    using var factory = Factory();
    using var client = Client(factory);
    var result = await Post(client, "/cars/new", "/cars", new Dictionary<string,string>
    { ["CarModel"]=model, ["Price"]=price, ["Miles"]=miles });
    Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    Assert.Contains("validation-errors", await result.Content.ReadAsStringAsync());
    using var scope = factory.Services.CreateScope();
    Assert.Equal(12, await scope.ServiceProvider.GetRequiredService<DealershipContext>().Cars.CountAsync());
  }

  [Fact]
  public async Task ValidCreateTrimsNameAndIgnoresPostedId()
  {
    using var factory = Factory();
    using var client = Client(factory);
    var result = await Post(client, "/cars/new", "/cars", new Dictionary<string,string>
    { ["CarModel"]="  New Car  ", ["Price"]="0", ["Miles"]="0", ["Id"]="1" });
    Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
    using var scope = factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DealershipContext>();
    var created = await db.Cars.SingleAsync(c => c.CarModel == "New Car");
    Assert.True(created.Id > 12);
    Assert.Equal(13, await db.Cars.CountAsync());
  }

  [Fact]
  public async Task InvalidUpdateLeavesStoredDataUnchanged()
  {
    using var factory = Factory();
    using var client = Client(factory);
    var result = await Post(client, "/cars/1/edit", "/cars/1", new Dictionary<string,string>
    { ["CarModel"]="Changed", ["Price"]="invalid", ["Miles"]="2" });
    Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    using var scope = factory.Services.CreateScope();
    var car = await scope.ServiceProvider.GetRequiredService<DealershipContext>().Cars.FindAsync(1);
    Assert.Equal("2019 Subaru Forester", car.CarModel);
    Assert.Equal(17500, car.Price);
  }

  [Theory]
  [InlineData("/cars")]
  [InlineData("/cars/1")]
  [InlineData("/cars/1/delete")]
  public async Task UnsafeRequestsRequireAntiforgeryToken(string action)
  {
    using var factory = Factory();
    using var client = Client(factory);
    var result = await client.PostAsync(action, new FormUrlEncodedContent(new Dictionary<string,string>()));
    Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    using var scope = factory.Services.CreateScope();
    Assert.Equal(12, await scope.ServiceProvider.GetRequiredService<DealershipContext>().Cars.CountAsync());
  }

  [Fact]
  public async Task DeleteRequiresConfirmationAndRemovesOnlySelectedRecord()
  {
    using var factory = Factory();
    using var client = Client(factory);
    var confirmation = await client.GetStringAsync("/cars/1/delete");
    Assert.Contains("2019 Subaru Forester", confirmation);
    var result = await Post(client, "/cars/1/delete", "/cars/1/delete", new Dictionary<string,string>());
    Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
    Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync("/cars/1")).StatusCode);
    Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/cars/2")).StatusCode);
  }

  [Theory]
  [InlineData("/cars/9999")]
  [InlineData("/cars/9999/edit")]
  [InlineData("/cars/9999/delete")]
  [InlineData("/not-a-route")]
  [InlineData("/cars/not-a-number")]
  public async Task UnknownRecordsAndRoutesReturn404(string path)
  {
    using var factory = Factory();
    using var client = Client(factory);
    Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync(path)).StatusCode);
  }

  [Fact]
  public async Task SearchFiltersAndSortAreAppliedTogether()
  {
    using var factory = Factory();
    using var client = Client(factory);
    var html = await client.GetStringAsync("/cars?Search=toyota&MaxPrice=23000&MaxMiles=45000&Sort=price");
    Assert.Contains("2021 Toyota Camry", html);
    Assert.DoesNotContain("Subaru Forester", html);
    Assert.Single(Regex.Matches(html, "data-listing-id="));
    var sorted = await client.GetStringAsync("/cars?Sort=price");
    Assert.True(sorted.IndexOf("Volkswagen Golf", StringComparison.Ordinal) < sorted.IndexOf("Tesla Model 3", StringComparison.Ordinal));
    var literalWildcard = await client.GetStringAsync("/cars?Search=%25");
    Assert.DoesNotContain("data-listing-id=", literalWildcard);
  }

  [Fact]
  public async Task InvalidNumericFilterDisplaysValidationInsteadOfUnfilteredResults()
  {
    using var factory = Factory();
    using var client = Client(factory);
    var html = await client.GetStringAsync("/cars?MaxPrice=abc");
    Assert.Contains("validation-errors", html);
    Assert.DoesNotContain("data-listing-id=", html);
  }


  [Fact]
  public async Task UpdateUsesRouteIdEvenWhenFormSuppliesAnotherId()
  {
    using var factory = Factory();
    using var client = Client(factory);
    var result = await Post(client, "/cars/1/edit", "/cars/1", new Dictionary<string,string>
    { ["Id"]="2", ["CarModel"]="Route One", ["Price"]="100", ["Miles"]="200" });
    Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
    using var scope = factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DealershipContext>();
    Assert.Equal("Route One", (await db.Cars.FindAsync(1)).CarModel);
    Assert.Equal("2021 Toyota Camry", (await db.Cars.FindAsync(2)).CarModel);
  }

  [Fact]
  public async Task DeleteUsesRouteIdEvenWhenFormSuppliesAnotherId()
  {
    using var factory = Factory();
    using var client = Client(factory);
    var result = await Post(client, "/cars/1/delete", "/cars/1/delete",
      new Dictionary<string,string> { ["Id"]="2" });
    Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
    using var scope = factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DealershipContext>();
    Assert.Null(await db.Cars.FindAsync(1));
    Assert.NotNull(await db.Cars.FindAsync(2));
  }

  public void Dispose()
  {
    Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
    if (Directory.Exists(_directory)) Directory.Delete(_directory, true);
  }
}
