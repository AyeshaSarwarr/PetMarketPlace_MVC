using Microsoft.Data.SqlClient;
using WebProject.Models.Entities;
using WebProject.Models.Interfaces;

namespace WebProject.Models.Repositories
{
    public class AllPetsRepository : IAllPetsInterface
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PetPalace;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public List<CatEntity> RetrieveCats()
        {
            var cats = new List<CatEntity>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Cat";
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader()) 
                    {
                        while (reader.Read()) 
                        {
                            var cat = new CatEntity
                            {
                                CatId = reader.GetInt32(reader.GetOrdinal("CatId")),
                                CatType = reader["CatType"].ToString(),
                                CatAge = reader.GetInt32(reader.GetOrdinal("CatAge")),
                                CatColor = reader["CatColor"].ToString(),
                                CatPrice = reader.GetDecimal(reader.GetOrdinal("CatPrice")),
                                CatPicturePath = reader["CatPicturePath"].ToString()
                            };
                            cats.Add(cat); 
                        }
                    }
                }
            }

            return cats;
        
        }
        public List<DogEntity> RetrieveDogs()
        {
            List<DogEntity> dogs = new List<DogEntity>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Dog";
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dog = new DogEntity
                            {
                                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                                DogBreed = reader["DogBreed"].ToString(),
                                DogAge = reader.GetInt32(reader.GetOrdinal("DogAge")),
                                DogColor = reader["DogColor"].ToString(),
                                DogPrice = reader.GetDecimal(reader.GetOrdinal("DogPrice")),
                                DogPicturePath = reader["DogPicturePath"].ToString()
                            };
                            dogs.Add(dog);
                        }
                    }
                }
            }
            return dogs;
        }
        public List<DogEntity> SearchDogsUsingId(int SellerId)
        {
            List<DogEntity> dogs = new List<DogEntity>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Dog Where SellerId = @SELLID";
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@SELLID", SellerId));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dog = new DogEntity
                            {
                                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                                DogBreed = reader["DogBreed"].ToString(),
                                DogAge = reader.GetInt32(reader.GetOrdinal("DogAge")),
                                DogColor = reader["DogColor"].ToString(),
                                DogPrice = reader.GetDecimal(reader.GetOrdinal("DogPrice")),
                                DogPicturePath = reader["DogPicturePath"].ToString()
                            };
                            dogs.Add(dog);
                        }
                    }
                }
            }
            return dogs;
        }
        public List<CatEntity> SearchCatsUsingId(int SellerId)
        {
            List<CatEntity> cats = new List<CatEntity>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Cat Where SellerId = @SELLID";
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@SELLID", SellerId));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cat = new CatEntity
                            {
                                CatId = reader.GetInt32(reader.GetOrdinal("CatId")),
                                CatType = reader["CatType"].ToString(),
                                CatAge = reader.GetInt32(reader.GetOrdinal("CatAge")),
                                CatColor = reader["CatColor"].ToString(),
                                CatPrice = reader.GetDecimal(reader.GetOrdinal("CatPrice")),
                                CatPicturePath = reader["CatPicturePath"].ToString()
                            };
                            cats.Add(cat);
                        }
                    }
                }
            }
            return cats;
        }
        public CatEntity GetCatById(int CatId)
        {
            CatEntity Cat = new CatEntity();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Cat Where CatId = @CATID";
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@CATID", CatId));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cat.CatId = CatId;
                            Cat.CatType = reader["CatType"].ToString();
                            Cat.CatAge = reader.GetInt32(reader.GetOrdinal("CatAge"));
                            Cat.CatColor = reader["CatColor"].ToString();
                            Cat.CatPrice = reader.GetDecimal(reader.GetOrdinal("CatPrice"));
                            Cat.CatPicturePath = reader["CatPicturePath"].ToString();
                            
                        }
                    }
                }

            }
            return Cat;
        }
        public DogEntity GetDogById(int DogId)
        {
            DogEntity dog = new DogEntity();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Dog Where DogId = @DOGID";
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@DOGID", DogId));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dog.DogId = DogId;
                            dog.DogBreed = reader["DogBreed"].ToString();
                                dog.DogAge = reader.GetInt32(reader.GetOrdinal("DogAge"));
                                dog.DogColor = reader["DogColor"].ToString();
                                dog.DogPrice = reader.GetDecimal(reader.GetOrdinal("DogPrice"));
                                dog.DogPicturePath = reader["DogPicturePath"].ToString();
                        }
                    }
                }
            }
            return dog;
        }
    }
}
