namespace Hes.Synchronization {

    using System;
    using System.Threading;

    /// <summary>
    /// This
    /// <see cref="T:System.IDisposable" />
    /// uses the
    /// <see cref="T:System.Threading.Monitor" />
    /// class. It is used to replaces try-finally blocks with "using" statements.
    /// </summary>
    /// <example>
    /// <code>
    ///     using (Lock.Enter(syncRoot))
    ///     {
    ///     // critical section here
    ///     }
    ///   </code>
    /// </example>
    public struct Lock : IDisposable {

        /// <summary>
        /// The sync root.
        /// </summary>
        private readonly object _syncObject;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Hes.Synchronization.Lock" />
        /// struct.
        /// </summary>
        /// <param name="syncObject">
        /// The sync object.
        /// </param>
        private Lock(object syncObject) {
            _syncObject = syncObject;
            Monitor.Enter(syncObject);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Hes.Synchronization.Lock" />
        /// struct.
        /// </summary>
        /// <param name="syncObject">
        /// The sync object.
        /// </param>
        /// <param name="millisecondsTimeout">
        /// The timeout for
        /// <see cref="M:System.Threading.Monitor.TryEnter(System.Object,System.Int32)" />
        /// in milliseconds.
        /// </param>
        /// <exception cref="T:Hes.Synchronization.LockTimeoutException">
        /// <see cref="M:System.Threading.Monitor.TryEnter(System.Object,System.Int32)" />
        /// returned false.
        /// </exception>
        private Lock(object syncObject, int millisecondsTimeout) {
            _syncObject = syncObject;
            if(!Monitor.TryEnter(syncObject, millisecondsTimeout))
                throw new LockTimeoutException();
        }

        /// <summary>
        /// Calls
        /// <see cref="M:System.Threading.Monitor.Exit(System.Object)" />
        /// .
        /// </summary>
        public void Dispose() {
            Monitor.Exit(_syncObject);
        }

        /// <summary>
        /// Enters a critical section with
        /// <see cref="M:System.Threading.Monitor.Enter(System.Object)" />
        /// and returns a new instance of
        /// <see cref="T:Hes.Synchronization.Lock" />
        /// .
        /// </summary>
        /// <param name="syncObject">
        /// The sync object.
        /// </param>
        /// <returns>
        /// Disposing the result leads to
        /// <see cref="M:System.Threading.Monitor.Exit(System.Object)" />
        /// .
        /// </returns>
        public static IDisposable Enter(object syncObject) {
            return new Lock(syncObject);
        }

        /// <summary>
        /// Enters a critical section with
        /// <see cref="M:System.Threading.Monitor.TryEnter(System.Object,System.Int32)" />
        /// and returns a new instance of
        /// <see cref="T:Hes.Synchronization.Lock" />
        /// .
        /// </summary>
        /// <param name="syncObject">
        /// The sync object.
        /// </param>
        /// <param name="millisecondsTimeout">
        /// The milliseconds Timeout.
        /// </param>
        /// <returns>
        /// Disposing the result leads to
        /// <see cref="M:System.Threading.Monitor.Exit(System.Object)" />
        /// .
        /// </returns>
        public static IDisposable TryEnter(object syncObject, int millisecondsTimeout) {
            return new Lock(syncObject, millisecondsTimeout);
        }
    }
}