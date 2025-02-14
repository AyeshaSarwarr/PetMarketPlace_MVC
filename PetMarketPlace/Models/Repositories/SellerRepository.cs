using Microsoft.Data.SqlClient;
using WebProject.Models.Entities;
using WebProject.Models.Interfaces;

namespace WebProject.Models.Repositories
{
    public class SellerRepository : ISellerInterface
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PetPalace;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public (SellerEntity,int) Login(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Sellers.*, Users.Username, Users.Password FROM Sellers " +
                               "INNER JOIN Users ON Sellers.UserId = Users.UserId " +
                               "WHERE Users.Username = @Username AND Users.Password = @Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            SellerEntity s = new SellerEntity();

                            s.UserId = reader.GetInt32(reader.GetOrdinal("UserId"));
                            s.Username = reader.GetString(reader.GetOrdinal("Username"));
                            s.Password = reader.GetString(reader.GetOrdinal("Password"));
                            s.BusinessName = reader.GetString(reader.GetOrdinal("BusinessName"));
                            s.BusinessAddress = reader.GetString(reader.GetOrdinal("BusinessAddress"));
                            s.ContactNumber = reader.GetString(reader.GetOrdinal("ContactNumber"));
                            s.Website = reader.GetString(reader.GetOrdinal("Website"));
                            var userId = s.UserId;

                            return (s,s.UserId);
                        }
                    }
                }
                return (null,0); // No matching seller found
            }
        }

        public void Signup(SellerEntity seller)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string userQuery = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)";
                SqlCommand userCommand = new SqlCommand(userQuery, connection);
                userCommand.Parameters.AddWithValue("@Username", seller.Username);
                userCommand.Parameters.AddWithValue("@Password", seller.Password);
                userCommand.Parameters.AddWithValue("@Role", "Seller");
                userCommand.ExecuteNonQuery();

                string userIdQuery = "SELECT UserId FROM Users WHERE Username = @Username";
                SqlCommand userIdCommand = new SqlCommand(userIdQuery, connection);
                userIdCommand.Parameters.AddWithValue("@Username", seller.Username);
                int userId = (int)userIdCommand.ExecuteScalar();

                string sellerQuery = "INSERT INTO Sellers (UserId, BusinessName, BusinessAddress, ContactNumber, Website) VALUES (@UserId, @BusinessName, @BusinessAddress, @ContactNumber, @Website)";
                SqlCommand sellerCommand = new SqlCommand(sellerQuery, connection);
                sellerCommand.Parameters.AddWithValue("@UserId", userId);
                sellerCommand.Parameters.AddWithValue("@BusinessName", seller.BusinessName);
                sellerCommand.Parameters.AddWithValue("@BusinessAddress", seller.BusinessAddress);
                sellerCommand.Parameters.AddWithValue("@ContactNumber", seller.ContactNumber);
                sellerCommand.Parameters.AddWithValue("@Website", seller.Website);
                sellerCommand.ExecuteNonQuery();
            }
        }

        public bool UserNotExists(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND Role = @role";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@role", "Seller");

                connection.Open();
                int userCount = (int)command.ExecuteScalar();
                return userCount == 0;
            }
        }
    }
}
