namespace CreditKiosk.Models
{
    public class Credit : Transaction
    {
        public int PurchaseId { get; set; }

        public Purchase Purchase { get; set; }

        public Credit(int purchaseId)
        {
            PurchaseId = purchaseId;
        }
    }
}
