// Learning C# and .NET programming
// https://learn.microsoft.com/en-us/dotnet/csharp/
// https://www.pluralsight.com/courses/c-sharp-10-fundamentals
// https://www.pluralsight.com/courses/c-sharp-10-best-practices
// https://www.pluralsight.com/courses/c-sharp-10-language-features-advanced
// https://www.pluralsight.com/courses/c-sharp-10-design-patterns


// Coding conventions
// https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions
// https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md
// https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/


using HelloWorldApp;
using HelloWorldApp.Banking;

Console.WriteLine("Hello Blue!");
Console.WriteLine();

Customer customer = new Customer("Blue, DSCE", "123-45-6789", "21601 76th Ave S, Kent, WA 98032");
var bankAccount = new BankAccount(customer, 1000m); // m/M suffix = decimal literal
Console.WriteLine($"Account {bankAccount.AccountNumber} was created for {bankAccount.Owner.Name},");
Console.WriteLine($"with initial balance of {bankAccount.Balance}");
Console.WriteLine();

bankAccount.MakeWithdrawal(500, DateTime.Now, "Rent payment");
Console.WriteLine("Remaining balance in account: {0}", bankAccount.Balance);
bankAccount.MakeDeposit(200, DateTime.Now, "Friend paid me back");
Console.WriteLine("New balance after deposit: {0}", bankAccount.Balance);
Console.WriteLine("All transactions:");
Console.WriteLine(bankAccount.ReportTransactionHistory());

var creditcard = new CreditAccount(customer, 0);
creditcard.MakeWithdrawal(100m, DateTime.Now, "Take out monthly advance");

try
{
    BankAccount invalidAccount = new BankAccount(customer, -55);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine(ex.Message);
}
Console.WriteLine();


IList<IReconciliation> accounts = new List<IReconciliation>
{
    new SavingsAccount(customer, 100m),
    new CheckingAccount(customer, 1000m, 10m),
    new CreditAccount(customer, 10000m),
};

foreach (var account in accounts)
{
    account.PerformMonthEndTransactions();
}




List<string> websites = new List<string>
{
    "www.google.com",
    "www.microsoft.com",
    "www.facebook.com",
    "www.linkedin.com",
    "www.twitter.com"
};

WebsiteConnections connections = new WebsiteConnections(websites);
await connections.ConnectInSequentialAsync("https");
Console.WriteLine();

await connections.ConnectInParallelAsync("http");
Console.WriteLine();

Console.WriteLine("Starting connections in for loop");
for (int i = 0; i < websites.Count; i++)
{
    using (var client = new HttpClient())
    {
        HttpResponseMessage result;
        if (i % 2 == 0)
        {
            result = await client.GetAsync($"https://{websites[i]}");
        }
        else
        {
            result = await client.GetAsync($"http://{websites[i]}");
        }
        
        Console.WriteLine($"Connection {i} - {websites[i]}: {result.StatusCode}");
    }
}
Console.WriteLine();

Console.WriteLine("Starting connections in for-each loop");
foreach (var website in websites)
{
    using (var client = new HttpClient())
    {
        var result = await client.GetAsync($"https://{website}");
        Console.WriteLine($"Connection to {website}: {result.StatusCode}");
    }
}
Console.WriteLine();


// BUG: What happens without await?
//for (int i = 0; i < websites.Count; i++)
//{
//    _ =Task.Run((async () =>
//    {
//        using (var client = new HttpClient())
//        {
//            var result = await client.GetAsync($"https://{websites[i]}");
//            Console.WriteLine($"Connection {i} - {websites[i]}: {result.StatusCode}");
//        }
//    }));

//    Console.WriteLine($"Iteration: {i}");
//}


// BUG: throws out-of-range exception
//IList<Task> tasks = new List<Task>();
//for (int i = 0; i < websites.Count; i++)
//{
//    var task = Task.Run((async () =>
//    {
//        // BUG: when each task starts, i has been incremented to 5
//        using (var client = new HttpClient())
//        {
//            var result = await client.GetAsync($"https://{websites[i]}");
//            Console.WriteLine($"Connection {i} - {websites[i]}: {result.StatusCode}");
//        }
//    }));

//    tasks.Add(task);

//    Console.WriteLine($"Iteration: {i}");
//}
//Task.WaitAll(tasks.ToArray());
//await Task.WhenAll(tasks.ToArray());


Console.WriteLine("Parallelize connections");
IList<Task> tasks = new List<Task>();
for (int i = 0; i < websites.Count; i++)
{
    int index = i;
    string website = websites[i];

    var task = Task.Run(async () =>
    {
        using (var client = new HttpClient())
        {
            var result = await client.GetAsync($"https://{website}");
            Console.WriteLine($"Connection {index} - {website}: {result.StatusCode}");
        }
    });
    tasks.Add(task);

    Console.WriteLine($"Iteration: {i}");
}

Task.WaitAll(tasks.ToArray());
