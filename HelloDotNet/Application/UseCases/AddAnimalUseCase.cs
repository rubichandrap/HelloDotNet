using Domain;
using Application.Interfaces;

namespace Application.UseCases
{
    public class AddAnimalUseCase
    {
        private readonly IAnimalRepository _repo;
        public AddAnimalUseCase(IAnimalRepository repo)
        {
            _repo = repo;
        }
        public bool Execute(Animal animal)
        {
            if (_repo.Count >= _repo.MaxCapacity) return false;
            _repo.Add(animal);
            return true;
        }
    }
}
