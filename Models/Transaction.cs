using System;

namespace CreditKiosk.Models
{
    public class Transaction
    {
        // This should really be decimal, but that won't work with Sqlite using Linq.
        double _amount;

        public int Id { get; set; }

        public DateTime Date { get; set; }

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

        public string Comment { get; set; } = string.Empty;

        public Transaction()
        {
            Date = DateTime.Now;
        }
    }
}
