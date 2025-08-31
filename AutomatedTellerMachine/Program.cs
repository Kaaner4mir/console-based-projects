using System.Text;

class Program
{
    public static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        short attempt = 0;
        const int registeredPassword = 1111;

        attempt = UserCoontrol(attempt, registeredPassword);
    }

    
    private static short UserCoontrol(short attempt, int registeredPassword)
    {
        while (attempt < 5)
        {
            string input = GetStringInput("👤 Lütfen bankamıza kayıtlı 4 haneli kart şifrenizi giriniz : ");

            if (!int.TryParse(input, out int password))
            {
                Message("⛔ Geçersiz giriş! Lütfen sadece rakam giriniz.", ConsoleColor.Red);
            }
            else
            {
                if (registeredPassword == password)
                {
                    Message("✅ Şifre doğrulama başarılı!", ConsoleColor.Green);
                    WaitingScreen();
                    break;
                }
                else
                {
                    Message($"❌ Hatalı şifre girdiniz! Kalan deneme hakkınız : {4 - attempt}", ConsoleColor.Yellow);
                    attempt++;
                }
            }
        }

        if (attempt == 5)
        {
            Message("🚫 5 kez hatalı giriş yaptınız. Güvenliğiniz için kartınıza geçici olarak bloke konulmuştur! \n" +
                "Lütfen en yakın sürede müşteri temsilcimiz ile iletişime geçiniz.", ConsoleColor.Red);
            WaitingScreen(ConsoleColor.DarkRed);
        }

        return attempt;
    }

    private static void SpinnerAnimation()
    {
        Console.Clear();
        char[] chars = { '-', '\\', '|', '/' };
        short loopCounter = 0;
        const short loopDuration = 50;

        Console.CursorVisible = false;
        while (loopCounter < 5)
        {
            foreach (var item in chars)
            {
                Console.Write($"\r{item}");
                Thread.Sleep(loopDuration);
            }
            loopCounter++;
        }
        Console.CursorVisible = true;
        Console.Clear();
    }

    private static void WaitingScreen(ConsoleColor color = ConsoleColor.Yellow)
    {
        Console.CursorVisible = false;
        Console.ForegroundColor = color;
        Console.WriteLine("\n⌛ Devam etmek için bir tuşa basınız");
        Console.ResetColor();
        Console.ReadKey();
        Console.CursorVisible = true;
    }

    private static void Message(string message, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"\n{message}");
        Console.ResetColor();
    }

    private static string GetStringInput(string message, ConsoleColor color = ConsoleColor.White)
    {
        while (true)
        {
            Console.ForegroundColor = color;
            Console.Write($"\n{message}");
            string? text = Console.ReadLine();
            Console.ResetColor();

            if (!string.IsNullOrWhiteSpace(text))
                return text;
        }
    }
}
