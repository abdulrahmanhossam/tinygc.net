namespace TinyGC;

/// <summary>
/// GC is our garbage collector!
/// 
/// HOW IT WORKS (3 phases):
/// 
/// 1. MARK PHASE:
///    - Start from "roots" (variables in your code)
///    - Mark every object we can reach as "alive"
///    - Follow references to mark all reachable objects
/// 
/// 2. SWEEP PHASE:
///    - Walk through all objects in the heap
///    - If NOT marked -> it's garbage, free it!
///    - If marked -> keep it (unmark for next time)
/// 
/// 3. COMPACT PHASE:
///    - Move objects together to reduce fragmentation
///    - (Simplified in this version!)
/// 
/// This simulates how real .NET GC works!
/// </summary>
public class GC
{
    /// <summary>
    /// Singleton instance.
    /// </summary>
    private static GC? _instance;

    /// <summary>
    /// Get the singleton instance.
    /// </summary>
    public static GC Instance => _instance ??= new GC();

    /// 
    /// Instance Value
    /// 
    public static int InstanceValue = 1;

    /// <summary>
    /// Our heap.
    /// </summary>
    private readonly Heap _heap;

    /// <summary>
    /// Root handles - keep objects alive!
    /// </summary>
    private readonly List<GcRootHandle> _roots = new();

    /// <summary>
    /// Constructor.
    /// </summary>
    private GC()
    {
        _heap = new Heap();
    }

    /// <summary>
    /// Allocate a new object.
    /// Returns the allocated object!
    /// </summary>
    public TObj New<TObj>() where TObj : GCObject, new()
    {
        // Create the object
        TObj obj = new TObj();

        // Calculate size (header + fields)
        int size = 32; // minimum object size
        if (obj.Size > 0) size = obj.Size;

        // Allocate memory
        int addr = _heap.Allocate(size);
        if (addr < 0)
        {
            throw new OutOfMemoryException("GC Heap exhausted!");
        }

        // Set up the object
        obj.Size = size;
        obj.Address = addr;
        obj.IsMarked = false;

        // Add to heap tracking
        _heap.AddObject(obj);

        // Create a root handle (keeps object alive!)
        var handle = new GcRootHandle(obj);
        _roots.Add(handle);

        Console.WriteLine($"[GC] Allocated {typeof(TObj).Name} at offset 0x{addr:x}");

        return obj;
    }

    ///
    /// Allocate object
    /// 
    public void AllocateObject()
    {
        var obj1 = GC.Instance.New<MyData>();
        obj1.Value = GC.InstanceValue++;
        obj1.Name = "object";
        Console.WriteLine($"Allocated {obj1.Name} with value {obj1.Value}");
    }

    /// <summary>
    /// Free a root handle.
    /// </summary>
    public void FreeRoot(GcRootHandle handle)
    {
        _roots.Remove(handle);
    }

    /// <summary>
    /// Run garbage collection!
    /// </summary>
    public void Collect()
    {
        Console.WriteLine("\n[GC] === Starting Garbage Collection ===");

        // PHASE 1: MARK
        MarkPhase();

        // PHASE 2: SWEEP
        SweepPhase();

        // PHASE 3: COMPACT
        CompactPhase();

        Console.WriteLine("[GC] === GC Complete ===\n");
    }

    /// <summary>
    /// MARK PHASE: Find all reachable objects.
    /// </summary>
    private void MarkPhase()
    {
        Console.WriteLine("[GC] MARK PHASE");

        // Step 1: Unmark ALL objects
        foreach (var obj in _heap.GetObjects())
        {
            obj.IsMarked = false;
        }

        // Step 2: Mark objects reachable from roots
        int markedCount = 0;
        foreach (var root in _roots)
        {
            if (root.IsAllocated && root.Target != null)
            {
                root.Target.IsMarked = true;
                markedCount++;
                Console.WriteLine($"[GC]   Marked root object at 0x{root.Target.Address:x}");
            }
        }

        // Step 3: Trace references (simplified!)
        // In a real GC, we'd follow all references within marked objects
        // For now, we only mark direct roots

        Console.WriteLine($"[GC]   Marked {markedCount} objects");
    }

    /// <summary>
    /// SWEEP PHASE: Free garbage.
    /// </summary>
    private void SweepPhase()
    {
        Console.WriteLine("[GC] SWEEP PHASE");

        int freedCount = 0;
        int keptCount = 0;

        // Check each object
        var objects = _heap.GetObjects();
        var toRemove = new List<GCObject>();

        foreach (var obj in objects)
        {
            if (!obj.IsMarked)
            {
                // Not marked = GARBAGE!
                _heap.Free(obj);
                _heap.RemoveObject(obj);
                toRemove.Add(obj);
                freedCount++;
                Console.WriteLine($"[GC]   Freed object at 0x{obj.Address:x}");
            }
            else
            {
                // Keep it
                obj.IsMarked = false; // Reset for next GC
                keptCount++;
            }
        }

        // Clean up root handles for freed objects
        var rootsToRemove = new List<GcRootHandle>();
        foreach (var root in _roots)
        {
            if (root.Target != null && toRemove.Contains(root.Target))
            {
                rootsToRemove.Add(root);
            }
        }
        foreach (var r in rootsToRemove)
        {
            _roots.Remove(r);
        }

        Console.WriteLine($"[GC]   Swept: {keptCount} kept, {freedCount} freed");
    }

    /// <summary>
    /// COMPACT PHASE: Defragment heap.
    /// </summary>
    private void CompactPhase()
    {
        Console.WriteLine("[GC] COMPACT PHASE");

        _heap.Compact();
    }

    /// <summary>
    /// Get statistics.
    /// </summary>
    public void PrintStats()
    {
        Console.WriteLine($"\n[GC] Statistics:");
        Console.WriteLine($"[GC]   Active roots: {_roots.Count}");
        _heap.PrintStats();
    }

    /// <summary>
    /// Shutdown.
    /// </summary>
    public void Shutdown()
    {
        Console.WriteLine("[GC] Shutdown complete.");
    }
}