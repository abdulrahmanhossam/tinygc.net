namespace TinyGC;

/// <summary>
/// GcRootHandle keeps a reference to an object, making it "reachable".
/// 
/// WHAT is a root?
/// A "root" is a reference that keeps an object alive.
/// If an object has no roots pointing to it, it's garbage!
/// 
/// Real .NET GC scans these roots to find reachable objects!
/// </summary>
public class GcRootHandle
{
    /// <summary>
    /// The object this handle points to.
    /// </summary>
    private GCObject? _target;

    /// <summary>
    /// Create a new root handle pointing to null.
    /// </summary>
    public GcRootHandle()
    {
        _target = null;
    }

    /// <summary>
    /// Create a root handle pointing to a specific object.
    /// </summary>
    public GcRootHandle(GCObject target)
    {
        _target = target;
    }

    /// <summary>
    /// Get/set the target object.
    /// </summary>
    public GCObject? Target
    {
        get => _target;
        set => _target = value;
    }

    /// <summary>
    /// Check if this handle is empty.
    /// </summary>
    public bool IsAllocated => _target != null;

    /// <summary>
    /// Free this handle (clear the reference).
    /// </summary>
    public void Free()
    {
        _target = null;
    }
}