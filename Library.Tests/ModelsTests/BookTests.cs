using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System;
using System.Collections.Generic;

namespace Library.Models.Tests
{
  [TestClass]
  public class BookTests : IDisposable
 {
    public BookTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    public void Dispose()
    {
      // Book.DeleteAll();
      // Person.DeleteAll();
    }

    [TestMethod]
    public void GetAll_ReturnListOfAllBooks_ListBook()
    {
      Book testBook = new Book("How To Program");
      testBook.Save();

      CollectionAssert.AreEqual(new List<Book>{testBook}, Book.GetAll());
    }

    [TestMethod]
    public void Find_ReturnBookWithId_Book()
    {
      Book testBook = new Book("How To Program");
      testBook.Save();
      int id = testBook.GetId();

      Assert.AreEqual(testBook, Book.Find(id));
    }

    [TestMethod]
    public void Delete_DeleteBookFromDatabase_void()
    {
      Book testBook1 = new Book("How To Program");
      testBook1.Save();
      Book testBook2 = new Book("Coding For Dummies");
      testBook2.Save();

      testBook1.Delete();

      CollectionAssert.AreEqual(new List<Book>{testBook2}, Book.GetAll());
    }

    [TestMethod]
    public void GetAuthors_ReturnListOfAllAuthors_ListPerson()
    {
      Book testBook = new Book("How To Program");
      testBook.Save();
      Person author1 = new Person("Bob", "Ross");
      author1.Save();
      Person author2 = new Person("Rob", "Boss");
      author2.Save();
      Person author3 = new Person("Sob", "Boss");
      author3.Save();

      testBook.AddAuthor(author1);
      testBook.AddAuthor(author2);

      CollectionAssert.AreEqual(new List<Person>{author1, author2}, testBook.GetAuthors());
    }

    [TestMethod]
    public void GetCopyCount_ReturnNumberOfCopiesForBook_Int()
    {
      Book testBook1 = new Book("How To Program");
      testBook1.Save();
      Book testBook2 = new Book("Coding For Dummies");
      testBook2.Save();

      testBook1.AddCopy();
      testBook1.AddCopy();
      testBook2.AddCopy();
      testBook2.AddCopy();
      testBook1.AddCopy();

      Assert.AreEqual(3, testBook1.GetCopyCount());
      Assert.AreEqual(2, testBook2.GetCopyCount());
    }
  }
}
