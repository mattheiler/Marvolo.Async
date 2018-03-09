using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Marvolo.Async
{
    [DebuggerStepThrough]
    public class AsyncAwaiter<T> : IAwaiter<T>
    {
        private readonly Task<T> _task;

        public AsyncAwaiter(Task<T> task)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
        }

        public void OnCompleted(Action continuation)
        {
            _task.GetAwaiter().OnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _task.GetAwaiter().UnsafeOnCompleted(continuation);
        }

        public bool IsCompleted => _task.GetAwaiter().IsCompleted;

        public T GetResult()
        {
            return _task.GetAwaiter().GetResult();
        }

        void IAwaiter.GetResult()
        {
            GetResult();
        }
    }
}