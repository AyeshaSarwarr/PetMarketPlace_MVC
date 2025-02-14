using WebProject.Models.Entities;

namespace WebProject.Models.Interfaces
{
    public interface IDogInterface
    {
        public void AddDog(DogEntity dog,int SellerId);
        public bool UpdateDog(DogEntity dog);
        public bool DeleteDog(int catId);
    }
}
