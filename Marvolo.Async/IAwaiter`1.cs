namespace Marvolo.Async
{
    public interface IAwaiter<out TResult> : IAwaiter
    {
        new TResult GetResult();
    }
}