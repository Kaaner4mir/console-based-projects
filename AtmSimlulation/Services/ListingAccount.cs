class ListingAccount
{
    public static void ListDemandAccount()
    {
        Console.Clear();

        ConsoleHelper.WriteColored("🏦 Accounts:\n", ConsoleColor.Cyan);

        int i = 1;
        foreach (var acc in DataStore.Accounts)
        {
            ConsoleHelper.WriteColored($"[{i++}] Account\n", ConsoleColor.White);
            ConsoleHelper.WriteColored($"🔑 ID       : {acc.AccountId}", ConsoleColor.Cyan);
            ConsoleHelper.WriteColored($"📛 Name     : {acc.AccountName}", ConsoleColor.Green);
            ConsoleHelper.WriteColored($"📅 Created  : {acc.CreatedDate:dd MMMM yyyy}", ConsoleColor.DarkGray);
            ConsoleHelper.WriteColored($"🏢 Branch   : {acc.Branch}", ConsoleColor.Yellow);
            ConsoleHelper.WriteColored($"💱 Currency : {acc.Currency}", ConsoleColor.Magenta);
            ConsoleHelper.WriteColored($"💰 Balance  : {acc.Currency}{acc.Balance:N2}", ConsoleColor.Blue);

            ConsoleHelper.WriteColored(new string('-', 40), ConsoleColor.DarkCyan);
        }
    }
    public static void ListingCreditCards()
    {
        ConsoleHelper.WriteColored("🏦 Credit Cards:\n", ConsoleColor.Cyan);

        foreach (var acc in DataStore.Cards)
        {
            ConsoleHelper.WriteColored($"🔑 ID       : {acc.AccountId}", ConsoleColor.Cyan);
            ConsoleHelper.WriteColored($"📛 Name     : {acc.CardName}", ConsoleColor.Green);
            ConsoleHelper.WriteColored($"📅 Created  : {acc.CreatedDate:dd MMMM yyyy}", ConsoleColor.DarkGray);
            ConsoleHelper.WriteColored($"🏢 Branch   : {acc.Branch}", ConsoleColor.Yellow);
            ConsoleHelper.WriteColored($"💱 Currency : {acc.Currency}", ConsoleColor.Magenta);
            ConsoleHelper.WriteColored($"💰 Balance  : {acc.Currency}{acc.Balance:N2} ", ConsoleColor.Blue);
            ConsoleHelper.WriteColored($"💲 Debt     : {acc.Currency}{10000 - (acc.Balance)}", ConsoleColor.Blue);

            ConsoleHelper.WriteColored(new string('-', 40), ConsoleColor.DarkCyan);
        }
    }
}