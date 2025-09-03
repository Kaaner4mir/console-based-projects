static class DataStore
{
    static Random _rnd = new Random();

    public static List<Account> Accounts = new List<Account>()
    {
        new Account
        {
            AccountId = _rnd.Next(10000,99999),
            AccountName = "Salary Account",
            Branch = "Dublin",
            Currency = "€",
            Balance = 15481.40m,
            CreatedDate = DateTime.UtcNow.AddMonths(-13)
        },
        new Account
        {
            AccountId = _rnd.Next(10000,99999),
            AccountName = "Saving Account",
            Branch = "Cork",
            Currency = "€",
            Balance = 102005.01m,
            CreatedDate = DateTime.UtcNow.AddMonths(-12)
        }
    };
    public static List<CreditCard> Cards = new List<CreditCard>()
    {
        new CreditCard
        {
            AccountId = _rnd.Next(10000,99999),
            CardName = "Arms",
            Branch = "Galway",
            CreatedDate = DateTime.UtcNow.AddMonths(-13).AddDays(-2),
            Currency = "€",
            Balance = 10000.00m
        }
    };
}