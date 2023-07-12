namespace HelloWorldApp.Banking;

public class CheckingAccount : BankAccount
{
    private readonly decimal monthlyReward = 0m;

    public CheckingAccount(Customer owner, decimal initBalance, decimal monthlyReward)
        : base(owner, initBalance)
    {
        this.monthlyReward = monthlyReward;
    }

    public override void PerformMonthEndTransactions()
    {
        if (monthlyReward != 0)
        {
            MakeDeposit(monthlyReward, DateTime.Now, "Add monthly reward");
        }
    }
}
