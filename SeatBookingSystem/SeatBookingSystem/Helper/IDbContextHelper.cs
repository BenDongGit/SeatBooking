namespace SeatBookingSystem.Helper
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Threading.Tasks;

    /// <summary>
    /// The database context wrapper interface
    /// </summary>
    /// <typeparam name="TContext">The database context</typeparam>
    public interface IDbContextHelper<TContext> where TContext : DbContext, new()
    {
        /// <summary>
        /// Calls the context action
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="func">The action body.</param>
        /// <returns>The result.</returns>
        T Call<T>(Func<TContext, T> func);

        /// <summary>
        /// Calls the context action
        /// </summary>
        /// <param name="action">The action</param>
        void Call(Action<TContext> action);

        /// <summary>
        /// Calls the context action async
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="func">The action body.</param>
        /// <returns>The result.</returns>
        Task<T> CallAsync<T>(Func<TContext, Task<T>> func);

        /// <summary>
        /// Calls the context action async
        /// </summary>
        /// <param name="func">The action</param>
        /// <returns>The action task</returns>
        Task CallAsync(Func<TContext, Task> func);

        /// <summary>
        /// Calls the context action with transaction.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="func">The action.</param>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns>The result</returns>
        T CallWithTransaction<T>(Func<TContext, T> func, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Calls the context action with transaction.
        /// </summary>
        /// <param name="action">The action</param>
        /// <param name="isolationLevel">The isolation level.</param>
        void CallWithTransaction(Action<TContext> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Calls the context action with transaction async.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="func">The action.</param>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns>The result</returns>
        Task<T> CallWithTransactionAsync<T>(Func<TContext, Task<T>> func, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Calls the context action with transaction async.
        /// </summary>
        /// <param name="func">The action.</param>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns>The result</returns>
        Task CallWithTransactionAsync(Func<TContext, Task> func, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}