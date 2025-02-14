using Microsoft.Data.SqlClient;
using WebProject.Models.Entities;
using WebProject.Models.Interfaces;

namespace WebProject.Models.Repositories
{
    public class CatRepository : ICatInterface
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PetPalace;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public void AddCat(CatEntity cat, int SellerId)
        {
            string query = "INSERT INTO Cat (CatType, CatAge, CatColor, CatPrice, CatPicturePath, SellerId) VALUES (@TYPE, @AGE, @COLOR, @PRICE, @PICPATH, @SELLERID)";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.Add(new SqlParameter("@TYPE", cat.CatType));
                        command.Parameters.Add(new SqlParameter("@AGE", cat.CatAge));
                        command.Parameters.Add(new SqlParameter("@COLOR", cat.CatColor));
                        command.Parameters.Add(new SqlParameter("@PRICE", cat.CatPrice));
                        command.Parameters.Add(new SqlParameter("@PICPATH", cat.CatPicturePath));
                        command.Parameters.Add(new SqlParameter("@SELLERID", SellerId));
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Error Occurred: " + ex.Message);
            }
        }

        public bool UpdateCat(CatEntity cat)
        {
            bool updateCat = false;
            if (CatExists(cat.CatId))
            {
                string query = "UPDATE Cat SET CatType = @TYPE, CatAge = @AGE, CatColor = @COLOR, CatPrice = @PRICE, CatPicturePath = @PICPATH WHERE CatId = @ID";
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            command.Parameters.Add(new SqlParameter("@TYPE", cat.CatType));
                            command.Parameters.Add(new SqlParameter("@AGE", cat.CatAge));
                            command.Parameters.Add(new SqlParameter("@COLOR", cat.CatColor));
                            command.Parameters.Add(new SqlParameter("@PRICE", cat.CatPrice));
                            command.Parameters.Add(new SqlParameter("@PICPATH", cat.CatPicturePath));
                            command.Parameters.Add(new SqlParameter("@ID", cat.CatId));

                            command.ExecuteNonQuery();
                            updateCat = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An Error Occurred: " + ex.Message);
                }
            }
            return updateCat;
        }

        public bool DeleteCat(int catId)
        {
            bool deleteCat = false;
            if (CatExists(catId))
            {
                string query = "DELETE FROM Cat WHERE CatId = @ID";
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            command.Parameters.Add(new SqlParameter("@ID", catId));
                            int rowsAffected = command.ExecuteNonQuery();
                            deleteCat = rowsAffected > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An Error Occurred: " + ex.Message);
                }
            }
            return deleteCat;
        }

        private bool CatExists(int catId)
        {
            bool catExists = false;
            string query = "SELECT COUNT(*) FROM Cat WHERE CatId = @ID";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.Add(new SqlParameter("@ID", catId));
                        int count = (int)command.ExecuteScalar();
                        catExists = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Error Occurred: " + ex.Message);
            }
            return catExists;
        }
    }
}
