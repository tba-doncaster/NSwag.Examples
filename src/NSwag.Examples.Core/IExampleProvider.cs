namespace NSwag.Examples.Core;

public interface IExampleProvider<out T>
{
    T GetExample();
}