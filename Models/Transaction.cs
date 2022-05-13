using System;

namespace CreditKiosk.Models
{
    /// <summary>
    /// Base class for transactions.
    /// </summary>
    public abstract class Transaction
    {
        // This should really be decimal, but that won't work with Sqlite using Linq.
        double _amount;

        /// <summary>
        /// Id of the transaction.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date and time for the transaction.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Amount of the transaction.
        /// </summary>
        public double Amount
        {
            get { return _amount; }
            set
            {
                if (value > 0)
                {
                    _amount = value;
                }
                else
                {
                    throw new ArgumentException("Summan kan inte vara 0 kr eller under.");
                }
            }
        }

        /// <summary>
        /// An optional comment of the transaction.
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Transaction()
        {
            Date = DateTime.Now;
        }
    }
}
