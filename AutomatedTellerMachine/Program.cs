using System.Text;

class Program
{
    public static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

    }


    private static void SpinnerAnimation()
    {
        char[] chars = { '-', '\\', '|', '/' };

        short loopCounter = 0;
        const short loopDuration = 50;

        Console.CursorVisible = false;

        while (loopCounter < 5)
        {
            foreach (var item in chars)
            {
                Console.Write($"\r" + $"{item}");
                Thread.Sleep(loopDuration);
            }
            loopCounter++;
        }
        Console.CursorVisible = true;
        Console.Clear();
    }
}