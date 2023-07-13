namespace HelloWorldApp.Banking;

public class CreditAccount : BankAccount2
{
    // credit account starts with initial balance of 0, and generally will have a negative balance

    public CreditAccount(Customer owner, decimal initBalance, decimal creditLimit)
        : base(owner, initBalance, -creditLimit)
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

    protected override Transaction? CheckWithdrawalLimit(bool isOverdrawn)
    {
        return
            isOverdrawn
            ? new Transaction(-20, DateTime.Now, "Apply overdraft fee")
            : default;
    }
}
