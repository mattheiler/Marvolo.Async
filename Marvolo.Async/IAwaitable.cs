namespace Marvolo.Async
{
    public interface IAwaitable
    {
        IAwaiter GetAwaiter();
    }
}