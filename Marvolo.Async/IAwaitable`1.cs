namespace Marvolo.Async
{
    public interface IAwaitable<out TResult> : IAwaitable
    {
        new IAwaiter<TResult> GetAwaiter();
    }
}