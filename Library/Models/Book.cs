using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Book
  {
    private int _id;
    private string _name;

    public Book(string name, int id = 0)
    {
      _id = id;
      _name = name;
    }
    public string GetName()
    {
      return _name;
    }
    public int GetId()
    {
      return _id;
    }

    public override bool Equals(System.Object otherBook)
    {
      if (!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        return _id == newBook._id && _name == newBook._name;
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
      cmd.CommandText = @"INSERT INTO books (name) VALUES (@name)";
      cmd.Parameters.Add(new MySqlParameter("@name", _name));

      cmd.ExecuteNonQuery();

      _id = (int)cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public static List<Book> GetAll()
    {
      List<Book> allBooks = new List<Book>();

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"SELECT * FROM books";
      MySqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookName = rdr.GetString(1);
        Book newBook = new Book(bookName, bookId);
        allBooks.Add(newBook);
      }

      conn.Close();
      if (conn != null)
        conn.Dispose();
      return allBooks;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"
        TRUNCATE TABLE books;
        TRUNCATE TABLE books_authors;
        TRUNCATE TABLE copies;
      ";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public static Book Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"SELECT * FROM `books` WHERE id = @thisId;";
      cmd.Parameters.Add(new MySqlParameter("@thisId", id));
      MySqlDataReader rdr = cmd.ExecuteReader();

      int bookId = 0;
      string bookName = "";

      if(rdr.Read())
      {
        bookId = rdr.GetInt32(0);
        bookName = rdr.GetString(1);
      }
      Book foundBook = new Book(bookName, bookId);

      conn.Close();
      if (conn !=null)
        conn.Dispose();

      return foundBook;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM books WHERE id = @thisId;";// DELETE from books_authors WHERE book_id = @thisId;";

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

    public void AddAuthor(Person author)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"INSERT INTO books_authors (book_id, author_id) VALUES (@BookId, @AuthorId)";
      cmd.Parameters.Add(new MySqlParameter("@AuthorId", author.GetId()));
      cmd.Parameters.Add(new MySqlParameter("@BookId", _id));
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public List<Person> GetAuthors()
    {
      List<Person> authors = new List<Person>();

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"
        SELECT persons.* FROM books
        JOIN books_authors ON (books.id = books_authors.book_id)
        JOIN persons ON (books_authors.author_id = persons.id)
        WHERE books.id = @ThisId
      ";
      cmd.Parameters.Add(new MySqlParameter("@ThisId", _id));
      MySqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string firstName = rdr.GetString(1);
        string lastName = rdr.GetString(2);
        Person newAuthor = new Person(firstName, lastName, authorId);
        authors.Add(newAuthor);
      }

      conn.Close();
      if (conn != null)
        conn.Dispose();

      return authors;
    }

    public void AddCopy()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"INSERT INTO copies (book_id) VALUES (@BookId)";
      cmd.Parameters.Add(new MySqlParameter("@BookId", _id));
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public int GetCopyCount()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"SELECT * FROM copies WHERE book_id = @thisId";
      cmd.Parameters.AddWithValue("@thisId", _id);
      MySqlDataReader rdr = cmd.ExecuteReader();
      int count = 0;
      while(rdr.Read())
        count++;

      conn.Close();
      if (conn != null)
        conn.Dispose();
      return count;
    }
  }
}
