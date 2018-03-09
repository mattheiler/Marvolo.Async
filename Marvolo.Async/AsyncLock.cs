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

        public bool IsEntered => _semaphore.CurrentCount == 0;

        ~AsyncLock()
        {
            _semaphore.Dispose();
        }

        public IDisposable Lock()
        {
            return Async.RunSynchronously(EnterAsync);
        }

        public AsyncAwaitable<IDisposable> LockAsync()
        {
            return Async.Run(EnterAsync);
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