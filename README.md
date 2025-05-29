# HelloDotNet

HelloDotNet is a simple console application designed as an exercise to explore and become familiar with the .NET ecosystem and C# syntax. This project simulates a basic pet management system, allowing users to manage information about animals (cats and dogs) in memory.

## Motivation

This project was created as part of a personal learning journey into .NET and C#, in preparation for a new role that requires proficiency in these technologies. The goal is to gain hands-on experience with C# syntax, .NET project structure, and common programming patterns.

**Note:**  
This project is also part of a module assessment for the Microsoft Learn training:  
[Challenge Project - Develop branching and looping structures in C#](https://learn.microsoft.com/en-us/training/modules/challenge-project-develop-branching-looping-structures-c-sharp/1-introduction)


## Features

- List all current pet information
- Add a new animal (cat or dog) with details such as age, appearance, personality, and nickname
- Edit existing animal information
- Validate and ensure completeness of animal data (ages, descriptions, nicknames, personalities)
- In-memory data storage with a configurable maximum capacity

## Project Structure

- `Program.cs`: Main entry point and user interface logic (console-based menu)
- `Infrastructure/Repositories/InMemoryAnimalRepository.cs`: In-memory implementation of the animal repository
- `Domain/`: Contains domain models (e.g., `Animal`)
- `Application/UseCases/`: Contains use case logic for adding, editing, and listing animals

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

### Running the Application

1. Clone the repository:
```
   git clone https://github.com/rubichandrap/HelloDotNet.git
   cd HelloDotNet
```

2. Build and run the project:
```
   dotnet run --project HelloDotNet
```

3. Follow the on-screen menu to interact with the application.

## Usage

Upon running, the application presents a menu with options to:

- List all animals
- Add a new animal
- Ensure animal ages and physical descriptions are complete
- Ensure animal nicknames and personality descriptions are complete
- Edit an animal's information

Example menu:
```
Welcome to the Contoso PetFriends app. Your main menu options are:
 1. List all of our current pet information
 2. Add a new animal friend
 3. Ensure animal ages and physical descriptions are complete
 4. Ensure animal nicknames and personality descriptions are complete
 5. Edit an animal
 6. Display all cats with a specified characteristic
 7. Display all dogs with a specified characteristic
Enter your selection number (or type Exit to exit the program)
```

## Customization

- The maximum number of animals is set in `InMemoryAnimalRepository` (default: 10).
- Initial sample animals are populated at startup for demonstration.

## Learning Focus

- C# syntax and object-oriented programming
- .NET project structure and conventions
- Repository and use case patterns
- Console application development

## License

This project is for educational purposes.

---

**Summary of content:**  
- Project overview, motivation, and features  
- Structure and usage instructions  
- Learning objectives and customization notes  
- Example menu for clarity  
- License and encouragement

Let me know if you want to add more technical details or setup instructions.
