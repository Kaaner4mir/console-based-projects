using System;
using System.Text;

class Program
{
    /// <summary>
    /// Programın giriş noktası. Sonsuz döngü ile kullanıcıdan işlem alır ve hesaplama yapar.
    /// </summary>
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        while (true)
        {
            DisplayMenu();

            Console.Write("\n🔎 Yapmak istediğiniz işlemi numerik olarak giriniz (1-14): ");
            if (!ushort.TryParse(Console.ReadLine(), out ushort act)) { Invalid(); continue; }

            switch (act)
            {
                case 1: PerformBinaryOperation("Toplama", (a, b) => a + b); break;
                case 2: PerformBinaryOperation("Çıkarma", (a, b) => a - b); break;
                case 3: PerformBinaryOperation("Çarpma", (a, b) => a * b); break;
                case 4: PerformBinaryOperation("Bölme", (a, b) => a / b, divisionCheck: true); break;
                case 5: PerformBinaryOperation("Üs alma", Math.Pow); break;
                case 6: PerformBinaryOperation("Kök alma", (radicand, index) => Math.Pow(radicand, 1.0 / index), rootCheck: true); break;
                case 7: Factorial(); break;
                default: Invalid(); break;
            }

            WaitingScreen();
        }
    }

    #region Matematiksel Operasyonlar

    /// <summary>
    /// İki sayılı işlemleri yapan genel fonksiyon.
    /// </summary>
    /// <param name="name">İşlemin adı (Toplama, Çıkarma vb.)</param>
    /// <param name="operation">İşlemi yapan fonksiyon</param>
    /// <param name="divisionCheck">Bölme kontrolü yapacaksa true</param>
    /// <param name="rootCheck">Kök işlemi kontrolü yapacaksa true</param>
    private static void PerformBinaryOperation(string name, Func<double, double, double> operation, bool divisionCheck = false, bool rootCheck = false)
    {
        try
        {
            double a = GetDoubleInput("\nBirinci sayıyı giriniz: ");
            double b = GetDoubleInput("\nİkinci sayıyı giriniz: ");

            if (divisionCheck)
            {
                if (a == 0 && b == 0) throw new Exception("0 / 0 işlemi tanımsızdır!");
                if (b == 0) throw new DivideByZeroException("Bölen 0 olamaz!");
            }

            if (rootCheck && b % 2 == 0 && a < 0)
                throw new Exception("Kök derecesi çift ise kök içi negatif olamaz!");

            double result = operation(a, b);

            if (double.IsInfinity(result) || double.IsNaN(result))
                throw new OverflowException("Sonuç sınırı aştı veya tanımsız!");

            Console.WriteLine(ShowResult(result));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ShowException(ex));
        }
    }

    /// <summary>
    /// Kullanıcının girdiği sayının faktöriyelini hesaplar ve ekrana yazdırır.
    /// </summary>
    private static void Factorial()
    {
        try
        {
            double number = GetDoubleInput("\nFaktöriyelini hesaplamak istediğiniz sayıyı giriniz: ");

            if (number < 0) throw new Exception("Girilen sayı negatif olamaz!");
            if (number != Math.Floor(number)) throw new Exception("Faktöriyel sadece tam sayılar için hesaplanabilir!");

            double result = 1;
            for (double i = 1; i <= number; i++)
                result *= i;

            if (double.IsInfinity(result))
                throw new OverflowException("Sonuç sınırı aştı!");

            Console.WriteLine(ShowResult(result));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ShowException(ex));
        }
    }

    #endregion

    #region Yardımcı Fonksiyonlar

    /// <summary>
    /// Kullanıcıdan double tipinde bir sayı alır.
    /// </summary>
    /// <param name="message">Kullanıcıya gösterilecek mesaj</param>
    /// <returns>Kullanıcının girdiği sayı</returns>
    private static double GetDoubleInput(string message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(message);
        if (!double.TryParse(Console.ReadLine(), out double value))
        {
            Invalid();
            throw new Exception("Geçersiz sayı girdiniz!");
        }
        return value;
    }

    /// <summary>
    /// İşlem sonucunu yeşil renkte gösterir.
    /// </summary>
    /// <param name="result">Sonuç değeri</param>
    /// <returns>Sonucu gösteren string</returns>
    private static string ShowResult(double result)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        return $"\n🟰 Sonuç: {result}";
    }

    /// <summary>
    /// Hata mesajını kırmızı renkte gösterir.
    /// </summary>
    /// <param name="ex">Hata nesnesi</param>
    /// <returns>Hata mesajı</returns>
    private static string ShowException(Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        return $"\n⚠️ Hata: {ex.Message}";
    }

    /// <summary>
    /// Menüde her bir matematiksel işlem için numara ve ismi gösterir.
    /// </summary>
    /// <param name="color">Renk</param>
    /// <param name="number">Numara</param>
    /// <param name="action">İşlem adı</param>
    private static void Operation(ConsoleColor color, string number, string action)
    {
        Console.ForegroundColor = color;
        Console.Write($"{number}. ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(action);
    }

    /// <summary>
    /// Kullanıcının görebileceği şekilde menüyü ekrana yazdırır.
    /// </summary>
    private static void DisplayMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(" << Hesap Makinesi >>\n");
        Operation(ConsoleColor.Red, " 1", "Toplama");
        Operation(ConsoleColor.Red, " 2", "Çıkarma");
        Operation(ConsoleColor.Red, " 3", "Çarpma");
        Operation(ConsoleColor.Red, " 4", "Bölme");
        Operation(ConsoleColor.Red, " 5", "Üs alma");
        Operation(ConsoleColor.Red, " 6", "Kök alma");
        Operation(ConsoleColor.Red, " 7", "Faktöriyel");
        Operation(ConsoleColor.Red, " 8", "Logaritma");
        Operation(ConsoleColor.Red, " 9", "Trigonometri");
        Operation(ConsoleColor.Red, "10", "Hafızaya ekleme");
        Operation(ConsoleColor.Red, "11", "Hafızadan çıkar");
        Operation(ConsoleColor.Red, "12", "Hafızayı göster");
        Operation(ConsoleColor.Red, "13", "Hafızayı sıfırla");
        Operation(ConsoleColor.Red, "14", "Çıkış");
    }

    /// <summary>
    /// Kullanıcı geçersiz bir işlem yaptığında uyarı verir.
    /// </summary>
    private static void Invalid()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n⚠️ Geçersiz bir işlem yaptınız!");
        Console.ResetColor();
    }

    /// <summary>
    /// Konsol ekranını bekletir, kullanıcıdan bir tuşa basmasını ister.
    /// </summary>
    private static void WaitingScreen()
    {
        Console.CursorVisible = false;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\n⌛ Devam etmek için bir tuşa basınız");
        Console.ResetColor();
        Console.ReadKey();
        Console.CursorVisible = true;
    }

    #endregion
}
