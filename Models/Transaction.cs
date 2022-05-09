using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditKiosk.Models
{
    public class Transaction
    {
        public string? Id { get; set; } = null;

        public DateTime Date { get; set; }

        public Person Person { get; set; }

        public decimal Amount { get; set; }

        public int Comment
        {
            get => default;
            set
            {
            }
        }

        public Transaction(DateTime date, Person person, decimal amount)
        {
            Date = date;
            Person = person;
            Amount = amount;
        }
    }
}
