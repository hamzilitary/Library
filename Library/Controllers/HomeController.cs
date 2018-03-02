using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System;

namespace Library.Controllers
{
  public class HomeController : Controller
  {

    [HttpGet("/")]
    public ActionResult Index()
    {
      return View("Index", Book.GetAll());
    }

    [HttpPost("/")]
    public ActionResult New()
    {
      string name = Request.Form["name"];
      Book book = new Book(name);
      book.Save();

      int numAuthors = Int32.Parse(Request.Form["number-of-authors"]);
      for (int i = 0; i < numAuthors; i++) {
        int authorId = Int32.Parse(Request.Form["author" + i]);
        book.AddAuthor(Author.Find(authorId));
      }

      return View("Index", Book.GetAll());
    }

    [HttpPost("/new-author")]
    public ActionResult NewAuthor()
    {
      string lastName = Request.Form["last-name"];
      string firstName = Request.Form["first-name"];

      Author author = new Author(lastName, firstName);
      author.Save();
      return View("Index", Book.GetAll());
    }

    [HttpGet("/books/new")]
    public ActionResult CreateForm()
    {
      return View("CreateForm");
    }

    [HttpGet("/authors/new")]
    public ActionResult AuthorCreateForm()
    {
      return View("AuthorCreateForm");
    }

    [HttpPost("/search")]
    public ActionResult Search()
    {
      return View("Index", Book.Search(Request.Form["search"]));
    }

    [HttpGet("/add/{id}")]
    public ActionResult AddCopy(int id)
    {
      Book.Find(id).AddCopy();
      return View("Index", Book.GetAll());
    }

    [HttpGet("/remove/{id}")]
    public ActionResult RemoveCopy(int id)
    {
      Book.Find(id).RemoveCopy();
      return View("Index", Book.GetAll());
    }

    [HttpGet("/delete/{id}")]
    public ActionResult Delete(int id)
    {
      Book.Find(id).Delete();
      return View("Index", Book.GetAll());
    }
  }
}
