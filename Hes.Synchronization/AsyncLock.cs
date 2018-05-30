namespace Hes.Synchronization {

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AsyncLock {

        /// <summary>
        /// Placeholder for a disposable resource which will be used to release semaphore
        /// </summary>
        private readonly Task<Releaser> _mReleaser;

        /// <summary>
        /// Underyling synchronization object.
        /// </summary>
        private readonly SemaphoreSlim _mSemaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Creates an AsyncLock.
        /// </summary>
        public AsyncLock() {
            _mReleaser = Task.FromResult(new Releaser(this));
        }

        /// <summary>
        /// Acquire lock async.
        /// </summary>
        /// <returns></returns>
        public Task<Releaser> LockAsync() {
            var wait = _mSemaphore.WaitAsync();
            return wait.IsCompleted ?
                _mReleaser
                : wait.ContinueWith((_, state) => new Releaser((AsyncLock) state), this, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        public struct Releaser : IDisposable {
            private readonly AsyncLock _mToRelease;

            internal Releaser(AsyncLock toRelease) {
                _mToRelease = toRelease;
            }

            public void Dispose() {
                _mToRelease?._mSemaphore.Release();
            }
        }
    }
}