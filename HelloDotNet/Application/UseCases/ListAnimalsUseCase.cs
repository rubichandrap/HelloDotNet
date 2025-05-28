using Domain;
using Application.Interfaces;

namespace Application.UseCases
{
    public class ListAnimalsUseCase
    {
        private readonly IAnimalRepository _repo;
        public ListAnimalsUseCase(IAnimalRepository repo)
        {
            _repo = repo;
        }
        public List<Animal> Execute()
        {
            return _repo.GetAll();
        }
    }
}
