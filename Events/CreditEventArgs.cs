using CreditKiosk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditKiosk.Events
{
    public class CreditEventArgs : EventArgs
    {
        public Purchase Purchase { get; set; }

        public double Amount { get; set; }

        public CreditEventArgs(Purchase? purchase, double amount)
        {
            Purchase = purchase;
            Amount = amount;
        }
    }
}
