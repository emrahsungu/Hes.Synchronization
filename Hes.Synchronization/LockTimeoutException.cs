namespace Hes.Synchronization {

    using System;

    /// <summary>
    /// This exception type is thrown if a timeout occurs when creating an instance of
    /// <see cref="T:Hes.Synchronization.Lock" />
    /// ,
    /// <see cref="T:Hes.Synchronization.ReadLock" />
    /// ,
    /// <see cref="T:Hes.Synchronization.WriteLock" />
    /// or
    /// <see cref="T:Hes.Synchronization.UpgradeableReadLock" />
    /// .
    /// </summary>
    public class LockTimeoutException : Exception {
    }
}