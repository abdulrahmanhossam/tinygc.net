namespace TinyGC;

/// <summary>
/// GCObject is the base class for ALL objects allocated by our GC.
/// 
/// This simulates what .NET does internally with object headers!
/// </summary>
public class GCObject
{
    /// <summary>
    /// Size of this object in bytes.
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    /// Is this object marked as reachable? (used during GC mark phase)
    /// </summary>
    public bool IsMarked { get; set; }

    /// <summary>
    /// Method table pointer (type info).
    /// </summary>
    public IntPtr MethodTable { get; set; }

    /// <summary>
    /// Address in the GC heap.
    /// </summary>
    public int Address { get; set; }

    public GCObject()
    {
        Size = 0;
        IsMarked = false;
        MethodTable = IntPtr.Zero;
    }
}