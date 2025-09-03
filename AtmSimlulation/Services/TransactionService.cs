public static class TransactionService
{
    public static void Deposit(Account account, decimal amount)
    {
        account.Balance += amount;
        account.Transactions.Add(new Transaction
        {
            Date = DateTime.Now,
            Type = "Deposit",
            Amount = amount,
            Currency = account.Currency
        });

        ConsoleHelper.WriteColored($"\n✅ Successfully deposited {account.Currency}{amount:N2}", ConsoleColor.Green);
        ConsoleHelper.WriteColored($"🆕 New balance: {account.Currency}{account.Balance:N2}", ConsoleColor.Green);
    }

    public static void Withdraw(Account account, decimal amount)
    {
        if (amount > account.Balance)
        {
            ConsoleHelper.WriteColored("\n❌ Insufficient funds.", ConsoleColor.Red);
            return;
        }

        account.Balance -= amount;
        account.Transactions.Add(new Transaction
        {
            Date = DateTime.Now,
            Type = "Withdrawal",
            Amount = amount,
            Currency = account.Currency
        });

        ConsoleHelper.WriteColored($"\n✅ Successfully withdrew {account.Currency}{amount:N2}", ConsoleColor.Green);
        ConsoleHelper.WriteColored($"🆕 New balance: {account.Currency}{account.Balance:N2}", ConsoleColor.Green);
    }
}
