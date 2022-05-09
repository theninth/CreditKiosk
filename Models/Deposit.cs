using System;

namespace CreditKiosk.Models
{
    public class Deposit : Transaction
    {
        public Deposit(DateTime date, Person person, decimal amount) : base(date, person, amount)
        {
        }
    }
}
