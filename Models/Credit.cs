namespace CreditKiosk.Models
{
    /// <summary>
    /// Class for a credit transaction. Represents a table in Entity Framework.
    /// </summary>
    public class Credit : Transaction
    {
        /// <summary>
        /// Purchase ID
        /// </summary>
        public int PurchaseId { get; set; }

        /// <summary>
        /// Purchase
        /// </summary>
        public Purchase Purchase { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="purchaseId">Id of purchase that the credit is connected to.</param>
        public Credit(int purchaseId)
        {
            PurchaseId = purchaseId;
        }
    }
}
