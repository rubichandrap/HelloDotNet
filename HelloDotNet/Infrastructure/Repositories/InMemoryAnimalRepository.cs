using Domain;
using Application.Interfaces;

namespace Infrastructure.Repositories
{
    public class InMemoryAnimalRepository : IAnimalRepository
    {
        private readonly List<Animal> _animals = new();
        public int MaxCapacity { get; }
        public int Count => _animals.Count;
        public InMemoryAnimalRepository(int maxCapacity)
        {
            MaxCapacity = maxCapacity;
        }
        public List<Animal> GetAll() => new List<Animal>(_animals);
        public Animal? GetById(string id) => _animals.Find(a => a.Id == id);
        public void Add(Animal animal) => _animals.Add(animal);
        public void Update(Animal animal)
        {
            var idx = _animals.FindIndex(a => a.Id == animal.Id);
            if (idx >= 0) _animals[idx] = animal;
        }
    }
}
