using WebProject.Models.Entities;

namespace WebProject.Models.Interfaces
{
    public interface ITransactionInterface
    {
        public bool BuyPet(TransactionsEntity transactions);
    }
}
