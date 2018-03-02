using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Checkout
  {
    public int Id;
    public string BookName;
    public DateTime CheckoutDate;
    public DateTime DueDate;
  }
}
