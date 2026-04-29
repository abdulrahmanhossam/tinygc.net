using System;

namespace TinyGC;

/// <summary>
/// Allocator is the main API for allocating objects in our GC.
/// 
/// This wraps the GC to provide a clean interface.
/// </summary>
public class Allocator
{
    private static Allocator? _instance;

    public static Allocator Instance => _instance ??= new Allocator();

    private readonly GC _gc;

    private Allocator()
    {
        _gc = GC.Instance;
    }

    /// <summary>
    /// Allocate a new object.
    /// </summary>
    public T New<T>() where T : GCObject, new()
    {
        return (T)_gc.New<T>();
    }

    /// <summary>
    /// Run garbage collection.
    /// </summary>
    public void Collect()
    {
        _gc.Collect();
    }

    /// <summary>
    /// Print statistics.
    /// </summary>
    public void PrintStats()
    {
        _gc.PrintStats();
    }
}