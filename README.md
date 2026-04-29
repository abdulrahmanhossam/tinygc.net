# TinyGC.NET - Educational Garbage Collector

A simple, educational garbage collector written in C# for .NET. Perfect for students learning how garbage collection works!

## What is Garbage Collection?

**Garbage Collection (GC)** is automatic memory management. When you create objects in code, they're stored in memory. When you're done with them, the GC automatically frees that memory so other programs can use it.

## How to Run

```bash
dotnet run
```

## How It Works

Our GC uses **Mark-Sweep** algorithm with 3 phases:

### 1. MARK PHASE
- Start from "roots" (variables in your code)
- Mark every object we can reach as **alive**

### 2. SWEEP PHASE
- Walk through all objects in the heap
- If object is NOT marked → it's garbage! Free it.
- If object IS marked → keep it

### 3. COMPACT PHASE
- Move objects together to reduce gaps (simplified in this version)

## Project Structure

| File | Purpose |
|------|---------|
| `GCObject.cs` | Base class for all GC objects |
| `Heap.cs` | Memory manager |
| `GC.cs` | Main GC with Mark/Sweep |
| `GcRootHandle.cs` | Keeps objects alive |
| `MyData.cs` | Example test class |
| `Program.cs` | Demo program |

## Example Usage

```csharp
// Allocate an object
var obj = GC.Instance.New<MyData>();
obj.Value = 42;
obj.Name = "Test";

// Run garbage collection
GC.Instance.Collect();

// Print statistics
GC.Instance.PrintStats();
```

## Key Concepts Learned

1. **Bump Pointer Allocation** - Fast memory allocation (just increment an index)
2. **Free List** - Track freed memory for reuse
3. **Mark-Sweep** - Foundation of GC algorithms
4. **Root Handles** - References that keep objects alive
5. **Object Headers** - Metadata about each object

## For Students

### Try these exercises:
1. Add reference tracing - follow references within objects
2. Implement compaction - move objects together
3. Add generational GC - young/old generations
4. Use Marshal.AllocHGlobal - real OS memory

## References

- [Writing a .NET GC in C# - Kevin Gosse](https://minidump.net)
- [Mark-Sweep GC Tutorial](https://dmitrysoshnikov.com/compilers/writing-a-mark-sweep-garbage-collector/)

## License

GNU General Public License v3.0 - See LICENSE file

---

**Happy Learning!** 🚀