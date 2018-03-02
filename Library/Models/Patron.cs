using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Patron
  {
    private int _id;
    private string _firstName;
    private string _lastName;

    public Patron(string lastName, string firstName, int id = 0)
    {
      _id = id;
      _firstName = firstName;
      _lastName = lastName;
    }
    public string GetFirstName()
    {
      return _firstName;
    }
    public string GetLastName()
    {
      return _lastName;
    }
    public int GetId()
    {
      return _id;
    }

    public override bool Equals(System.Object otherPatron)
    {
      if (!(otherPatron is Patron))
      {
        return false;
      }
      else
      {
        Patron newPatron = (Patron) otherPatron;
        return _id == newPatron._id && _firstName == newPatron._firstName && _lastName == newPatron._lastName;
      }
    }

    public override int GetHashCode()
    {
      return _id.GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"INSERT INTO patrons (firstName, lastName) VALUES (@firstName, @lastName)";
      cmd.Parameters.Add(new MySqlParameter("@firstName", _firstName));
      cmd.Parameters.Add(new MySqlParameter("@lastName", _lastName));

      cmd.ExecuteNonQuery();

      _id = (int)cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public static List<Patron> GetAll()
    {
      List<Patron> allPatrons = new List<Patron>();

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"SELECT * FROM patrons";
      MySqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int patronId = rdr.GetInt32(0);
        string patronFirstName = rdr.GetString(1);
        string patronLastName = rdr.GetString(2);
        Patron newPatron = new Patron(patronLastName, patronFirstName, patronId);
        allPatrons.Add(newPatron);
      }

      conn.Close();
      if (conn != null)
        conn.Dispose();
      return allPatrons;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"DELETE FROM patrons;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public static Patron Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"SELECT * FROM `patrons` WHERE id = @thisId;";
      cmd.Parameters.Add(new MySqlParameter("@thisId", id));
      MySqlDataReader rdr = cmd.ExecuteReader();

      int patronId = 0;
      string patronFirstName = "";
      string patronLastName = "";

      if(rdr.Read())
      {
        patronId = rdr.GetInt32(0);
        patronFirstName = rdr.GetString(1);
        patronLastName = rdr.GetString(2);
      }
      Patron foundPatron = new Patron(patronLastName, patronFirstName, patronId);

      conn.Close();
      if (conn !=null)
        conn.Dispose();

      return foundPatron;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM patrons WHERE id = @thisId;";// DELETE from books_patrons WHERE patron_id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = this._id;
      cmd.Parameters.Add(thisId);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public bool CheckoutBook(int bookId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"SELECT id FROM copies WHERE book_id = @BookId AND checked_out = @CheckedOut;";
      cmd.Parameters.AddWithValue("@BookId", bookId);
      cmd.Parameters.AddWithValue("@CheckedOut", false);
      MySqlDataReader rdr = cmd.ExecuteReader();
      if (!rdr.Read())
        return false;
      int copyId = rdr.GetInt32(0);
      conn.Close();
      if (conn != null)
        conn.Dispose();

      conn = DB.Connection();
      conn.Open();
      cmd = conn.CreateCommand();
      cmd.CommandText = @"
        UPDATE copies SET checked_out = true WHERE id = @CopyId;
        INSERT INTO checkouts (copy_id, patron_id, checkout_date) VALUES (@CopyId, @PatronId, @Date);
      ";
      cmd.Parameters.AddWithValue("@CopyId", copyId);
      cmd.Parameters.AddWithValue("@PatronId", _id);
      cmd.Parameters.AddWithValue("@Date", DateTime.Now);
      cmd.ExecuteNonQuery();
      int checkoutId = (int)cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
        conn.Dispose();

        conn = DB.Connection();
        conn.Open();
        cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO active_checkouts (checkout_id, due_date) VALUES (@CheckoutId, @Date);";
        cmd.Parameters.AddWithValue("@CheckoutId", checkoutId);
        cmd.Parameters.AddWithValue("@Date", DateTime.Now.AddDays(14));
        cmd.ExecuteNonQuery();

        conn.Close();
        if (conn != null)
          conn.Dispose();
      return true;
    }

    public void ReturnBook(int checkoutId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();

      cmd.CommandText = @"
        UPDATE active_checkouts
        JOIN checkouts ON (active_checkouts.checkout_id = checkouts.id)
        JOIN copies ON (checkouts.copy_id = copies.id)
        SET copies.checked_out = false
        WHERE active_checkouts.id = @CheckoutId;

        DELETE FROM active_checkouts WHERE id = @CheckoutId;
      ";
      cmd.Parameters.AddWithValue("@CheckoutId", checkoutId);
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public List<Checkout> GetCheckoutHistory()
    {
      List<Checkout> allCheckouts = new List<Checkout>();

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"
        SELECT checkouts.checkout_date, books.name FROM checkouts
        JOIN copies ON (checkouts.copy_id = copies.id)
        JOIN books ON (copies.book_id = books.id)
        WHERE checkouts.patron_id = @ThisId
      ;";
      cmd.Parameters.AddWithValue("@ThisId", _id);
      MySqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        Checkout newCheckout = new Checkout();
        newCheckout.CheckoutDate = rdr.GetDateTime(0);
        newCheckout.BookName = rdr.GetString(1);
        allCheckouts.Add(newCheckout);
      }

      conn.Close();
      if (conn != null)
        conn.Dispose();
      return allCheckouts;
    }

    public List<Checkout> GetActiveCheckouts()
    {
      List<Checkout> allCheckouts = new List<Checkout>();

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"
        SELECT  checkouts.checkout_date, books.name, active_checkouts.due_date, active_checkouts.id FROM active_checkouts
        JOIN checkouts ON (active_checkouts.checkout_id = checkouts.id)
        JOIN copies ON (checkouts.copy_id = copies.id)
        JOIN books ON (copies.book_id = books.id)
        WHERE checkouts.patron_id = @ThisId
      ;";
      cmd.Parameters.AddWithValue("@ThisId", _id);
      MySqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        Checkout newCheckout = new Checkout();
        newCheckout.CheckoutDate = rdr.GetDateTime(0);
        newCheckout.BookName = rdr.GetString(1);
        newCheckout.DueDate = rdr.GetDateTime(2);
        newCheckout.Id = rdr.GetInt32(3);
        allCheckouts.Add(newCheckout);
      }

      conn.Close();
      if (conn != null)
        conn.Dispose();
      return allCheckouts;
    }
  }
}
