using Domain;

namespace Application.Interfaces
{
    public interface IAnimalRepository
    {
        List<Animal> GetAll();
        Animal? GetById(string id);
        void Add(Animal animal);
        void Update(Animal animal);
        int Count { get; }
        int MaxCapacity { get; }
    }
}
