using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System;
using System.Collections.Generic;

namespace Library.Models.Tests
{
  [TestClass]
  public class AuthorTests : IDisposable
 {
    public AuthorTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    public void Dispose()
    {
      Book.DeleteAll();
      Author.DeleteAll();
    }

    [TestMethod]
    public void GetAll_ReturnListOfAllAuthors_ListAuthor()
    {
      Author testAuthor = new Author("John", "Smith");
      testAuthor.Save();

      CollectionAssert.AreEqual(new List<Author>{testAuthor}, Author.GetAll());
    }

    [TestMethod]
    public void Find_ReturnAuthorWithId_Author()
    {
      Author testAuthor = new Author("John", "Smith");
      testAuthor.Save();
      int id = testAuthor.GetId();

      Assert.AreEqual(testAuthor, Author.Find(id));
    }

    [TestMethod]
    public void Delete_DeleteAuthorFromDatabase_void()
    {
      Author testAuthor1 = new Author("John", "Smith");
      testAuthor1.Save();
      Author testAuthor2 = new Author("Jane", "Doe");
      testAuthor2.Save();

      testAuthor1.Delete();

      CollectionAssert.AreEqual(new List<Author>{testAuthor2}, Author.GetAll());
    }

    [TestMethod]
    public void GetBooks_ReturnListOfAllBooks_ListBook()
    {
      Book testBook1 = new Book("How To Program");
      testBook1.Save();
      Book testBook2 = new Book("Coding For Dummies");
      testBook2.Save();
      Book testBook3 = new Book("Learning C#");
      testBook3.Save();

      Author author = new Author("Bob", "Ross");
      author.Save();

      author.AddBook(testBook1);
      author.AddBook(testBook3);

      CollectionAssert.AreEqual(new List<Book>{testBook1, testBook3}, author.GetBooks());
    }
  }
}
