class DepositService
{
    public static void Deposit()
    {
        try
        {
            Console.Clear();

            ListingAccount.ListDemandAccount();

            int id = ConsoleHelper.GetInput<int>("💳 Enter the ID number of the account you want to deposit into: ");

            var account = DataStore.Accounts.FirstOrDefault(x => x.AccountId == id);

            if (account == null)
            {
                ConsoleHelper.WriteColored("\n⛔ The ID you entered was not found in the system. Please enter a valid ID.", ConsoleColor.Red);
                return;
            }

            decimal depositAmount = ConsoleHelper.GetInput<decimal>("💰 Please enter the amount you want to deposit: ");

            if (depositAmount <= 0)
            {
                ConsoleHelper.WriteColored("\n❌ Please enter a valid positive value.", ConsoleColor.Red);
                return;
            }

            if (depositAmount < 10)
            {
                ConsoleHelper.WriteColored("\n❌ The deposit amount must be at least €10.", ConsoleColor.Red);
                return;
            }

            TransactionService.Deposit(account, depositAmount);

        }
        catch (Exception exc)
        {
            ConsoleHelper.WriteColored($"\n⛔ Error: {exc.Message}", ConsoleColor.Red);
        }
    }
}
