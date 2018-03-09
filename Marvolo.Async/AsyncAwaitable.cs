using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Marvolo.Async
{
    [DebuggerStepThrough]
    public struct AsyncAwaitable : IAwaitable
    {
        private readonly Task _task;

        public AsyncAwaitable(Task task)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
        }

        public IAwaiter GetAwaiter()
        {
            return new AsyncAwaiter(_task);
        }
    }
}