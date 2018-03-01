using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
  public class HomeController : Controller
  {

    [HttpGet("/")]
    public ActionResult Login()
    {
      return View("Index", "Hello World");
    }

    [HttpPost("/")]
    public ActionResult Index()
    {
      string loginChoice = Request.Form["loginChoice"];
      if (loginChoice == "librarian")
        return RedirectToAction("Index", "Librarian");
      else if (loginChoice == "patron") {
        return RedirectToAction("Index", "Patron");
      }
      return View();
    }
  }
}
