namespace TinyGC;

/// <summary>
/// MyData - A simple test class for our GC.
/// </summary>
public class MyData : GCObject
{
    public int Value { get; set; }
    public string Name { get; set; } = "";

    public MyData()
    {
        Value = 0;
        Name = "default";
    }

    public new int Size => base.Size > 0 ? base.Size : 32;
}