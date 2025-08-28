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

            Console.Write("\nYapmak istediğiniz işlemi numerik olarak giriniz (1-?) : ");
            if (!short.TryParse(Console.ReadLine(), out short inputOperation))
            {
                Message(ConsoleColor.Red, ExceptionMessage("Geçersiz bir işlem yaptınız!"));
                continue;
            }

            switch (inputOperation)
            {
                case 1: { double result = PerformOperation("Toplama", (val1, val2) => val1 + val2); ShowResult(result); break; }
                case 2: { double result = PerformOperation("Çıkarma", (val1, val2) => val1 - val2); ShowResult(result); break; }
                case 3: { double result = PerformOperation("Çarpma", (val1, val2) => val1 * val2); ShowResult(result); break; }
                case 4: { double result = PerformOperation("Bölme", (val1, val2) => val1 / val2); ShowResult(result); break; }
                case 5: { double result = Exponentiation((baseNum, exponent) => Math.Pow(baseNum, exponent)); ShowResult(result); break; }
                case 6: { double result = Root((radicand, expansion) => Math.Pow(radicand, 1.0 / expansion)); ShowResult(result); break; }
                case 7: Factorial(); break;
                case 8: { double result = Modulus((dividend, divisor) => dividend % divisor); ShowResult(result); break; }
                case 9: { double result = Logarithm((baseNum, argument) => Math.Log(baseNum, argument)); ShowResult(result); break; }
                default: Message(ConsoleColor.Red, ExceptionMessage("Geçersiz seçim!")); break;
            }

            WaitingScreen();

        }
    }

    #region Matematiksel işlemler

    /// <summary>
    /// Üslü sayı işlemini yapar.
    /// </summary>
    /// <param name="operation"></param>
    /// <returns></returns>
    private static double Exponentiation(Func<double, double, double> operation)
    {
        double baseNum = GetDoubleInput("⬇️ Taban sayıyı giriniz : ");
        double exponent = GetDoubleInput("⬆️ Kuvveti giriniz : ");

        double result = operation(baseNum, exponent);

        if (baseNum == 0 && exponent == 0)
        {
            Message(ConsoleColor.Red, ExceptionMessage("TANIMSIZ!"));
            return double.NaN;
        }

        return result;

    }

    /// <summary>
    /// Faktöriyel işlemlerini yapar.
    /// </summary>
    private static void Factorial()
    {
        try
        {
            int number = GetIntegerInput("Faktöriyelini öğrenmek istediğiniz sayıyı giriniz : ");

            int result = 1;

            if (number < 0)
                Message(ConsoleColor.Red, ExceptionMessage("Girmiş olduğunuz sayı pozitif olmalıdır!"));
            else

                for (int i = number; i > 0; i--)
                {
                    result *= i;
                }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✅ İşleminin sonucu: {result}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            ExceptionMessage(ex.Message);
        }

    }

    private static double Logarithm(Func<double, double, double> operation)
    {
        double baseNum = GetDoubleInput("⬇️ Taban sayıyı giriniz : ");
        double argument = GetDoubleInput("⬆️ Logaritması alınacak sayıyı giriniz : ");

        if (baseNum <= 0 || baseNum == 1)
        {
            Message(ConsoleColor.Red, ExceptionMessage("Taban pozitif olmalı ve 1 olamaz!"));
            return double.NaN;
        }

        if (argument <= 0)
        {
            Message(ConsoleColor.Red, ExceptionMessage("Logaritma alınacak sayı pozitif olmalı!"));
            return double.NaN;
        }

        double result = operation(argument, baseNum);
        return result;
    }


    /// <summary>
    /// Mod işlemlerini yapar.
    /// </summary>
    private static double Modulus(Func<double, double, double> operation)
    {
        double dividend = GetDoubleInput("🔢 Bölünen sayıyı giriniz : ");
        double divisor = GetDoubleInput("✂️ Bölen sayıyı giriniz  : ");

        double result = operation(dividend, divisor);

        if (divisor == 0)
        {
            Message(ConsoleColor.Red, ExceptionMessage("Bölen 0 olamaz!"));
            return double.NaN;
        }

        return result;
    }

    /// <summary>
    /// Matematiksel işlem yapar.
    /// </summary>
    private static double PerformOperation(string operationName, Func<double, double, double> operation)
    {

        double val1 = GetDoubleInput("➡️ Lütfen birinci sayıyı giriniz : ");
        double val2 = GetDoubleInput("➡️ Lütfen ikinci sayıyı giriniz : ");

        double result = operation(val1, val2);

        if (operationName.ToLower() == "bölme" && val2 == 0)
        {
            Message(ConsoleColor.Red, ExceptionMessage("Bölen 0 olamaz"));
            return double.NaN;
        }

        return result;
    }

    /// <summary>
    /// Köklü sayı işlemini yapar.
    /// </summary>
    private static double Root(Func<double, double, double> operation)
    {
        double radicand = GetDoubleInput("↘️ Kök içini giriniz : ");
        double expansion = GetDoubleInput("↖️ Kök derecesini giriniz : ");

        double result = operation(radicand, expansion);

        if (radicand < 0 && expansion % 2 == 0)
        {
            Message(ConsoleColor.Red, ExceptionMessage("TANIMSIZ!"));
            return double.NaN;
        }
        else if (radicand == 0 && expansion == 0)
        {
            Message(ConsoleColor.Red, ExceptionMessage("TANIMSIZ!"));
            return double.NaN;
        }
        else if (radicand < 0 && expansion % 1 != 0)
        {
            Message(ConsoleColor.Red, ExceptionMessage("TANIMSIZ!"));
            return double.NaN;
        }

        return result;
    }

    #endregion

    /// <summary>
    /// Kullanıcıdan double değer alır.
    /// </summary>
    private static double GetDoubleInput(string message)
    {
        while (true)
        {
            Console.Write($"\n{message}");
            if (double.TryParse(Console.ReadLine(), out double value))
            {
                return value;
            }
            else
            {
                Message(ConsoleColor.Red, "Lütfen geçerli bir sayı giriniz!");
            }
        }
    }

    /// <summary>
    /// Kullanıcıdan integer değer alır.
    /// </summary>
    private static int GetIntegerInput(string message)
    {
        while (true)
        {
            Console.Write($"\n{message}");
            if (int.TryParse(Console.ReadLine(), out int value))
            {
                return value;
            }
            else
            {
                Message(ConsoleColor.Red, "Lütfen geçerli bir sayı giriniz!");
            }
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
        Operation("5", "Üs alma      xⁿ");
        Operation("6", "Kök alma     ⁿ√x");
        Operation("7", "Faktöriyel   ❗");
        Operation("8", "Mod alma      %");
        Operation("9", "Logaritma   logx(y)");
    }

    /// <summary>
    /// Hata mesajı oluşturur.
    /// </summary>
    private static string ExceptionMessage(string exceptionMessage)
    {
        return $"\n⚠️ Bir hata oluştu : {exceptionMessage}";
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
        Console.WriteLine($"{operation}");
    }

    /// <summary>
    /// İşlem sonucunu ekrana yazar.
    /// </summary>
    private static void ShowResult(double result)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n✅ İşleminin sonucu: {result}");
        Console.ResetColor();
    }

    /// <summary>
    /// Kullanıcı tarafından herhangi bir tuşa basılana kadar bilgi mesajı ile ekranı bekletir.
    /// </summary>
    private static void WaitingScreen()
    {
        Console.CursorVisible = false;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("\n⌛ Devam etmek için lütfen bir tuşa basınız");
        Console.ResetColor();
        Console.ReadKey();
        Console.CursorVisible = true;
        SpinnerAnimation();
    }

    /// <summary>
    /// Konsolda sabit bir süre boyunca basit bir dönen animasyon görüntüler.
    /// </summary>
    private static void SpinnerAnimation()
    {
        Console.Clear();

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
