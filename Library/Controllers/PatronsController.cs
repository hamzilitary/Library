using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System;
using System.Collections.Generic;

namespace Library.Controllers
{
  public class PatronsController : Controller
  {
    [HttpGet("/patrons")]
    public ActionResult Index()
    {
      return View("Index", Patron.GetAll());
    }

    [HttpPost("/patrons")]
    public ActionResult New()
    {
      string lastName = Request.Form["last-name"];
      string firstName = Request.Form["first-name"];

      Patron patron = new Patron(lastName, firstName);
      patron.Save();
      return View("Index", Patron.GetAll());
    }

    [HttpGet("/patrons/new")]
    public ActionResult CreateForm()
    {
      return View("CreateForm");
    }

    [HttpGet("/patrons/{id}/delete")]
    public ActionResult Delete(int id)
    {
      Patron.Find(id).Delete();
      return View("Index", Patron.GetAll());
    }

    [HttpGet("/patrons/{id}")]
    public ActionResult Details(int id)
    {
      return View("Details", Patron.Find(id));
    }

    [HttpGet("/patrons/{id}/checkout")]
    public ActionResult Checkout(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      model.Add("books", Book.GetAll());
      model.Add("patronId", id);
      return View("Checkout", model);
    }

    [HttpGet("/patrons/{id}/checkout/{bookId}")]
    public ActionResult Checkout(int id, int bookId)
    {
      Patron patron = Patron.Find(id);
      patron.CheckoutBook(bookId);

      Dictionary<string, object> model = new Dictionary<string, object>();
      model.Add("books", Book.GetAll());
      model.Add("patronId", id);
      return View("Checkout", model);
    }

    [HttpGet("/patrons/{id}/return/{checkoutId}")]
    public ActionResult ReturnBook(int id, int checkoutId)
    {
      Patron patron = Patron.Find(id);
      patron.ReturnBook(checkoutId);
      return View("Details", patron);
    }
  }

}
