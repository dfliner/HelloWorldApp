// https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/tutorials/oop
/*
    Abstraction: Modeling the relevant attributes and interactions of entities as classes
                 to define an abstract representation of a system.
    Encapsulation: Hiding the internal state and functionality of an object and only allowing access
                 through a public set of functions.
    Inheritance: Ability to create new abstractions based on existing abstractions.
    Polymorphism: Ability to implement inherited properties or methods in different ways
                  across multiple abstractions.
*/
using System.Text;

namespace HelloWorldApp.Banking;

public /*abstract*/ class BankAccount : IReconciliation
{
    // To create a new account
    private static int accountNumberSeed = 1000000000;

    // Record all transactions happened in this account
    private List<Transaction> allTransactions = new List<Transaction>();

    public string AccountNumber { get; }
    public Customer Owner { get; }

    public decimal Balance
    {
        get
        {
            decimal balance = 0;
            foreach (var trans in allTransactions)
            {
                balance += trans.Amount;
            }
            return balance;
        }
    }

    public IReadOnlyList<Transaction> AllTransactions => allTransactions.AsReadOnly();

    public BankAccount(Customer owner, decimal initBalance)
    {
        int account = Interlocked.Increment(ref accountNumberSeed);
        AccountNumber = account.ToString();

        Owner = owner;
        //Balance = initBalance;
        MakeDeposit(initBalance, DateTime.Now, "Initial balance");
    }

    public void MakeDeposit(decimal amount, DateTime datetime, string note)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Deposit amount must be positive");
        }

        var deposit = new Transaction(amount, datetime, note);
        allTransactions.Add(deposit);
    }

    public void MakeWithdrawal(decimal amount, DateTime datetime, string note)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Withdraw amount must be positive");
        }

        if (Balance - amount < 0)
        {
            // overdraw
            throw new InvalidOperationException("No sufficient balance for this withdrawal");
        }

        var withdrawal = new Transaction(-amount, datetime, note);
        allTransactions.Add(withdrawal);
    }

    public string ReportTransactionHistory()
    {
        var report = new StringBuilder();
        report.AppendLine("Date\t\tAmount\tBalance\tNote");

        decimal balance = 0;
        foreach (var trans in AllTransactions)
        {
            balance += trans.Amount;
            report.AppendLine($"{trans.Datetime.ToShortDateString()}\t{trans.Amount}\t{balance}\t{trans.Notes}");
        }

        return report.ToString();
    }

    public virtual void PerformMonthEndTransactions()
    {

    }
}
