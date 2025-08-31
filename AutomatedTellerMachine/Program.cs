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
            CreatedDate = DateTime.UtcNow.AddMonths(-13).AddDays(-2) // 13 months 2 days ago
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

    public static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        const int registeredPassword = 1111;
        short attempt = 0;

        attempt = UserControl(attempt, registeredPassword);

        while (true)
        {
            SpinnerAnimation();
            DisplayMenu();
        }
    }

    /// <summary>
    /// Validates user login by checking the PIN code.
    /// </summary>
    /// <param name="attempt">Number of login attempts.</param>
    /// <param name="registeredPassword">Registered password to validate against.</param>
    /// <returns>Updated number of attempts.</returns>
    private static short UserControl(short attempt, int registeredPassword)
    {
        while (attempt < 5)
        {
            string input = GetStringInput("👤 Please enter your 4-digit PIN code: ");
            if (int.TryParse(input, out int password) && password == registeredPassword)
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
        Console.Clear();
        WriteColored("🏦 Accounts:\n", ConsoleColor.Cyan);

        foreach (var acc in _accounts)
        {
            WriteColored($"🔑 ID       : {acc.AccountId}", ConsoleColor.Cyan);
            WriteColored($"📛 Name     : {acc.AccountName}", ConsoleColor.Green);
            WriteColored($"🏢 Branch   : {acc.Branch}", ConsoleColor.Yellow);
            WriteColored($"💱 Currency : {acc.Currency}", ConsoleColor.Magenta);
            WriteColored($"💰 Balance  : {acc.Balance:N2} {acc.Currency}", ConsoleColor.Blue);
            WriteColored($"📅 Created  : {acc.CreatedDate:dd MMMM yyyy}", ConsoleColor.DarkGray);
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
        WriteColored("2. Exit", ConsoleColor.Red);

        string choice = GetStringInput("\n👉 Enter the operation you want to perform numerically : ");

        if (choice == "1")
            ListAccounts();
        WaitingScreen();
    }

    /// <summary>
    /// Shows a waiting message until the user presses a key.
    /// </summary>
    /// <param name="color">Text color.</param>
    private static void WaitingScreen(ConsoleColor color = ConsoleColor.Yellow)
    {
        Console.CursorVisible = false;
        WriteColored("\n⌛ Press any key to continue...", color);
        Console.ReadKey();
        Console.CursorVisible = true;
    }

    /// <summary>
    /// Reads user input from console.
    /// </summary>
    /// <param name="message">Message to display.</param>
    /// <param name="color">Text color of the message.</param>
    /// <returns>User input as string.</returns>
    private static string GetStringInput(string message, ConsoleColor color = ConsoleColor.White)
    {
        while (true)
        {
            WriteColored(message, color, false);
            string? text = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(text))
                return text;
        }
    }

    /// <summary>
    /// Writes colored text to the console.
    /// </summary>
    /// <param name="text">Text to print.</param>
    /// <param name="color">Text color.</param>
    /// <param name="newLine">Whether to add a new line after text.</param>
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

