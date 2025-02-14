using WebProject.Models.Entities;
namespace WebProject.Models.Interfaces
{
    public interface ICatInterface
    {
        public void AddCat(CatEntity cat, int SellerId);
        public bool UpdateCat(CatEntity cat);
        public bool DeleteCat(int catId);
    }
}
