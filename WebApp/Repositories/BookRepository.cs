using Microsoft.Data.SqlClient;
using WebApp.Models;

namespace WebApp.Repositories;

public class BookRepository
{
    private readonly string connectionString;
 
    public BookRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public Book? GetById(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = @"SELECT B.Id, B.Title, CONCAT(U.FirstName, ' ',  U.LastName) AS Author, B.Quantity, B.Year, U.Id AS AuthorId 
                           FROM Books B
                           INNER JOIN Users U ON B.AuthorId = U.Id
                           WHERE B.Id = @Id;";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Book
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Author = reader.GetString(2),
                            Quantity = reader.GetInt32(3),
                            Year = reader.GetInt32(4),
                            AuthorId = reader.GetInt32(5)
                        };
                    }
                }
            }
        }

        return null;
    }

    public IList<Book> GetAll()
    {
        var result = new List<Book>();
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = @"SELECT B.Id, B.Title, CONCAT(U.FirstName, ' ',  U.LastName) AS Author, B.Quantity, B.Year,  U.Id AS AuthorId 
                           FROM Books B
                           INNER JOIN Users U ON B.AuthorId = U.Id ";
            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Book
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Author = reader.GetString(2),
                        Quantity = reader.GetInt32(3),
                        Year = reader.GetInt32(4),
                        AuthorId = reader.GetInt32(5)
                    });
                }
            }
        }
        return result;
    }

    public void Add(Book book)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = @"INSERT INTO Books (Title, AuthorId, Quantity, Year) VALUES (@Title, @AuthorId, @Quantity, @Year);";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", book.Title);
                command.Parameters.AddWithValue("@AuthorId", book.AuthorId);
                command.Parameters.AddWithValue("@Quantity", book.Quantity);
                command.Parameters.AddWithValue("@Year", book.Year);
                command.ExecuteNonQuery();
            }
        }
    }

    public bool UpdateBookQuantity(int bookId, int quantity)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = @"UPDATE Books SET Quantity = @Quantity WHERE Id = @Id;";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", bookId);
                command.Parameters.AddWithValue("@Quantity", quantity);
               return command.ExecuteNonQuery() > 0;
            }
        }
    }

    public void Delete(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = @"DELETE FROM Books WHERE Id = @Id;";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteReader();
            }
        }
    }
}
