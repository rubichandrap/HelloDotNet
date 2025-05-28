using Domain;
using Application.Interfaces;

namespace Application.UseCases
{
    public class EditAnimalUseCase
    {
        private readonly IAnimalRepository _repo;
        public EditAnimalUseCase(IAnimalRepository repo)
        {
            _repo = repo;
        }
        public bool Execute(Animal animal)
        {
            var existing = _repo.GetById(animal.Id);
            if (existing == null) return false;
            _repo.Update(animal);
            return true;
        }
    }
}
