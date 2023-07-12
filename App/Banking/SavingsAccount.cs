namespace HelloWorldApp.Banking;

public class SavingsAccount : BankAccount
{
    public SavingsAccount(Customer owner, decimal initBalance)
        : base(owner, initBalance)
    {
    }

    public override void PerformMonthEndTransactions()
    {
        if (Balance > 500m)
        {
            decimal interest = Balance * 0.05m;
            MakeDeposit(interest, DateTime.Now, "Apply monthly interest");
        }
    }
}
