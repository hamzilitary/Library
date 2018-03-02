using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System;
using System.Collections.Generic;

namespace Library.Models.Tests
{
  [TestClass]
  public class PatronTests : IDisposable
 {
    public PatronTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    public void Dispose()
    {
      Book.DeleteAll();
      Patron.DeleteAll();
    }

    [TestMethod]
    public void GetAll_ReturnListOfAllPatrons_ListPatron()
    {
      Patron testPatron = new Patron("John", "Smith");
      testPatron.Save();

      CollectionAssert.AreEqual(new List<Patron>{testPatron}, Patron.GetAll());
    }

    [TestMethod]
    public void Find_ReturnPatronWithId_Patron()
    {
      Patron testPatron = new Patron("John", "Smith");
      testPatron.Save();
      int id = testPatron.GetId();

      Assert.AreEqual(testPatron, Patron.Find(id));
    }

    [TestMethod]
    public void Delete_DeletePatronFromDatabase_void()
    {
      Patron testPatron1 = new Patron("John", "Smith");
      testPatron1.Save();
      Patron testPatron2 = new Patron("Jane", "Doe");
      testPatron2.Save();

      testPatron1.Delete();

      CollectionAssert.AreEqual(new List<Patron>{testPatron2}, Patron.GetAll());
    }

    [TestMethod]
    public void GetCheckoutHistory_ReturnAllCheckouts_ListCheckout()
    {
      Patron testPatron = new Patron("John", "Smith");
      testPatron.Save();

      Book testBook = new Book("The Test Book");
      testBook.Save();

      bool success = testPatron.CheckoutBook(testBook.GetId());
      Assert.AreEqual(false, success);

      testBook.AddCopy();
      success = testPatron.CheckoutBook(testBook.GetId());
      Assert.AreEqual(true, success);

      Checkout checkout = testPatron.GetCheckoutHistory()[0];
      Assert.AreEqual(testBook.GetName(), checkout.BookName);
    }
  }
}
