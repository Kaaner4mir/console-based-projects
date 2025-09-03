static class ConsoleHelper
{
    public static T GetInput<T>(string message, ConsoleColor color = ConsoleColor.White)
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

    public static void WriteColored(string text, ConsoleColor color = ConsoleColor.White, bool newLine = true)
    {
        Console.ForegroundColor = color;
        if (newLine) Console.WriteLine(text);
        else Console.Write(text);
        Console.ResetColor();
    }

    public static void WaitingScreen(ConsoleColor color = ConsoleColor.Yellow)
    {
        Console.CursorVisible = false;
        WriteColored("\n⌛ Press any key to continue...", color);
        Console.ReadKey();
        Console.CursorVisible = true;
    }

    public static void SpinnerAnimation()
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