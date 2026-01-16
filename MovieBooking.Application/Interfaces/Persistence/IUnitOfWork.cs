using System.Data;

namespace MovieBooking.Application.Interfaces.Persistence;

/// <summary>
/// Abstraction for coordinating persistence operations across multiple repositories.
/// This is the ONLY component allowed to call SaveChanges and manage transactions.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persists all staged changes to the database.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken ct = default);

    /// <summary>
    /// Executes the given operation inside a database transaction with the specified isolation level.
    /// The transaction is committed if the operation succeeds, or rolled back on failure.
    /// </summary>
    Task ExecuteInTransactionAsync(
        Func<CancellationToken, Task> operation,
        IsolationLevel isolationLevel,
        CancellationToken ct = default);

    /// <summary>
    /// Executes the given operation inside a database transaction with the specified isolation level
    /// and returns a result. The transaction is committed if the operation succeeds, or rolled back on failure.
    /// </summary>
    Task<TResult> ExecuteInTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        IsolationLevel isolationLevel,
        CancellationToken ct = default);
}