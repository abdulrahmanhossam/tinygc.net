namespace TinyGC;

/// <summary>
/// Heap manages memory for our GC.
/// 
/// HOW IT WORKS (simple version):
/// 1. We allocate a large byte array to act as our heap
/// 2. We track used vs free portions with an index
/// 3. When an object is freed, we mark it as free
/// 4. During "compact", we move objects to fill gaps
/// 
/// This is a simplified version that still teaches the concepts!
/// Real GCs use OS VirtualAlloc - but our approach is easier to understand.
/// </summary>
public class Heap
{
    /// <summary>
    /// The heap memory (a big byte array).
    /// We use a managed array instead of OS memory for simplicity.
    /// In real GC, you'd use Marshal.AllocHGlobal / VirtualAlloc.
    /// </summary>
    private byte[] _memory;

    /// <summary>
    /// Total size of heap in bytes.
    /// </summary>
    private const int HEAP_SIZE = 1024 * 1024 * 10; // 10MB

    /// <summary>
    /// Current allocation position (bump pointer).
    /// </summary>
    private int _allocPosition;

    /// <summary>
    /// Total objects allocated
    /// </summary>
    private int _totalObjects;

    /// <summary>
    /// Total objects freed
    /// </summary>
    private int _freedObjects;

    /// <summary>
    /// All objects currently allocated.
    /// We track this so the GC knows what's in the heap.
    /// </summary>
    private List<GCObject> _objects = new List<GCObject>();

    /// <summary>
    /// Initialize the heap.
    /// </summary>
    public Heap()
    {
        _memory = new byte[HEAP_SIZE];
        _allocPosition = 0;
        _totalObjects = 0;
        _freedObjects = 0;
        Console.WriteLine($"[Heap] Initialized: {HEAP_SIZE / 1024}KB heap");
    }

    /// <summary>
    /// Allocate memory for an object.
    /// Returns the offset where object is stored, or -1 if out of memory.
    /// </summary>
    public int Allocate(int size)
    {
        // Align to 8 bytes
        size = (size + 7) & ~7;

        // Check if we have space
        if (_allocPosition + size > HEAP_SIZE)
        {
            Console.WriteLine("[Heap] Out of memory!");
            return -1;
        }

        // The "address" is just the offset in our array
        int address = _allocPosition;

        // Move the bump pointer
        _allocPosition += size;

        Console.WriteLine($"[Heap] Allocated {size} bytes at offset 0x{address:x}");
        return address;
    }

    /// <summary>
    /// Add an object to our tracking list.
    /// </summary>
    public void AddObject(GCObject obj)
    {
        _totalObjects += 1;
        _objects.Add(obj);
    }

    /// <summary>
    /// Remove an object from tracking.
    /// </summary>
    public void RemoveObject(GCObject obj)
    {
        _freedObjects += 1;
        _objects.Remove(obj);
    }

    /// <summary>
    /// Get all allocated objects.
    /// </summary>
    public List<GCObject> GetObjects() => _objects;

    /// <summary>
    /// Get allocation position (end of used memory).
    /// </summary>
    public int GetAllocPosition() => _allocPosition;

    /// <summary>
    /// Get heap start address.
    /// </summary>
    public int GetStart() => 0;

    /// <summary>
    /// Get heap end address.
    /// </summary>
    public int GetEnd() => HEAP_SIZE;

    /// <summary>
    /// Free an object's memory.
    /// </summary>
    public void Free(GCObject obj)
    {
        Console.WriteLine($"[Heap] Freed object at offset 0x{obj.Address:x}");
    }

    /// <summary>
    /// Reset the heap (simple compaction - just reset position).
    /// Real compaction would move objects, but that's complex!
    /// </summary>
    public void Compact()
    {
        Console.WriteLine("[Heap] Compacting...");

        // For now, just log - real compaction requires updating all references!
        // Students can implement this later as an exercise.
    }

    /// <summary>
    /// Print heap statistics.
    /// </summary>
    public void PrintStats()
    {
        Console.WriteLine($"[Heap] Stats: {_allocPosition}/{HEAP_SIZE} bytes used, {_objects.Count} objects");
        Console.WriteLine($"Total objects allocated: {_totalObjects}, Total objects freed: {_freedObjects}, Current objects in memory: {_objects.Count}");
    }
}