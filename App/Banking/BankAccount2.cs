using System.Text;

namespace HelloWorldApp.Banking;

public /*abstract*/ class BankAccount2 : IReconciliation
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


    private readonly decimal minimalBalance;

    public BankAccount2(Customer owner, decimal initBalance)
        : this(owner, initBalance, 0)
    {
    }
    public BankAccount2(Customer owner, decimal initBalance, decimal minimalBalance)
    {
        int account = Interlocked.Increment(ref accountNumberSeed);
        AccountNumber = account.ToString();

        Owner = owner;
        this.minimalBalance = minimalBalance;
        //Balance = initBalance;

        // only make the initial deposit if initBalance > 0
        // this allows the credit account to open with a 0 balance
        if (initBalance > 0)
        {
            MakeDeposit(initBalance, DateTime.Now, "Initial balance");
        }
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

        // if below the minimal balance
        //if (Balance - amount < minimalBalance)
        //{
        //    // overdraw
        //    throw new InvalidOperationException("No sufficient balance for this withdrawal");
        //}
        Transaction? overdraftTransaction = CheckWithdrawalLimit(Balance - amount < minimalBalance);

        var withdrawal = new Transaction(-amount, datetime, note);
        allTransactions.Add(withdrawal);

        // how to handle overdraft transaction?
        if (overdraftTransaction != null)
        {
            allTransactions.Add(overdraftTransaction);
        }
    }

    protected virtual Transaction? CheckWithdrawalLimit(bool isOverdrawn)
    {
        if (isOverdrawn)
        {
            throw new InvalidOperationException("No sufficient balance for this withdrawal");
        }
        else
        {
            return default;
        }
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
