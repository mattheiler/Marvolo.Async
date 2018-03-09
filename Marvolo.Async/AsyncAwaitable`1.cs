using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Marvolo.Async
{
    [DebuggerStepThrough]
    public struct AsyncAwaitable<T> : IAwaitable<T>
    {
        private readonly Task<T> _task;

        public AsyncAwaitable(Task<T> task)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
        }

        public IAwaiter<T> GetAwaiter()
        {
            return new AsyncAwaiter<T>(_task);
        }

        IAwaiter IAwaitable.GetAwaiter()
        {
            return GetAwaiter();
        }
    }
}