using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Author
  {
    private int _id;
    private string _firstName;
    private string _lastName;

    public Author(string lastName, string firstName, int id = 0)
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

    public override bool Equals(System.Object otherAuthor)
    {
      if (!(otherAuthor is Author))
      {
        return false;
      }
      else
      {
        Author newAuthor = (Author) otherAuthor;
        return _id == newAuthor._id && _firstName == newAuthor._firstName && _lastName == newAuthor._lastName;
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
      cmd.CommandText = @"INSERT INTO authors (firstName, lastName) VALUES (@firstName, @lastName)";
      cmd.Parameters.Add(new MySqlParameter("@firstName", _firstName));
      cmd.Parameters.Add(new MySqlParameter("@lastName", _lastName));

      cmd.ExecuteNonQuery();

      _id = (int)cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public static List<Author> GetAll()
    {
      List<Author> allAuthors = new List<Author>();

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"SELECT * FROM authors";
      MySqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorFirstName = rdr.GetString(1);
        string authorLastName = rdr.GetString(2);
        Author newAuthor = new Author(authorLastName, authorFirstName, authorId);
        allAuthors.Add(newAuthor);
      }

      conn.Close();
      if (conn != null)
        conn.Dispose();
      return allAuthors;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"DELETE FROM authors; DELETE FROM books_authors;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
        conn.Dispose();
    }

    public static Author Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = @"SELECT * FROM `authors` WHERE id = @thisId;";
      cmd.Parameters.Add(new MySqlParameter("@thisId", id));
      MySqlDataReader rdr = cmd.ExecuteReader();

      int authorId = 0;
      string authorFirstName = "";
      string authorLastName = "";

      if(rdr.Read())
      {
        authorId = rdr.GetInt32(0);
        authorFirstName = rdr.GetString(1);
        authorLastName = rdr.GetString(2);
      }
      Author foundAuthor = new Author(authorLastName, authorFirstName, authorId);

      conn.Close();
      if (conn !=null)
        conn.Dispose();

      return foundAuthor;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM authors WHERE id = @thisId;";// DELETE from books_authors WHERE author_id = @thisId;";

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
        SELECT books.* FROM authors
        JOIN books_authors ON (authors.id = books_authors.author_id)
        JOIN books ON (books_authors.book_id = books.id)
        WHERE authors.id = @ThisId;";
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
