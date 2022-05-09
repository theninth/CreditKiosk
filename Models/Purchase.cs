using System;

namespace CreditKiosk.Models
{
    public class Purchase : Transaction
    {
        
        public Purchase(DateTime date, Person person, decimal amount, ProductGroup group) : base(date, person, amount)
        {
            Group = group;
        }

        public ProductGroup Group { get; set; }
    }
}
