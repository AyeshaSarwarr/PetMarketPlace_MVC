using WebProject.Models.Entities;

namespace WebProject.Models.Interfaces
{
    public interface ISellerInterface
    {
        (SellerEntity,int) Login(string username, string password);
        void Signup(SellerEntity seller);
        bool UserNotExists(string username);
    }
}
