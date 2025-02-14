using Microsoft.Data.SqlClient;
using WebProject.Models.Entities;
using WebProject.Models.Interfaces;
using WebProject.Models.Entities;

namespace WebProject.Models.Repositories
{
    public class TransactionRepository : ITransactionInterface
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PetPalace;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public bool BuyPet(TransactionsEntity transactions)
        {
            // SQL query to insert a new transaction
            string query = "INSERT INTO Transactions (SellerId, BuyerId, PetType, PetId, PhoneNumber, Address, PaymentAmount, TransactionDate) VALUES (@SELLID, @BUYERID, @PETTYPE, @PETID, @PHONENUMBER, @ADDRESS, @PAYMENTAMOUNT, @TRANSACTIONDATE)";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        // Adding parameters to the SQL command
                        command.Parameters.Add(new SqlParameter("@SELLID", transactions.SellerId));
                        command.Parameters.Add(new SqlParameter("@BUYERID", transactions.BuyerId));
                        command.Parameters.Add(new SqlParameter("@PETTYPE", transactions.PetType));
                        command.Parameters.Add(new SqlParameter("@PETID", transactions.PetId));
                        command.Parameters.Add(new SqlParameter("@PHONENUMBER", transactions.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@ADDRESS", transactions.Address));
                        command.Parameters.Add(new SqlParameter("@PAYMENTAMOUNT", transactions.PaymentAmount));
                        command.Parameters.Add(new SqlParameter("@TRANSACTIONDATE", transactions.TransactionDate));

                        // Execute the command
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Error Occurred: " + ex.Message);
                return false;
            }
            return true;
        }
        public void AddToCart(int petId,string Section)
        {
            AllPetsEntity petsEntity = new AllPetsEntity();
            if(Section == "Cat")
            {
                CatEntity cat = new CatEntity();
                cat.CatId = petId;
                petsEntity.Cats.Add(cat);
            }
            else if (Section == "Dog")
            {
                DogEntity dog = new DogEntity();
                dog.DogId = petId;
                petsEntity.Dogs.Add(dog);
            }
        }
    }
}
