namespace QuickMGenerate.Tests._Tools;

public class Container<T>
{
    public Container() { }
    public Container(T? value) { Value = value; }
    public T? Value;
}
