using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public IActionResult Index() => View();

    [Route("/error")]
    public IActionResult Error()
    {
      Response.StatusCode = 500;
      return Content("Something went wrong. Please try again.");
    }
  }
}
