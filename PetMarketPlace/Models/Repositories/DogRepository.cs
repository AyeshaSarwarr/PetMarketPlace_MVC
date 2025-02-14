using Microsoft.Data.SqlClient;
using WebProject.Models.Entities;
using WebProject.Models.Interfaces;

namespace WebProject.Models.Repositories
{
    public class DogRepository : IDogInterface
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PetPalace;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public void AddDog(DogEntity dog, int SellerId)
        {
            string query = "INSERT INTO Dog (DogBreed, DogAge, DogColor, DogPrice, DogPicturePath, SellerId) VALUES (@BREED, @AGE, @COLOR, @PRICE, @PICPATH, @SELLID)";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.Add(new SqlParameter("@BREED", dog.DogBreed));
                        command.Parameters.Add(new SqlParameter("@AGE", dog.DogAge));
                        command.Parameters.Add(new SqlParameter("@COLOR", dog.DogColor));
                        command.Parameters.Add(new SqlParameter("@PRICE", dog.DogPrice));
                        command.Parameters.Add(new SqlParameter("@PICPATH", dog.DogPicturePath));
                        command.Parameters.Add(new SqlParameter("@SELLID", SellerId));
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Error Occurred: " + ex.Message);
            }
        }

        public bool UpdateDog(DogEntity dog)
        {
            bool updateDog = false;
            if (DogExists(dog.DogId))
            {
                string query = "UPDATE Dog SET DogBreed = @BREED, DogAge = @AGE, DogColor = @COLOR, DogPrice = @PRICE, DogPicturePath = @PICPATH WHERE DogId = @ID";
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            command.Parameters.Add(new SqlParameter("@BREED", dog.DogBreed));
                            command.Parameters.Add(new SqlParameter("@AGE", dog.DogAge));
                            command.Parameters.Add(new SqlParameter("@COLOR", dog.DogColor));
                            command.Parameters.Add(new SqlParameter("@PRICE", dog.DogPrice));
                            command.Parameters.Add(new SqlParameter("@PICPATH", dog.DogPicturePath));
                            command.Parameters.Add(new SqlParameter("@ID", dog.DogId));

                            command.ExecuteNonQuery();
                            updateDog = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An Error Occurred: " + ex.Message);
                }
            }
            return updateDog;
        }

        public bool DeleteDog(int dogId)
        {
            bool deleteDog = false;
            if (DogExists(dogId))
            {
                string query = "DELETE FROM Dog WHERE DogId = @ID";
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            command.Parameters.Add(new SqlParameter("@ID", dogId));
                            int rowsAffected = command.ExecuteNonQuery();
                            deleteDog = rowsAffected > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An Error Occurred: " + ex.Message);
                }
            }
            return deleteDog;
        }

        private bool DogExists(int dogId)
        {
            bool dogExists = false;
            string query = "SELECT COUNT(*) FROM Dog WHERE DogId = @ID";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.Add(new SqlParameter("@ID", dogId));
                        int count = (int)command.ExecuteScalar();
                        dogExists = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Error Occurred: " + ex.Message);
            }
            return dogExists;
        }
    }
}
