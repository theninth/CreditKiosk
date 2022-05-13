namespace CreditKiosk.Models
{
    /// <summary>
    /// Class for a deposit transaction. Represents a table in Entity Framework.
    /// </summary>
    public class Deposit : Transaction
    {
        /// <summary>
        /// Person ID of the person the deposit is connected to.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Person the deposit is connected to.
        /// </summary>
        public Person Person { get; set; }
    }
}
