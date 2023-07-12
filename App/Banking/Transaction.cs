namespace HelloWorldApp.Banking;

public class Transaction
{
    public decimal Amount { get; }
    public DateTime Datetime { get; }
    public string Notes { get; }

    public Transaction(decimal amount, DateTime datetime, string note)
    {
        Amount = amount;
        Datetime = datetime;
        Notes = note;
    }
}
