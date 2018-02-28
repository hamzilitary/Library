using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Person
  {
    private int _id;
    private string _firstName;
    private string _lastName;

    public Person(string firstName, string lastName, int id = 0)
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

    public override bool Equals(System.Object otherPerson)
    {
      if (!(otherPerson is Person))
      {
        return false;
      }
      else
      {
        Person newPerson = (Person) otherPerson;
        return _id == newPerson._id && _firstName == newPerson._firstName && _lastName == newPerson._lastName;
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
      cmd.CommandText = @"INSERT INTO persons (firstName, lastName) VALUES (@firstName, @lastName)";
      cmd.Parameters.Add(new MySqlParameter("@firstName", _firstName));
      cmd.Parameters.Add(new MySqlParameter("@lastName", _lastName));

      cmd.ExecuteNonQuery();

      _id = (int)cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public static List<Person> GetAll()
    {
      List<Person> allPersons = new List<Person>();

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"SELECT * FROM persons";
      MySqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int personId = rdr.GetInt32(0);
        string personFirstName = rdr.GetString(1);
        string personLastName = rdr.GetString(2);
        Person newPerson = new Person(personFirstName, personLastName, personId);
        allPersons.Add(newPerson);
      }

      conn.Close();
      if (conn != null)
        conn.Dispose();
      return allPersons;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"TRUNCATE TABLE persons; TRUNCATE TABLE books_authors;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public static Person Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"SELECT * FROM `persons` WHERE id = @thisId;";
      cmd.Parameters.Add(new MySqlParameter("@thisId", id));
      MySqlDataReader rdr = cmd.ExecuteReader();

      int personId = 0;
      string personFirstName = "";
      string personLastName = "";

      if(rdr.Read())
      {
        personId = rdr.GetInt32(0);
        personFirstName = rdr.GetString(1);
        personLastName = rdr.GetString(2);
      }
      Person foundPerson = new Person(personFirstName, personLastName, personId);

      conn.Close();
      if (conn !=null)
        conn.Dispose();

      return foundPerson;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM persons WHERE id = @thisId;";// DELETE from books_authors WHERE person_id = @thisId;";

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

    public void AddBook(Book book)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"INSERT INTO books_authors (book_id, author_id) VALUES (@BookId, @AuthorId)";
      cmd.Parameters.Add(new MySqlParameter("@AuthorId", _id));
      cmd.Parameters.Add(new MySqlParameter("@BookId", book.GetId()));
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public List<Book> GetBooks()
    {
      List<Book> books = new List<Book>();

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"
        SELECT books.* FROM persons
        JOIN books_authors ON (persons.id = books_authors.author_id)
        JOIN books ON (books_authors.book_id = books.id)
        WHERE persons.id = @ThisId;";
      cmd.Parameters.Add(new MySqlParameter("@ThisId", _id));
      MySqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookName = rdr.GetString(1);
        Book newBook = new Book(bookName, bookId);
        books.Add(newBook);
      }

      conn.Close();
      if (conn != null)
        conn.Dispose();

      return books;
    }
  }
}
