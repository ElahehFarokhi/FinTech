namespace FinTech.Domain.Enums;

public enum TransactionType
{
    Deposit,
    Withdrawal,
    TransferIn,  // Money received from another account
    TransferOut,  // Money sent to another account
}
