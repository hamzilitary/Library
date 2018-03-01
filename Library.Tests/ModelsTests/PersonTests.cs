using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System;
using System.Collections.Generic;

namespace Library.Models.Tests
{
  [TestClass]
  public class PersonTests : IDisposable
 {
    public PersonTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    public void Dispose()
    {
      // Book.DeleteAll();
      // Person.DeleteAll();
    }

    [TestMethod]
    public void GetAll_ReturnListOfAllPersons_ListPerson()
    {
      Person testPerson = new Person("John", "Smith");
      testPerson.Save();

      CollectionAssert.AreEqual(new List<Person>{testPerson}, Person.GetAll());
    }

    [TestMethod]
    public void Find_ReturnPersonWithId_Person()
    {
      Person testPerson = new Person("John", "Smith");
      testPerson.Save();
      int id = testPerson.GetId();

      Assert.AreEqual(testPerson, Person.Find(id));
    }

    [TestMethod]
    public void Delete_DeletePersonFromDatabase_void()
    {
      Person testPerson1 = new Person("John", "Smith");
      testPerson1.Save();
      Person testPerson2 = new Person("Jane", "Doe");
      testPerson2.Save();

      testPerson1.Delete();

      CollectionAssert.AreEqual(new List<Person>{testPerson2}, Person.GetAll());
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

      Person author = new Person("Bob", "Ross");
      author.Save();

      author.AddBook(testBook1);
      author.AddBook(testBook3);

      CollectionAssert.AreEqual(new List<Book>{testBook1, testBook3}, author.GetBooks());
    }
  }
}
