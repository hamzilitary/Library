using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
  public class PatronController : Controller
  {

    [HttpGet("/patron")]
    public ActionResult Index()
    {
      return View("Index", Book.GetAll());
    }

    [HttpPost("/patron/search")]
    public ActionResult Search()
    {
      return View("Index", Book.Search(Request.Form["search"]));
    }
  }
}
