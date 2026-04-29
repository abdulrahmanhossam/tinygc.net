namespace TinyGC;

/// <summary>
/// Program.cs - Demo showing how to use our GC
/// 
/// Run with: dotnet run
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("  TinyGC.NET - Educational GC Demo");
        Console.WriteLine("========================================\n");

        ShowMenu();

        Console.WriteLine("\n=== Demo Complete! ===");
    }

    static void ShowMenu()
    {
        Console.WriteLine("\n=== TinyGC.NET Menu ===");
        Console.WriteLine("1. Allocate object");
        Console.WriteLine("2. Run GC");
        Console.WriteLine("3. View stats");
        Console.WriteLine("4. Exit");
        Console.Write("Enter choice: ");
        bool validInput = Int32.TryParse(Console.ReadLine(), out int userInput);
        if (validInput)
        {
            switch (userInput)
            {
                case 1:
                    GC.Instance.AllocateObject();
                    break;
                case 2:
                    // All objects still have roots, so none are garbage!
                    GC.Instance.Collect();
                    break;
                case 3:
                    GC.Instance.PrintStats();
                    break;
                case 4:
                    GC.Instance.Shutdown();
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a number.");
        }
        ShowMenu();
    }
}