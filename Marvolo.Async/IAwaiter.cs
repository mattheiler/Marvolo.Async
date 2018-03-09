using System.Runtime.CompilerServices;

namespace Marvolo.Async
{
    public interface IAwaiter : ICriticalNotifyCompletion
    {
        bool IsCompleted { get; }

        void GetResult();
    }
}