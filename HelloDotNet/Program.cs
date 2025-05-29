using Domain;
using Application.Interfaces;
using Application.UseCases;
using Infrastructure.Repositories;

class Program
{
    private readonly IAnimalRepository animalRepository;
    private readonly AddAnimalUseCase addAnimalUseCase;
    private readonly EditAnimalUseCase editAnimalUseCase;
    private readonly ListAnimalsUseCase listAnimalsUseCase;
    private string choice = "";
    private string? readResult;

    public Program()
    {
        animalRepository = new InMemoryAnimalRepository(10);
        addAnimalUseCase = new AddAnimalUseCase(animalRepository);
        editAnimalUseCase = new EditAnimalUseCase(animalRepository);
        listAnimalsUseCase = new ListAnimalsUseCase(animalRepository);
    }

    static void Main(string[] args)
    {
        Program program = new Program();
        program.PopulateAnimals();
        program.Menu();
    }

    private void Menu()
    {
        do
        {
            Console.WriteLine("Welcome to the Contoso PetFriends app. Your main menu options are:");
            Console.WriteLine(" 1. List all of our current pet information");
            Console.WriteLine(" 2. Add a new animal friend");
            Console.WriteLine(" 3. Ensure animal ages and physical descriptions are complete");
            Console.WriteLine(" 4. Ensure animal nicknames and personality descriptions are complete");
            Console.WriteLine(" 5. Edit an animal");
            Console.WriteLine(" 6. Display all cats with a specified characteristic");
            Console.WriteLine(" 7. Display all dogs with a specified characteristic");
            Console.WriteLine();
            Console.WriteLine("Enter your selection number (or type Exit to exit the program)");

            readResult = Console.ReadLine();

            if (readResult != null) choice = readResult.ToLower();

            switch (choice)
            {
                case "1":
                    ListAnimals();
                    break;
                case "2":
                    AddAnimal();
                    break;
                case "3":
                    EnsureAgesAndDescriptions();
                    break;
                case "4":
                    EnsureNicknamesAndPersonalities();
                    break;
                case "5":
                    EditAnimal();
                    break;
                case "6":
                    DisplayAnimalsByCharacteristic("cat");
                    break;
                case "7":
                    DisplayAnimalsByCharacteristic("dog");
                    break;
                case "exit":
                    Console.WriteLine("Exiting the program.");
                    return;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        } while (choice != "exit");
    }

    private void ListAnimals()
    {
        Console.WriteLine("Listing all animals added\n");
        var animals = listAnimalsUseCase.Execute();
        for (int i = 0; i < animals.Count; i++)
        {
            var animal = animals[i];
            Console.WriteLine($"{i + 1}.\tID: {animal.Id}\n\tSpecies: {animal.Species}\n\tAge: {animal.Age}\n\tAppearance: {animal.Appearance}\n\tPersonality: {animal.Personality}\n\tNickname: {animal.Nickname}\n");
        }
    }

    private void AddAnimal()
    {
        if (animalRepository.Count >= animalRepository.MaxCapacity)
        {
            Console.WriteLine("Maximum animal capacity reached. Cannot add more animals.");
            return;
        }

        Console.WriteLine($"We currently have {animalRepository.Count} pets that need homes. We can manage {(animalRepository.MaxCapacity - animalRepository.Count)} more.");

        string shouldAddAnotherAnimal = "Y";
        bool isValidEntry = false;

        while (shouldAddAnotherAnimal == "Y" && animalRepository.Count < animalRepository.MaxCapacity)
        {
            Animal newAnimal = new Animal
            {
                Id = (animalRepository.Count + 1).ToString(),
                Species = "",
                Age = 0,
                Appearance = "",
                Personality = "",
                Nickname = ""
            };

            // Loop until a valid animal species is entered
            do
            {
                Console.WriteLine("\n\rEnter 'dog' or 'cat' to begin a new entry");

                readResult = Console.ReadLine();

                if (readResult == null) continue;

                string animalSpecies = readResult.ToLower();

                if (animalSpecies != "dog" && animalSpecies != "cat") continue;

                newAnimal.Species = animalSpecies;
                isValidEntry = true;
            } while (isValidEntry == false);

            // Prompt for age
            do
            {
                isValidEntry = false;

                Console.WriteLine("Enter the pet's age or enter ? if unknown");

                readResult = Console.ReadLine();

                if (readResult == null) continue;

                if (readResult == "?")
                {
                    newAnimal.Age = 0;
                    isValidEntry = true;
                    continue;
                }

                if (int.TryParse(readResult, out int age))
                {
                    newAnimal.Age = age;
                    isValidEntry = true;
                    continue;
                }

                Console.WriteLine("Invalid age. Please enter a valid number or '?' for unknown.");
            } while (isValidEntry == false);

            // get a description of the pet's physical appearance - animalPhysicalDescription can be blank.
            do
            {
                isValidEntry = false;

                Console.WriteLine("Enter a physical description of the pet (size, color, gender, weight, housebroken)");

                readResult = Console.ReadLine();

                if (readResult == null) continue;

                string animalPhysicalDescription = readResult;

                if (animalPhysicalDescription != "") newAnimal.Appearance = animalPhysicalDescription;

                isValidEntry = true;
            } while (isValidEntry == false);

            // get a description of the pet's personality - animalPersonalityDescription can be blank.
            do
            {
                isValidEntry = false;

                Console.WriteLine("Enter a description of the pet's personality (likes or dislikes, tricks, energy level)");

                readResult = Console.ReadLine();

                if (readResult == null) continue;

                string animalPersonalityDescription = readResult;

                if (animalPersonalityDescription != "") newAnimal.Personality = animalPersonalityDescription;

                isValidEntry = true;
            } while (isValidEntry == false);

            // get the pet's nickname. animalNickname can be blank.
            do
            {
                isValidEntry = false;

                Console.WriteLine("Enter a nickname for the pet");

                readResult = Console.ReadLine();

                if (readResult == null) continue;

                string animalNickname = readResult;

                if (animalNickname != "") newAnimal.Nickname = animalNickname;

                isValidEntry = true;
            } while (isValidEntry == false);

            if (!addAnimalUseCase.Execute(newAnimal))
            {
                Console.WriteLine("Failed to add animal (capacity reached or other error).");
                break;
            }

            // Prompt to add another animal if the maximum capacity has not been reached
            if (animalRepository.Count < animalRepository.MaxCapacity)
            {
                Console.WriteLine("Do you want to enter info for another pet (Y/n)");

                do
                {
                    readResult = Console.ReadLine();

                    if (readResult != null) shouldAddAnotherAnimal = readResult;
                } while (shouldAddAnotherAnimal != "Y" && shouldAddAnotherAnimal != "n");
            }
        }
    }

    private void EnsureAgesAndDescriptions()
    {
        var animals = listAnimalsUseCase.Execute();
        foreach (var animal in animals)
        {
            if (animal.Age == 0)
            {
                Console.WriteLine($"Animal ID {animal.Id} ({animal.Species}) has an unknown age.");
            }

            if (string.IsNullOrWhiteSpace(animal.Appearance))
            {
                Console.WriteLine($"Animal ID {animal.Id} ({animal.Species}) has no appearance description.");
            }
        }
    }

    private void EnsureNicknamesAndPersonalities()
    {
        var animals = listAnimalsUseCase.Execute();
        foreach (var animal in animals)
        {
            if (string.IsNullOrWhiteSpace(animal.Nickname))
            {
                Console.WriteLine($"Animal ID {animal.Id} ({animal.Species}) has no nickname.");
            }

            if (string.IsNullOrWhiteSpace(animal.Personality))
            {
                Console.WriteLine($"Animal ID {animal.Id} ({animal.Species}) has no personality description.");
            }
        }
    }

    private void EditAnimal()
    {
        bool isValidEntry = false;
        Animal? animalToEdit = null;

        var animals = listAnimalsUseCase.Execute();

        // Prompt for the ID of the animal to edit
        do
        {
            Console.WriteLine("Enter the ID of the animal you want to edit:");

            readResult = Console.ReadLine();

            if (readResult == null) continue;

            animalToEdit = animals.FirstOrDefault(a => a.Id == readResult);

            if (animalToEdit == null)
            {
                Console.WriteLine("Invalid ID. Please try again.");
                continue;
            }

            Console.WriteLine($"Editing Animal ID {animalToEdit.Id} ({animalToEdit.Species})");

            isValidEntry = true;
        } while (isValidEntry == false || animalToEdit == null);

        // Prompt for new species
        do
        {
            isValidEntry = false;

            Console.WriteLine("Enter new species either 'cat' or 'dog' (or press Enter to keep current):");

            readResult = Console.ReadLine();

            if (readResult == null || readResult == "")
            {
                Console.WriteLine("No species entered. Keeping current species.");
                isValidEntry = true;
                continue;
            }

            if (readResult.ToLower() != "dog" && readResult.ToLower() != "cat")
            {
                Console.WriteLine("Invalid species. Please enter 'dog' or 'cat'.");
                continue;
            }

            animalToEdit.Species = readResult;
            isValidEntry = true;
        } while (isValidEntry == false);

        // Prompt for new age
        do
        {
            isValidEntry = false;

            Console.WriteLine("Enter new age (or press Enter to keep current):");

            readResult = Console.ReadLine();

            if (readResult == null || readResult == "")
            {
                Console.WriteLine("No age entered. Keeping current age.");
                isValidEntry = true;
                continue;
            }

            if (!int.TryParse(readResult, out int newAge))
            {
                Console.WriteLine("Invalid age. Please enter a valid number.");
                continue;
            }

            animalToEdit.Age = newAge;
            isValidEntry = true;
        } while (isValidEntry == false);

        // Prompt for new appearance
        do
        {
            isValidEntry = false;

            Console.WriteLine("Enter new appearance description (or press Enter to keep current):");

            readResult = Console.ReadLine();

            if (readResult == null || readResult == "")
            {
                Console.WriteLine("No appearance entered. Keeping current appearance.");
                isValidEntry = true;
                continue;
            }

            animalToEdit.Appearance = readResult;
            isValidEntry = true;
        } while (isValidEntry == false);

        // Prompt for new personality
        do
        {
            isValidEntry = false;

            Console.WriteLine("Enter new personality description (or press Enter to keep current):");

            readResult = Console.ReadLine();

            if (readResult == null || readResult == "")
            {
                Console.WriteLine("No personality entered. Keeping current personality.");
                isValidEntry = true;
                continue;
            }

            animalToEdit.Personality = readResult;
            isValidEntry = true;
        } while (isValidEntry == false);

        // Prompt for new nickname
        do
        {
            isValidEntry = false;

            Console.WriteLine("Enter new nickname (or press Enter to keep current):");

            readResult = Console.ReadLine();

            if (readResult == null || readResult == "")
            {
                Console.WriteLine("No nickname entered. Keeping current nickname.");
                isValidEntry = true;
                continue;
            }

            animalToEdit.Nickname = readResult;
            isValidEntry = true;
        } while (isValidEntry == false);

        // Save changes using the use case
        if (!editAnimalUseCase.Execute(animalToEdit))
        {
            Console.WriteLine("Failed to update animal.");
        }
    }

    private void DisplayAnimalsByCharacteristic(string species)
    {
        Console.WriteLine($"Displaying all {species}s with a specified characteristic:");
        Console.WriteLine("Enter the characteristic to filter by (e.g., 'friendly', 'playful', etc.):");

        readResult = Console.ReadLine();

        if (readResult == null || readResult.Trim() == "")
        {
            Console.WriteLine("No characteristic entered. Returning to main menu.\n");
            return;
        }

        var animals = listAnimalsUseCase.Execute()
            .Where(a => a.Species.Equals(species, StringComparison.OrdinalIgnoreCase) &&
                        (a.Personality.Contains(readResult, StringComparison.OrdinalIgnoreCase) ||
                         a.Appearance.Contains(readResult, StringComparison.OrdinalIgnoreCase)));

        if (!animals.Any())
        {
            Console.WriteLine($"No {species}s found with the characteristic '{readResult}'.\n");
            return;
        }

        foreach (var animal in animals)
        {
            Console.WriteLine($"ID: {animal.Id}, Species: {animal.Species}, Age: {animal.Age}, Appearance: {animal.Appearance}, Personality: {animal.Personality}, Nickname: {animal.Nickname}");
        }
    }

    private void PopulateAnimals()
    {
        addAnimalUseCase.Execute(new Animal
        {
            Id = "1",
            Species = "Dog",
            Age = 5,
            Appearance = "Brown and White",
            Personality = "Friendly",
            Nickname = "Buddy"
        });

        addAnimalUseCase.Execute(new Animal
        {
            Id = "2",
            Species = "Cat",
            Age = 3,
            Appearance = "Black and White",
            Personality = "Independent",
            Nickname = "Whiskers"
        });

        addAnimalUseCase.Execute(new Animal
        {
            Id = "3",
            Species = "Dog",
            Age = 4,
            Appearance = "Gray",
            Personality = "Playful",
            Nickname = "Fluffy"
        });

        addAnimalUseCase.Execute(new Animal
        {
            Id = "4",
            Species = "Cat",
            Age = 2,
            Appearance = "Orange",
            Personality = "Angry",
            Nickname = "Meow"
        });
    }
}