namespace CreditKiosk.Enums
{
    /// <summary>
    /// All the types a transaction can be.
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// Deposit type of transaction.
        /// </summary>
        Deposit,
        /// <summary>
        /// Purchase type of transaction.
        /// </summary>
        Purchase,
        /// <summary>
        /// Credit transaction connected to a purchase.
        /// </summary>
        Credit
    }
}
