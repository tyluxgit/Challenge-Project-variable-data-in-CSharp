using System.Text;
Console.OutputEncoding = Encoding.UTF8;


static void DisplaySearchAnimation(string dogName)
{
    string[] searchingIcons = { "|", "/", "-", "\\" };
    Console.Write($"\rSearching for {dogName}...");

    for (int j = 5; j > 0; j--)
    {
        foreach (string icon in searchingIcons)
        {
            Console.Write($"\rSearching {dogName}... {icon} ({j}s) ");
            Thread.Sleep(150);
        }
    }

    Console.Write("\r" + new string(' ', Console.BufferWidth) + "\r");
}


// ourAnimals array will store the following: 
string animalSpecies = "";
string animalID = "";
string animalAge = "";
string animalPhysicalDescription = "";
string animalPersonalityDescription = "";
string animalNickname = "";
string suggestedDonation = "";

// variables that support data entry
int maxPets = 8;
string? readResult;
string menuSelection = "";
decimal decimalDonation = 0.00m;

// array used to store runtime data
string[,] ourAnimals = new string[maxPets, 7];

// sample data ourAnimals array entries
for (int i = 0; i < maxPets; i++)
{
    switch (i)
    {
        case 0:
            animalSpecies = "dog";
            animalID = "d1";
            animalAge = "2";
            animalPhysicalDescription = "medium sized cream colored female golden retriever weighing about 45 pounds. housebroken.";
            animalPersonalityDescription = "loves to have her belly rubbed and likes to chase her tail. gives lots of kisses.";
            animalNickname = "lola";
            suggestedDonation = "85.00";
            break;

        case 1:
            animalSpecies = "dog";
            animalID = "d2";
            animalAge = "9";
            animalPhysicalDescription = "large reddish-brown male golden retriever weighing about 85 pounds. housebroken.";
            animalPersonalityDescription = "loves to have his ears rubbed when he greets you at the door, or at any time! loves to lean-in and give doggy hugs.";
            animalNickname = "gus";
            suggestedDonation = "49.99";
            break;

        case 2:
            animalSpecies = "cat";
            animalID = "c3";
            animalAge = "1";
            animalPhysicalDescription = "small white female weighing about 8 pounds. litter box trained.";
            animalPersonalityDescription = "friendly";
            animalNickname = "snow";
            suggestedDonation = "40.00";
            break;

        case 3:
            animalSpecies = "cat";
            animalID = "c4";
            animalAge = "";
            animalPhysicalDescription = "";
            animalPersonalityDescription = "";
            animalNickname = "lion";
            suggestedDonation = "";

            break;

        default:
            animalSpecies = "";
            animalID = "";
            animalAge = "";
            animalPhysicalDescription = "";
            animalPersonalityDescription = "";
            animalNickname = "";
            suggestedDonation = "";
            break;

    }

    ourAnimals[i, 0] = "ID #: " + animalID;
    ourAnimals[i, 1] = "Species: " + animalSpecies;
    ourAnimals[i, 2] = "Age: " + animalAge;
    ourAnimals[i, 3] = "Nickname: " + animalNickname;
    ourAnimals[i, 4] = "Physical description: " + animalPhysicalDescription;
    ourAnimals[i, 5] = "Personality: " + animalPersonalityDescription;

    if (!decimal.TryParse(suggestedDonation, out decimalDonation))
    {
        decimalDonation = 45.00m; // if suggestedDonation NOT a number, default to 45.00
    }
    ourAnimals[i, 6] = $"Suggested Donation: {decimalDonation:C2}";
}

// top-level menu options
do
{
    // NOTE: the Console.Clear method is throwing an exception in debug sessions
    Console.Clear();

    Console.WriteLine("Welcome to the Contoso PetFriends app. Your main menu options are:");
    Console.WriteLine(" 1. List all of our current pet information");
    Console.WriteLine(" 2. Display all dogs with a specified characteristic");
    Console.WriteLine();
    Console.WriteLine("Enter your selection number (or type Exit to exit the program)");

    readResult = Console.ReadLine();
    if (readResult != null)
    {
        menuSelection = readResult.ToLower();
    }

    // switch-case to process the selected menu option
    switch (menuSelection)
    {
        case "1":
            // list all pet info
            for (int i = 0; i < maxPets; i++)
            {
                if (ourAnimals[i, 0] != "ID #: ")
                {
                    Console.WriteLine();
                    for (int j = 0; j < 7; j++)
                    {
                        Console.WriteLine(ourAnimals[i, j].ToString());
                    }
                }
            }

            Console.WriteLine("\r\nPress the Enter key to continue");
            readResult = Console.ReadLine();

            break;

        case "2":
            Console.WriteLine("\nEnter dog characteristics to search for separated by commas:");
            readResult = Console.ReadLine();
            string[] searchTerms = readResult.Split(',')
                                             .Select(t => t.Trim().ToLower())
                                             .Where(t => !string.IsNullOrWhiteSpace(t))
                                             .ToArray();

            if (searchTerms.Length == 0)
            {
                Console.WriteLine("⚠ No valid characteristics entered. Please try again.");
                break;
            }

            bool foundDog = false;
            HashSet<string> matchedTerms = new HashSet<string>(); // Stocke les termes qui ont eu une correspondance
            HashSet<string> unmatchedTerms = new HashSet<string>(searchTerms); // Commence avec tous les termes

            for (int i = 0; i < maxPets; i++)
            {
                if (ourAnimals[i, 1] == null || !ourAnimals[i, 1].ToLower().Contains("dog"))
                    continue; // Ignore si ce n'est pas un chien ou s'il y a des valeurs nulles

                string dogName = ourAnimals[i, 3] ?? "Unknown";
                string dogDescription = $"{ourAnimals[i, 4] ?? ""}\n{ourAnimals[i, 5] ?? ""}";

                // Animation de recherche
                DisplaySearchAnimation(dogName);

                // Recherche des termes correspondants
                List<string> foundKeywords = searchTerms.Where(term => dogDescription.ToLower().Contains(term)).ToList();

                if (foundKeywords.Count > 0)
                {
                    Console.WriteLine($"\n✅ Our dog {dogName} matches your search!");
                    Console.WriteLine($"📌 {dogName} ({ourAnimals[i, 0]})\n{dogDescription}");
                    Console.WriteLine($"🔍 Matched keywords: {string.Join(", ", foundKeywords)}");

                    foundDog = true;
                    matchedTerms.UnionWith(foundKeywords); // Ajouter les termes trouvés
                }
            }

            // Identifier les termes qui n'ont pas eu de correspondance
            unmatchedTerms.ExceptWith(matchedTerms);

            if (!foundDog)
            {
                Console.WriteLine("\n❌ No matching dogs found.");
            }

            if (unmatchedTerms.Count > 0)
            {
                Console.WriteLine("\n⚠ No matches found for these keywords: " + string.Join(", ", unmatchedTerms));
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            break;

        default:
            break;
    }

} while (menuSelection != "exit");
