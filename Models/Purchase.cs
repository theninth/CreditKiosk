using System;

namespace CreditKiosk.Models
{
    public class Purchase : Transaction
    {
        
        public Purchase(DateTime date, Person person, decimal amount) : base(date, person, amount)
        {
        }
    }
}
