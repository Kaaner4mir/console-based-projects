class Account
{
    public int AccountId { get; set; }
    public string? AccountName { get; set; }
    public string? Branch { get; set; }
    public string? Currency { get; set; }
    public DateTime CreatedDate { get; set; }
    public decimal? Balance { get; set; }
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();

}

class CreditCard : Account
{
    public int AccountId { get; set; }
    public string? CardName { get; set; }
    public string? Branch { get; set; }
    public string? Currency { get; set; }
    public DateTime CreatedDate { get; set; }
    public decimal? Balance { get; set; }
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();

}