using Wallet.Domain.Responses;
using Wallet.Domain.SharedModels;

namespace Wallet.Repository.Interfaces;

public interface ITransactionRepository
{
    Task<Result<List<TransactionResponse>>> GetTransactions(int userId, int walletId,
        CancellationToken cancellationToken = default);
}