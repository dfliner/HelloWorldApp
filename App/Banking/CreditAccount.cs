namespace HelloWorldApp.Banking;

public class CreditAccount : BankAccount
{
    public CreditAccount(Customer owner, decimal initBalance)
        : base(owner, initBalance)
    {
    }

    public override void PerformMonthEndTransactions()
    {
        if (Balance < 0)
        {
            decimal interest = -Balance * 0.199m;
            MakeWithdrawal(interest, DateTime.Now, "Charge monthly interest");
        }
    }
}
