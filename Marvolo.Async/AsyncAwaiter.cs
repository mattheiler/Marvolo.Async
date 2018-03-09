using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Marvolo.Async
{
    [DebuggerStepThrough]
    public struct AsyncAwaiter : IAwaiter
    {
        private readonly Task _task;

        public AsyncAwaiter(Task task)
        {
            _task = task;
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

        public void GetResult()
        {
            _task.GetAwaiter().GetResult();
        }
    }
}