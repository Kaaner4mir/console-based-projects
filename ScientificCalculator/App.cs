using System.Text;

class App
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        while (true)
        {
            DisplayMenu();

            Message(ConsoleColor.White, "Yapmak istediğiniz işlemi numerik olarak seçiniz (1-?) : ");
            if (!short.TryParse(Console.ReadLine(), out short inputOperation)) { }

        }
    }



    /// <summary>
    ///  Konsola ana işlem menüsünü görüntüler.
    /// </summary>
    private static void DisplayMenu()
    {
        Console.Clear();

        Operation("1", "Toplama      ➕");
        Operation("2", "Çıkarma      ➖");
        Operation("3", "Çarpma       ✖️");
        Operation("4", "Bölme        ➗");
    }
    /// <summary>
    /// Konsola istenilen renkte mesaj yazdırır.
    /// </summary>
    private static void Message(ConsoleColor color, string message)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    /// <summary>
    /// Belirtilen sıra adı ve işlem ayrıntılarıyla birlikte bir işlem mesajını konsolda görüntüler.
    /// </summary>
    private static void Operation(string queue, string operation)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{queue}. ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{operation}\n");
    }
    /// <summary>
    /// Kullanıcı tarafından herhangi bir tuşa basılana kadar bilgi mesajı ile ekranı bekletir.
    /// </summary>
    private static void WaitingScreen()
    {
        Console.CursorVisible = false;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("⌛ Devam etmek için lütfen bir tuşa basınız");
        Console.ResetColor();
        Console.ReadKey();
        Console.CursorVisible = true;
    }
    /// <summary>
    /// Konsolda sabit bir süre boyunca basit bir dönen animasyon görüntüler.
    /// </summary>
    private static void SpinnerAnimation()
    {
        char[] characters = { '-', '\\', '|', '/' };

        int loopCounter = 0;
        int loopDuration = 50;

        Console.CursorVisible = false;

        while (loopCounter < 5)
        {
            foreach (char c in characters)
            {
                Console.Write("\r" + c);
                Thread.Sleep(loopDuration);
            }

            loopCounter++;

        }

        Console.CursorVisible = true;

    }

}