using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System;


namespace Library.Controllers
{
  public class LibrarianController : Controller
  {

    [HttpGet("/librarian")]
    public ActionResult Index()
    {
      return View("Index", Book.GetAll());
    }

    [HttpPost("/librarian")]
    public ActionResult New()
    {
      string name = Request.Form["name"];
      int authorId = Int32.Parse(Request.Form["author"]);

      Book book = new Book(name);
      book.Save();
      book.AddAuthor(Author.Find(authorId));
      return View("Index", Book.GetAll());
    }

    [HttpPost("/librarian/new-author")]
    public ActionResult NewAuthor()
    {
      string lastName = Request.Form["last-name"];
      string firstName = Request.Form["first-name"];

      Author author = new Author(lastName, firstName);
      author.Save();
      return View("Index", Book.GetAll());
    }

    [HttpGet("/librarian/books/new")]
    public ActionResult CreateForm()
    {
      return View("CreateForm");
    }

    [HttpGet("/librarian/authors/new")]
    public ActionResult AuthorCreateForm()
    {
      return View("AuthorCreateForm");
    }

    [HttpPost("/librarian/search")]
    public ActionResult Search()
    {
      return View("Index", Book.Search(Request.Form["search"]));
    }

    [HttpGet("/librarian/add/{id}")]
    public ActionResult AddCopy(int id)
    {
      Book.Find(id).AddCopy();
      return View("Index", Book.GetAll());
    }

    [HttpGet("/librarian/remove/{id}")]
    public ActionResult Remove(int id)
    {
      Book.Find(id).Delete();
      return View("Index", Book.GetAll());
    }
  }
}
