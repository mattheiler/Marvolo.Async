using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Marvolo.Async
{
    [DebuggerStepThrough]
    public sealed class AsyncLock
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        ~AsyncLock()
        {
            _semaphore.Dispose();
        }

        public IDisposable Lock()
        {
            return EnterAsync().GetAwaiter().GetResult();
        }

        public AsyncAwaitable<IDisposable> LockAsync()
        {
            return new AsyncAwaitable<IDisposable>(EnterAsync());
        }

        private async Task<IDisposable> EnterAsync()
        {
            await _semaphore.WaitAsync();
            return new Releaser(this);
        }

        private void Exit()
        {
            _semaphore.Release();
        }

        private class Releaser : IDisposable
        {
            private readonly AsyncLock _lock;

            public Releaser(AsyncLock @lock)
            {
                _lock = @lock;
            }

            void IDisposable.Dispose()
            {
                _lock.Exit();
            }
        }
    }
}