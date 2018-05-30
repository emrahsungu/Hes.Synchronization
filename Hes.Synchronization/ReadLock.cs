namespace Hes.Synchronization {

    using System;
    using System.Threading;

    /// <summary>
    /// This
    /// <see cref="T:System.IDisposable" />
    /// uses the
    /// <see cref="T:System.Threading.ReaderWriterLockSlim" />
    /// for critical sections that allow one writer and multiple reader. The counter parts are
    /// <see cref="T:Hes.Synchronization.WriteLock" />
    /// and
    /// <see cref="T:Hes.Synchronization.UpgradeableReadLock" />
    /// . It is used to replaces try-finally blocks with "using" statements.
    /// </summary>
    /// <example>
    /// <code>
    ///     using (ReadLock.Enter(rwLock))
    ///     {
    ///     // critical section here
    ///     }
    ///   </code>
    /// </example>
    public struct ReadLock : IDisposable {

        /// <summary>
        /// The reader writer lock.
        /// </summary>
        private readonly ReaderWriterLockSlim _syncObject;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Hes.Synchronization.ReadLock" />
        /// struct.
        /// </summary>
        /// <param name="syncObject">
        /// The reader writer lock
        /// </param>
        private ReadLock(ReaderWriterLockSlim syncObject) {
            _syncObject = syncObject;
            syncObject.EnterReadLock();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Hes.Synchronization.ReadLock" />
        /// struct.
        /// </summary>
        /// <param name="syncObject">
        /// The reader writer lock
        /// </param>
        /// <param name="millisecondsTimeout">
        /// The timeout for
        /// <see cref="M:System.Threading.ReaderWriterLockSlim.TryEnterReadLock(System.Int32)" />
        /// in milliseconds.
        /// </param>
        /// <exception cref="T:Hes.Synchronization.LockTimeoutException">
        /// <see cref="M:System.Threading.ReaderWriterLockSlim.TryEnterReadLock(System.Int32)" />
        /// returned false.
        /// </exception>
        private ReadLock(ReaderWriterLockSlim syncObject, int millisecondsTimeout) {
            _syncObject = syncObject;
            if(!syncObject.TryEnterReadLock(millisecondsTimeout))
                throw new LockTimeoutException();
        }

        /// <summary>
        /// Calls
        /// <see cref="M:System.Threading.ReaderWriterLockSlim.ExitReadLock" />
        /// .
        /// </summary>
        public void Dispose() {
            _syncObject.ExitReadLock();
        }

        /// <summary>
        /// Enters a critical section with
        /// <see cref="M:System.Threading.ReaderWriterLockSlim.EnterReadLock" />
        /// and returns a new instance of
        /// <see cref="T:Hes.Synchronization.ReadLock" />
        /// .
        /// </summary>
        /// <param name="syncObject">
        /// The reader writer lock.
        /// </param>
        /// <returns>
        /// A
        /// <see cref="T:Hes.Synchronization.ReadLock" />
        /// that can be disposed to call
        /// <see cref="M:System.Threading.ReaderWriterLockSlim.ExitReadLock" />
        /// .
        /// </returns>
        public static IDisposable Enter(ReaderWriterLockSlim syncObject) {
            return new ReadLock(syncObject);
        }

        /// <summary>
        /// Enters a critical section with
        /// <see cref="M:System.Threading.ReaderWriterLockSlim.TryEnterReadLock(System.Int32)" />
        /// and returns a new instance of
        /// <see cref="T:Hes.Synchronization.ReadLock" />
        /// .
        /// </summary>
        /// <param name="syncObject">
        /// The reader writer lock.
        /// </param>
        /// <param name="millisecondsTimeout">
        /// The timeout for
        /// <see cref="M:System.Threading.ReaderWriterLockSlim.TryEnterReadLock(System.Int32)" />
        /// in milliseconds.
        /// </param>
        /// <returns>
        /// A
        /// <see cref="T:Hes.Synchronization.WriteLock" />
        /// that can be disposed to call
        /// <see cref="M:System.Threading.ReaderWriterLockSlim.ExitReadLock" />
        /// .
        /// </returns>
        /// <exception cref="T:Hes.Synchronization.LockTimeoutException">
        /// <see cref="M:System.Threading.ReaderWriterLockSlim.TryEnterReadLock(System.Int32)" />
        /// returned false.
        /// </exception>
        public static IDisposable TryEnter(ReaderWriterLockSlim syncObject, int millisecondsTimeout) {
            return new ReadLock(syncObject, millisecondsTimeout);
        }
    }
}