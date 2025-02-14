using WebProject.Models.Entities;

namespace WebProject.Models.Interfaces
{
    public interface IAllPetsInterface
    {
        public List<CatEntity> RetrieveCats();
        public List<DogEntity> RetrieveDogs();
        public List<CatEntity> SearchCatsUsingId(int SellerId);
        public List<DogEntity> SearchDogsUsingId(int SellerId);
        public CatEntity GetCatById(int CatId);
        public DogEntity GetDogById(int DogId);
    }
}
