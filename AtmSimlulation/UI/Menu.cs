class Menu
{
    public static void DisplayMainMenu()
    {
        ConsoleHelper.WriteColored("<< 📋 Main Menu >>\n", ConsoleColor.Cyan);

        var mainMenuItems = new (string Text, ConsoleColor Color)[]
        {
            ("1. List Accounts", ConsoleColor.Yellow),
            ("2. Deposit Money", ConsoleColor.Green),
            ("3. Withdraw Money", ConsoleColor.Red),
            ("4. Payments", ConsoleColor.White),
            ("5. Applications", ConsoleColor.Blue),
            ("6. Transactions", ConsoleColor.Magenta)
        };

        foreach (var item in mainMenuItems)
        {
            ConsoleHelper.WriteColored(item.Text, item.Color);
        }
    }
}