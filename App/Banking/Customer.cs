namespace HelloWorldApp.Banking;

public class Customer
{
    public string Name { get; private set; }
    public string Address { get; private set; }
    public string SSN { get; private set; }

    public Customer(string name, string ssn, string address)
    {
        Name = name;
        SSN = ssn;
        Address = address;
    }
}
