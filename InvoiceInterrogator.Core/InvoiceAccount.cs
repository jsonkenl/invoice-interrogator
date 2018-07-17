namespace InvoiceInterrogator.Core
{
    public class InvoiceAccount
    {
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
