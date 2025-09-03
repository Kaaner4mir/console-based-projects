using System.Text;

class Startup
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            try
            {
                Console.Clear();

                Menu.DisplayMainMenu();

                short operation = ConsoleHelper.GetInput<short>("\n👉 Please enter the number of the operation you want to select : ");

                switch (operation)
                {
                    case 1: ListingAccount.ListDemandAccount(); break;
                    case 2: DepositService.Deposit(); break;
                    case 3: WithdrawService.Withdraw(); break;
                    default: throw new Exception("You made an unknown transaction!");
                }
                ConsoleHelper.WaitingScreen();
                ConsoleHelper.SpinnerAnimation();
            }

            catch (Exception exc)
            {
                ConsoleHelper.WriteColored($"⛔ Error : {exc.Message}");
            }
        }
    }
}