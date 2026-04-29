using System;

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

        RunDemo();

        GC.Instance.Shutdown();
    }

    static void RunDemo()
    {
        Console.WriteLine("=== STEP 1: Allocate Objects ===\n");

        // Allocate some objects
        var obj1 = GC.Instance.New<MyData>();
        obj1.Value = 42;
        obj1.Name = "First Object";
        Console.WriteLine($"obj1: Value={obj1.Value}, Name={obj1.Name}");

        var obj2 = GC.Instance.New<MyData>();
        obj2.Value = 100;
        obj2.Name = "Second Object";
        Console.WriteLine($"obj2: Value={obj2.Value}, Name={obj2.Name}");

        Console.WriteLine("\n=== STEP 2: Statistics ===\n");
        GC.Instance.PrintStats();

        Console.WriteLine("\n=== STEP 3: Run GC ===\n");
        // All objects still have roots, so none are garbage!
        GC.Instance.Collect();
        
        Console.WriteLine("\n=== STEP 4: After GC ===\n");
        GC.Instance.PrintStats();

        Console.WriteLine("\n=== STEP 5: More Allocations ===\n");
        
        // Allocate more objects
        for (int i = 0; i < 3; i++)
        {
            var temp = GC.Instance.New<MyData>();
            temp.Value = i * 10;
            temp.Name = $"Object {i}";
            Console.WriteLine($"Allocated: {temp.Name}");
        }

        GC.Instance.Collect();
        GC.Instance.PrintStats();

        Console.WriteLine("\n=== Demo Complete! ===");
    }
}