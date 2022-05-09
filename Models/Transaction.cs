using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditKiosk.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int PersonId { get; set; }
        public Person? Person { get; set; }

        public decimal Amount { get; set; }

        public string Comment { get; set; }

        public Transaction()
        {
            Date = DateTime.Now;
        }
    }
}
