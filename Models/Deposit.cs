namespace CreditKiosk.Models
{
    public class Deposit : Transaction
    {
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
