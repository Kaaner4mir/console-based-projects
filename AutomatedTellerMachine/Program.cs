using System.Text;

class Program
{
    static Random _rnd = new Random();

    /// <summary>
    /// Sample accounts created for demonstration.
    /// </summary>
    static List<Account> _accounts = new List<Account>()
    {
        new Account
        {
            AccountId = _rnd.Next(10000, 99999),
            AccountName = "Salary Account",
            Branch = "Dublin",
            Currency = "€",
            Balance = 15481.4058m,
            CreatedDate = DateTime.UtcNow.AddMonths(-13).AddDays(-2)
        },
        new Account
        {
            AccountId = _rnd.Next(10000, 99999),
            AccountName = "Saving Account",
            Branch = "Cork",
            Currency = "€",
            Balance = 102005.0109m,
            CreatedDate = DateTime.UtcNow.AddMonths(-12).AddDays(-2)
        }
    };

    static List<CreditCard> _cards = new List<CreditCard>()
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

    public static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        const int registeredPassword = 1111;
        short attempt = 0;

        attempt = UserControl(attempt, registeredPassword);

        while (true)
        {
            try
            {
                SpinnerAnimation();
                DisplayMenu();

                string choice = GetInput<string>("\n👉 Enter the operation you want to perform numerically : ");

                switch (choice)
                {
                    case "1":
                        ShowBalance();
                        break;
                    case "2":
                        DepositMoney();
                        break;
                    case "3":
                        WithdrawMoney();
                        break;
                    default:
                        throw new InvalidOperationException("Invalid menu selection! Please choose a valid option.");
                }
            }
            catch (Exception ex)
            {
                WriteColored($"\n⚠️ Error : {ex.Message}", ConsoleColor.Red);
                WaitingScreen();
            }

            WaitingScreen();
        }
    }

    #region Operations

    private static void ShowBalance()
    {
        Console.Clear();
        ListAccounts();
        ListingCards();
    }

    private static void DepositMoney()
    {
        try
        {
            Console.Clear();
            ListAccounts();

            int id = GetInput<int>("💳 Enter the ID number of the account you want to deposit into: ");

            var account = _accounts.FirstOrDefault(x => x.AccountId == id);

            if (account == null)
                throw new InvalidOperationException("Account not found!");
            else
            {
                decimal depositAmount = GetInput<decimal>("💰 Please enter the amount you want to deposit: ");

                if (depositAmount < 10)
                {
                    WriteColored($"\n❌ The deposit amount must be at least €10", ConsoleColor.Red);
                }
                else
                {
                    account.Balance += depositAmount;

                    account.Transactions.Add(new Transaction
                    {
                        Date = DateTime.Now,
                        Type = "Deposit",
                        Amount = depositAmount,
                        Currency = account.Currency
                    });

                    WriteColored($"\n✅ Successfully deposited {account.Currency}{depositAmount:N2} at {DateTime.Now:dd/MM/yyyy HH:mm}", ConsoleColor.Green);
                }
            }
        }
        catch (Exception ex)
        {
            WriteColored($"\n⚠️ Error : {ex.Message}", ConsoleColor.Red);
        }
    }

    private static void WithdrawMoney()
    {
        try
        {
            Console.Clear();
            ListAccounts();

            int id = GetInput<int>("💳 Enter the ID number of the account you want to withdraw into: ");

            var account = _accounts.FirstOrDefault(x => x.AccountId == id);

            if (account == null)
                throw new InvalidOperationException(" Account not found!");
            else
            {
                decimal withdrawAmount = GetInput<decimal>("💰 Please enter the amount you want to withdraw: ");

                if (withdrawAmount < 10)
                {
                    WriteColored($"\n❌ The deposit amount must be at least €10", ConsoleColor.Red);
                }
                else if (withdrawAmount > account.Balance)
                    WriteColored($"\n❌ Insufficient Balance!", ConsoleColor.Red);
                else
                {
                    account.Balance -= withdrawAmount;

                    account.Transactions.Add(new Transaction
                    {
                        Date = DateTime.Now,
                        Type = "Deposit",
                        Amount = withdrawAmount,
                        Currency = account.Currency
                    });

                    WriteColored($"\n✅ Successfully withdrawed {account.Currency}{withdrawAmount:N2} at {DateTime.Now:dd/MM/yyyy HH:mm}", ConsoleColor.Green);
                }
            }
        }
        catch (Exception ex)
        {
            WriteColored($"\n⚠️ Error : {ex.Message}", ConsoleColor.Red);
        }
    }

    private static void ShowTransactions()
    {
        Console.Clear();
        int id = GetInput<int>("📋 Enter the account ID to view transactions: ");

        var account = _accounts.FirstOrDefault(x => x.AccountId == id);

        if (account == null)
        {
            WriteColored("⚠️ Account not found!", ConsoleColor.Red);
            return;
        }

        WriteColored($"\n📊 Transaction History for {account.AccountName} (ID: {account.AccountId})\n", ConsoleColor.Cyan);

        if (!account.Transactions.Any())
        {
            WriteColored("ℹ️ No transactions found for this account.", ConsoleColor.Yellow);
            return;
        }

        foreach (var trx in account.Transactions)
        {
            ConsoleColor color = trx.Type == "Deposit" ? ConsoleColor.Green : ConsoleColor.Red;
            WriteColored($"[{trx.Date:dd/MM/yyyy HH:mm}] {trx.Type,-10} {trx.Currency}{trx.Amount:N2}", color);
        }
    }



    #endregion

    /// <summary>
    /// Validates user login by checking the PIN code.
    /// </summary>
    private static short UserControl(short attempt, int registeredPassword)
    {
        while (attempt < 5)
        {
            int password = GetInput<int>("👤 Please enter your 4-digit PIN code: ");
            if (password == registeredPassword)
            {
                WriteColored("\n✅ Authentication successful!", ConsoleColor.Green);
                WaitingScreen();
                return attempt;
            }

            attempt++;
            WriteColored($"❌ Invalid PIN! Remaining attempts: {5 - attempt}", ConsoleColor.Yellow);
        }
        WriteColored("🚫 Too many failed attempts! Your card has been temporarily blocked!", ConsoleColor.Red);
        WaitingScreen(ConsoleColor.DarkRed);
        Environment.Exit(0);
        return attempt;
    }

    /// <summary>
    /// Displays all accounts in the system.
    /// </summary>
    private static void ListAccounts()
    {
        WriteColored("🏦 Accounts:\n", ConsoleColor.Cyan);

        int i = 1;
        foreach (var acc in _accounts)
        {
            WriteColored($"[{i++}] Account", ConsoleColor.White);
            WriteColored($"🔑 ID       : {acc.AccountId}", ConsoleColor.Cyan);
            WriteColored($"📛 Name     : {acc.AccountName}", ConsoleColor.Green);
            WriteColored($"📅 Created  : {acc.CreatedDate:dd MMMM yyyy}", ConsoleColor.DarkGray);
            WriteColored($"🏢 Branch   : {acc.Branch}", ConsoleColor.Yellow);
            WriteColored($"💱 Currency : {acc.Currency}", ConsoleColor.Magenta);
            WriteColored($"💰 Balance  : {acc.Currency}{acc.Balance:N2}", ConsoleColor.Blue);

            WriteColored(new string('-', 40), ConsoleColor.DarkCyan);
        }
    }

    /// <summary>
    /// Displays all credit cards in the system.
    /// </summary>
    private static void ListingCards()
    {
        WriteColored("🏦 Credit Cards:\n", ConsoleColor.Cyan);

        foreach (var acc in _cards)
        {
            WriteColored($"🔑 ID       : {acc.AccountId}", ConsoleColor.Cyan);
            WriteColored($"📛 Name     : {acc.CardName}", ConsoleColor.Green);
            WriteColored($"📅 Created  : {acc.CreatedDate:dd MMMM yyyy}", ConsoleColor.DarkGray);
            WriteColored($"🏢 Branch   : {acc.Branch}", ConsoleColor.Yellow);
            WriteColored($"💱 Currency : {acc.Currency}", ConsoleColor.Magenta);
            WriteColored($"💰 Balance  : {acc.Currency}{acc.Balance:N2} ", ConsoleColor.Blue);
            WriteColored($"💲 Debt     : {acc.Currency}{10000 - (acc.Balance)}", ConsoleColor.Blue);

            WriteColored(new string('-', 40), ConsoleColor.DarkCyan);
        }
    }

    /// <summary>
    /// Displays the main menu.
    /// </summary>
    private static void DisplayMenu()
    {
        Console.Clear();
        WriteColored(" << 📋 MENU >>\n", ConsoleColor.Cyan);
        WriteColored("1. List Accounts", ConsoleColor.Yellow);
        WriteColored("2. Deposit Money", ConsoleColor.Green);
        WriteColored("3. Withdraw Money", ConsoleColor.Red);
        WriteColored("4. Payments", ConsoleColor.White);
        WriteColored("5. Applications", ConsoleColor.Blue);
        WriteColored("6. Transactions", ConsoleColor.Magenta);
    }

    /// <summary>
    /// Shows a waiting message until the user presses a key.
    /// </summary>
    private static void WaitingScreen(ConsoleColor color = ConsoleColor.Yellow)
    {
        Console.CursorVisible = false;
        WriteColored("\n⌛ Press any key to continue...", color);
        Console.ReadKey();
        Console.CursorVisible = true;
    }

    /// <summary>
    /// Generic input method for user values.
    /// </summary>
    private static T GetInput<T>(string message, ConsoleColor color = ConsoleColor.White)
    {
        while (true)
        {
            WriteColored(message, color, false);
            string? text = Console.ReadLine();

            try
            {
                if (typeof(T) == typeof(string))
                {
                    if (!string.IsNullOrWhiteSpace(text))
                        return (T)(object)text;
                }
                else
                {
                    return (T)Convert.ChangeType(text, typeof(T));
                }
            }
            catch
            {
                WriteColored($"⚠️ Please enter a valid {typeof(T).Name} value!", ConsoleColor.Red);
            }
        }
    }

    /// <summary>
    /// Writes colored text to the console.
    /// </summary>
    private static void WriteColored(string text, ConsoleColor color = ConsoleColor.White, bool newLine = true)
    {
        Console.ForegroundColor = color;
        if (newLine) Console.WriteLine(text);
        else Console.Write(text);
        Console.ResetColor();
    }

    private static void SpinnerAnimation()
    {
        Console.CursorVisible = false;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;

        char[] chars = { '-', '\\', '|', '/' };
        const short loopDuration = 50;
        short loopTime = 0;

        while (loopTime < 6)
        {
            foreach (var item in chars)
            {
                Console.Write("\r" + item);
                Thread.Sleep(loopDuration);
            }
            loopTime++;
        }
        Console.Clear();
        Console.CursorVisible = true;
    }
}
