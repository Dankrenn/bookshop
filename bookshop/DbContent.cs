using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace bookshop
{
    public class DbContent
    {
       private static string connectionString = "Host = localhost;Database = bookShop; Username = postgres;password = 0211";


        public UserApp GetUserForEmailAndPass(string Email, string Pass) 
        {
            try
            {
                UserApp user = new UserApp();
                string Sqlcmd = "SELECT * FROM users WHERE email = @Email And pass = @Pass";
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(Sqlcmd, conn))
                    {
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Pass", Pass);
                        using(NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                user.Id = reader.GetInt32(0);
                                user.Email = reader.GetValue(1).ToString();
                                user.Password = reader.GetValue(2).ToString(); 
                                user.Rule = reader.GetValue(3).ToString();
                            }
                            else
                            {
                                new Exception("Пользователь ненайден");
                            }
                        }
                    }
                    conn.Close();
                }
                return user;
            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
                throw;
            }
        }
        public void AddUser(UserApp user)
        {
            try
            {
                 string Sqlcmd = "INSERT INTO users(email, pass, rule_user) VALUES (@Email,@Pass,@Rule)";
                using(NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {                   
                    conn.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(Sqlcmd, conn))
                    {
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Pass", user.Password);
                        command.Parameters.AddWithValue("@Rule", "Пользователь");
                        command.ExecuteNonQuery();
                    }
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
                throw;
            }
        }

        public List<Book> GetBooks()
        {
            try
            {
                List<Book> books = new List<Book>();
                string Sqlcmd = "SELECT * FROM book";
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(Sqlcmd, conn))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                               Book book = new Book();
                                book.Id = Convert.ToInt32(reader.GetValue(0));
                                book.Name = reader.GetValue(1).ToString();
                                book.Autor = reader.GetValue(2).ToString();
                                book.Prise = Convert.ToInt32(reader.GetValue(3));
                                book.Count = Convert.ToInt32(reader.GetValue(4));
                                book.TypeBook = reader.GetValue(5).ToString();
                                books.Add(book);
                            }
                        }
                    }
                    conn.Close();
                }
                return books;
            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
                throw;
            }
        }

        public List<Book> GetBooksForStrAndIndex(string str, int  index)
        {
            try
            {
                List<Book> books = new List<Book>();
                string Sqlcmd = "SELECT * FROM book ";
                if (str.Length > 0)
                {
                    Sqlcmd += $"WHERE name_book  ilike '%{str}%' ";
                }
                switch (index)
                {
                    case 0:
                        Sqlcmd += " ORDER BY autor ASC";
                        break;
                    case 1:
                        Sqlcmd += " ORDER BY autor ASC";
                        break;
                    case 2:
                        Sqlcmd += " ORDER BY prise ASC";
                        break;
                    case 3:
                        Sqlcmd += " ORDER BY prise ASC";
                        break;
                }
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(Sqlcmd, conn))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Book book = new Book();
                                book.Id = Convert.ToInt32(reader.GetValue(0));
                                book.Name = reader.GetValue(1).ToString();
                                book.Autor = reader.GetValue(2).ToString();
                                book.Prise = Convert.ToInt32(reader.GetValue(3));
                                book.Count = Convert.ToInt32(reader.GetValue(4));
                                book.TypeBook = reader.GetValue(5).ToString();
                                books.Add(book);
                            }
                        }
                    }
                    conn.Close();
                }
                return books;
            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
                throw;
            }
        }

        public void AddBookForUser(Book book, UserApp user)
        {
            try
            {
                string Sqlcmd = "INSERT INTO orders(id_users, starus, prise_order) VALUES (@id_users,@starus,@prise_order)";
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(Sqlcmd, conn))
                    {
                        command.Parameters.AddWithValue("@id_users", user.Id);
                        command.Parameters.AddWithValue("@starus", "В обработке");
                        command.Parameters.AddWithValue("@prise_order", 0);
                        command.ExecuteNonQuery();
                    }
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
                throw;
            }
        }

        public void AddOrder(Book book, UserApp user, int count, int prise)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string sqlcmd = "INSERT INTO orders (id_users, starus, prise_order) VALUES (@userId, @status, @totalPrice) RETURNING id_orders";
                        // Добавляем заказ в таблицу "orders"
                        using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, connection))
                        {
                            command.Parameters.AddWithValue("userId", user.Id); 
                            command.Parameters.AddWithValue("status", "Новый"); 
                            command.Parameters.AddWithValue("totalPrice", prise); 

                            int orderId = (int)command.ExecuteScalar();
                            string sqlcmd2 = "INSERT INTO ordersbook (id_orders, id_book) VALUES (@orderId, @bookId)";
                            using (NpgsqlCommand command2 = new NpgsqlCommand(sqlcmd2, connection))
                            {
                                command2.Parameters.AddWithValue("orderId", orderId);
                                command2.Parameters.AddWithValue("bookId", book.Id); 
                                command2.ExecuteNonQuery();
                            }
                        }

                        string sqlcmd3 = "UPDATE book SET count_book = count_book - @count WHERE id_book = @bookId";
                        // Уменьшаем количество книг в таблице "book"
                        using (NpgsqlCommand command3 = new NpgsqlCommand(sqlcmd3, connection))
                        {
                            command3.Parameters.AddWithValue("count", count);
                            command3.Parameters.AddWithValue("bookId", book.Id);
                            command3.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Ошибка при добавлении заказа: " + ex.Message);
                    }
                }
            }
        }

        public List<Order> GetOrdersForAdmin()
        {
            try
            {
                List<Order> orders = new List<Order>();
                string Sqlcmd = "SELECT * FROM orders";
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(Sqlcmd, conn))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                               Order order = new Order();
                                order.idOrder = Convert.ToInt32(reader.GetValue(0));
                                order.idUser = Convert.ToInt32(reader.GetValue(1));
                                order.Status = reader.GetValue(2).ToString();
                                order.Prise = Convert.ToInt32(reader.GetValue(3));
                                orders.Add(order);
                            }
                        }
                    }
                    conn.Close();
                }
                return orders;
            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
                throw;
            }
        }

        public void UpdateOrders(string status,Order order)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlcmd = "UPDATE orders SET starus = @starus WHERE id_orders = @id_orders";
                    using (NpgsqlCommand command = new NpgsqlCommand(sqlcmd, connection))
                    {
                        command.Parameters.AddWithValue("@starus", status);
                        command.Parameters.AddWithValue("@id_orders", order.idOrder);
                        command.ExecuteNonQuery();
                        
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
