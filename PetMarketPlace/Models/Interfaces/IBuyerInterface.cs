using WebProject.Models.Entities;

namespace WebProject.Models.Interfaces
{
    public interface IBuyerInterface
    {
        BuyerEntity Login(string username, string password);
        void Signup(BuyerEntity buyer);
        bool UserExists(string username);
        public bool ValidatePhoneNumber(string phoneNumber);
    }
}
