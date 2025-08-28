using System.Text;

class App
{
    static double _memory = 0;

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        while (true)
        {
            DisplayMenu();

            Console.Write("\nYapmak istediğiniz işlemi numerik olarak giriniz (1-?) : ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || !short.TryParse(input, out short inputOperation)) 
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
                case 10: Trigonometry(); break;
                case 11: MemoryTransaction(); break;
                case 12: Exit(); break;
                default: Message(ConsoleColor.Red, ExceptionMessage("Geçersiz seçim!")); break;
            }

            WaitingScreen();
        }
    }

    #region Matematiksel işlemler

    /// <summary>
    /// Üslü sayı işlemini gerçekleştirir. Kullanıcıdan taban ve üs değerlerini alır,
    /// matematiksel validasyonları yapar ve sonucu döndürür.
    /// </summary>
    /// <param name="operation">Üs alma işlemini gerçekleştiren delegate fonksiyon (Math.Pow)</param>
    /// <returns>Hesaplanan üs değeri. Hata durumunda double.NaN döner.</returns>
    /// <remarks>
    /// Validasyonlar:
    /// - 0^0 tanımsız olduğu için hata verir
    /// - Negatif sayının kesirli kuvveti tanımsız olduğu için hata verir
    /// - Sonuç sonsuz ise hata verir
    /// </remarks>
    private static double Exponentiation(Func<double, double, double> operation)
    {
        try
        {
            double baseNum = GetDoubleInput("⬇️ Taban sayıyı giriniz : ");
            double exponent = GetDoubleInput("⬆️ Kuvveti giriniz : ");

            if (baseNum == 0 && exponent == 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("0^0 TANIMSIZ!"));
                return double.NaN;
            }

            if (baseNum < 0 && exponent != Math.Floor(exponent))
            {
                Message(ConsoleColor.Red, ExceptionMessage("Negatif sayının kesirli kuvveti TANIMSIZ!"));
                return double.NaN;
            }

            double result = operation(baseNum, exponent);
            
            if (double.IsInfinity(result))
            {
                Message(ConsoleColor.Red, ExceptionMessage("Sonuç sonsuz!"));
                return double.NaN;
            }

            return result;
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"Üs alma işleminde hata: {ex.Message}"));
            return double.NaN;
        }
    }

    /// <summary>
    /// Pozitif tam sayının faktöriyelini hesaplar. Kullanıcıdan sayı alır,
    /// validasyonları yapar ve sonucu ekrana yazdırır.
    /// </summary>
    /// <remarks>
    /// Validasyonlar:
    /// - Negatif sayılar için hata verir
    /// - 20'den büyük sayılar için taşma riski nedeniyle hata verir
    /// - OverflowException durumunda özel hata mesajı gösterir
    /// 
    /// Performans: long veri tipi kullanılarak daha büyük sayılar desteklenir.
    /// </remarks>
    private static void Factorial()
    {
        try
        {
            int number = GetIntegerInput("Faktöriyelini öğrenmek istediğiniz sayıyı giriniz : ");

            if (number < 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("Girmiş olduğunuz sayı pozitif olmalıdır!"));
                return;
            }

            if (number > 20)
            {
                Message(ConsoleColor.Red, ExceptionMessage("20'den büyük sayılar için faktöriyel hesaplanamaz (taşma riski)!"));
                return;
            }

            long result = 1; // long kullanarak daha büyük sayıları destekle

            for (int i = number; i > 0; i--)
            {
                result *= i;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✅ İşleminin sonucu: {result}");
            Console.ResetColor();
        }
        catch (OverflowException)
        {
            Message(ConsoleColor.Red, ExceptionMessage("Sonuç çok büyük, hesaplanamıyor!"));
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"Faktöriyel işleminde hata: {ex.Message}"));
        }
    }

    /// <summary>
    /// Belirtilen taban ve argüman için logaritma hesaplar. Kullanıcıdan taban ve
    /// logaritması alınacak sayıyı alır, matematiksel validasyonları yapar.
    /// </summary>
    /// <param name="operation">Logaritma hesaplama fonksiyonu (Math.Log)</param>
    /// <returns>Hesaplanan logaritma değeri. Hata durumunda double.NaN döner.</returns>
    /// <remarks>
    /// Validasyonlar:
    /// - Taban pozitif olmalı ve 1 olamaz
    /// - Logaritması alınacak sayı pozitif olmalı
    /// - Sonuç sonsuz veya NaN ise hata verir
    /// </remarks>
    private static double Logarithm(Func<double, double, double> operation)
    {
        try
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
            
            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                Message(ConsoleColor.Red, ExceptionMessage("Geçersiz logaritma işlemi!"));
                return double.NaN;
            }

            return result;
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"Logaritma işleminde hata: {ex.Message}"));
            return double.NaN;
        }
    }

    /// <summary>
    /// Modulo (kalan) işlemini gerçekleştirir. Kullanıcıdan bölünen ve bölen
    /// değerlerini alır ve kalanı hesaplar.
    /// </summary>
    /// <param name="operation">Modulo işlemini gerçekleştiren delegate fonksiyon</param>
    /// <returns>Hesaplanan kalan değeri. Bölen 0 ise double.NaN döner.</returns>
    /// <remarks>
    /// Validasyonlar:
    /// - Bölen 0 olamaz (sıfıra bölme hatası)
    /// - Hata durumunda detaylı mesaj gösterir
    /// </remarks>
    private static double Modulus(Func<double, double, double> operation)
    {
        try
        {
            double dividend = GetDoubleInput("🔢 Bölünen sayıyı giriniz : ");
            double divisor = GetDoubleInput("✂️ Bölen sayıyı giriniz  : ");

            if (divisor == 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("Bölen 0 olamaz!"));
                return double.NaN;
            }

            double result = operation(dividend, divisor);
            return result;
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"Mod işleminde hata: {ex.Message}"));
            return double.NaN;
        }
    }

    /// <summary>
    /// Temel matematiksel işlemleri (toplama, çıkarma, çarpma, bölme) gerçekleştirir.
    /// Kullanıcıdan iki sayı alır ve belirtilen işlemi uygular.
    /// </summary>
    /// <param name="operationName">İşlemin adı (hata mesajları için kullanılır)</param>
    /// <param name="operation">Gerçekleştirilecek matematiksel işlem delegate'i</param>
    /// <returns>İşlem sonucu. Hata durumunda double.NaN döner.</returns>
    /// <remarks>
    /// Validasyonlar:
    /// - Bölme işleminde bölen 0 kontrolü
    /// - Sonuç sonsuz ise hata verir
    /// - Genel hata yakalama ve raporlama
    /// </remarks>
    private static double PerformOperation(string operationName, Func<double, double, double> operation)
    {
        try
        {
            double val1 = GetDoubleInput("➡️ Lütfen birinci sayıyı giriniz : ");
            double val2 = GetDoubleInput("➡️ Lütfen ikinci sayıyı giriniz : ");

            if (operationName.ToLower() == "bölme" && val2 == 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("Bölen 0 olamaz"));
                return double.NaN;
            }

            double result = operation(val1, val2);
            
            if (double.IsInfinity(result))
            {
                Message(ConsoleColor.Red, ExceptionMessage("Sonuç sonsuz!"));
                return double.NaN;
            }

            return result;
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"{operationName} işleminde hata: {ex.Message}"));
            return double.NaN;
        }
    }

    /// <summary>
    /// Köklü sayı işlemini gerçekleştirir. Kullanıcıdan kök içi ve kök derecesini alır,
    /// matematiksel validasyonları yapar ve sonucu döndürür.
    /// </summary>
    /// <param name="operation">Kök alma işlemini gerçekleştiren delegate fonksiyon</param>
    /// <returns>Hesaplanan kök değeri. Hata durumunda double.NaN döner.</returns>
    /// <remarks>
    /// Validasyonlar:
    /// - Çift dereceli kök için negatif sayı tanımsız
    /// - 0^0 tanımsız
    /// - Negatif sayının kesirli kökü tanımsız
    /// - Kök derecesi 0 olamaz
    /// - Sonuç sonsuz veya NaN ise hata verir
    /// </remarks>
    private static double Root(Func<double, double, double> operation)
    {
        try
        {
            double radicand = GetDoubleInput("↘️ Kök içini giriniz : ");
            double expansion = GetDoubleInput("↖️ Kök derecesini giriniz : ");

            if (radicand < 0 && expansion % 2 == 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("Çift dereceli kök için negatif sayı TANIMSIZ!"));
                return double.NaN;
            }
            else if (radicand == 0 && expansion == 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("0^0 TANIMSIZ!"));
                return double.NaN;
            }
            else if (radicand < 0 && expansion % 1 != 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("Negatif sayının kesirli kökü TANIMSIZ!"));
                return double.NaN;
            }
            else if (expansion == 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("Kök derecesi 0 olamaz!"));
                return double.NaN;
            }

            double result = operation(radicand, expansion);
            
            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                Message(ConsoleColor.Red, ExceptionMessage("Geçersiz kök işlemi!"));
                return double.NaN;
            }

            return result;
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"Kök işleminde hata: {ex.Message}"));
            return double.NaN;
        }
    }

    /// <summary>
    /// Mevcut hafıza değerini konsola yazdırır. Yeşil renkte görüntülenir.
    /// </summary>
    /// <remarks>
    /// Hafıza değeri global _memory değişkeninden okunur.
    /// Renk formatı: Yeşil metin, sonra normal renge dönüş.
    /// </remarks>
    private static void ShowMemory()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nHafıza : {_memory}");
        Console.ResetColor();
    }

    /// <summary>
    /// Trigonometrik fonksiyonları (sin, cos, tan, cot, sec, csc) hesaplar.
    /// Kullanıcıdan derece cinsinden açı alır, radyana çevirir ve seçilen
    /// trigonometrik fonksiyonu uygular.
    /// </summary>
    /// <remarks>
    /// Özellikler:
    /// - Derece cinsinden giriş, radyan cinsinden hesaplama
    /// - Tanımsız değerler için özel kontroller (90°, 0° gibi)
    /// - 6 farklı trigonometrik fonksiyon desteği
    /// - Hassas sıfır kontrolü (1e-10 tolerans)
    /// 
    /// Tanımsız durumlar:
    /// - Tan(90°), Cot(0°), Sec(90°), Csc(0°)
    /// </remarks>
    private static void Trigonometry()
    {
        try
        {
            DisplayTrigonometryMenu();

            int choice = GetIntegerInput("Yapmak istediğiniz trigonometrik işlemi numerik olarak seçiniz (1-6) : ");
            double degree = GetDoubleInput("Yapmak istediğiniz trigonometrik işlem için derece giriniz : ");
            double radian = degree * (Math.PI / 180);

            string funcName = "";
            double result;

            switch (choice)
            {
                case 1:
                    result = Math.Sin(radian);
                    funcName = "Sin";
                    break;
                case 2:
                    result = Math.Cos(radian);
                    funcName = "Cos";
                    break;
                case 3:
                    if (Math.Abs(Math.Cos(radian)) < 1e-10) // Cos ≈ 0 kontrolü
                    {
                        Message(ConsoleColor.Red, ExceptionMessage("Tan(90°) TANIMSIZ!"));
                        return;
                    }
                    result = Math.Tan(radian);
                    funcName = "Tan";
                    break;
                case 4:
                    if (Math.Abs(Math.Sin(radian)) < 1e-10) // Sin ≈ 0 kontrolü
                    {
                        Message(ConsoleColor.Red, ExceptionMessage("Cot(0°) TANIMSIZ!"));
                        return;
                    }
                    result = 1.0 / Math.Tan(radian);
                    funcName = "Cot";
                    break;
                case 5:
                    if (Math.Abs(Math.Cos(radian)) < 1e-10) // Cos ≈ 0 kontrolü
                    {
                        Message(ConsoleColor.Red, ExceptionMessage("Sec(90°) TANIMSIZ!"));
                        return;
                    }
                    result = 1.0 / Math.Cos(radian);
                    funcName = "Sec";
                    break;
                case 6:
                    if (Math.Abs(Math.Sin(radian)) < 1e-10) // Sin ≈ 0 kontrolü
                    {
                        Message(ConsoleColor.Red, ExceptionMessage("Csc(0°) TANIMSIZ!"));
                        return;
                    }
                    result = 1.0 / Math.Sin(radian);
                    funcName = "Csc";
                    break;
                default:
                    Message(ConsoleColor.Red, ExceptionMessage("Geçersiz trigonometrik işlem!"));
                    return;
            }

            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                Message(ConsoleColor.Red, ExceptionMessage($"{funcName}({degree}°) TANIMSIZ!"));
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✅ {funcName}({degree}°) = {result}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"Trigonometrik işlemde hata: {ex.Message}"));
        }
    }

    /// <summary>
    /// Hafıza işlemlerini yönetir. Kullanıcıya 4 seçenek sunar: hafızaya ekleme,
    /// hafızadan çıkarma, hafızayı görüntüleme ve hafızayı sıfırlama.
    /// </summary>
    /// <remarks>
    /// İşlemler:
    /// 1. Hafızaya ekle: Mevcut hafıza değerine sayı ekler
    /// 2. Hafızadan çıkar: Mevcut hafıza değerinden sayı çıkarır
    /// 3. Hafızayı getir: Mevcut hafıza değerini gösterir
    /// 4. Hafızayı sıfırla: Hafıza değerini 0 yapar
    /// 
    /// Hafıza değeri global _memory değişkeninde saklanır.
    /// </remarks>
    private static void MemoryTransaction()
    {
        try
        {
            DisplayMemoryTransaction();

            int choice = GetIntegerInput("Yapmak istediğiniz işlemi numerik olarak seçiniz (1-4) : ");

            switch (choice)
            {
                case 1:
                    double addAmount = GetDoubleInput("Sayıyı giriniz : ");
                    _memory += addAmount;
                    ShowMemory();
                    break;
                case 2:
                    double subAmount = GetDoubleInput("Sayıyı giriniz : ");
                    _memory -= subAmount;
                    ShowMemory();
                    break;
                case 3:
                    ShowMemory();
                    break;
                case 4:
                    _memory = 0;
                    ShowMemory();
                    break;
                default:
                    Message(ConsoleColor.Red, ExceptionMessage("Geçersiz seçim!"));
                    break;
            }
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"Hafıza işleminde hata: {ex.Message}"));
        }
    }

    #endregion

    #region Diğer işlemler

    /// <summary>
    /// Kullanıcıdan double türünde sayısal değer alır. Geçersiz girişler için
    /// tekrar giriş ister ve null/boş girişleri kontrol eder.
    /// </summary>
    /// <param name="message">Kullanıcıya gösterilecek prompt mesajı</param>
    /// <returns>Kullanıcının girdiği geçerli double değeri</returns>
    /// <remarks>
    /// Validasyonlar:
    /// - Null veya boş giriş kontrolü
    /// - Double.TryParse ile sayısal değer kontrolü
    /// - Hata durumunda kullanıcıya açıklayıcı mesaj
    /// - Sonsuz döngü ile geçerli değer alınana kadar devam eder
    /// </remarks>
    private static double GetDoubleInput(string message)
    {
        while (true)
        {
            Console.Write($"\n{message}");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
            {
                Message(ConsoleColor.Red, "Lütfen bir değer giriniz!");
                continue;
            }

            if (double.TryParse(input, out double value))
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
    /// Kullanıcıdan integer türünde tam sayı değer alır. Geçersiz girişler için
    /// tekrar giriş ister ve null/boş girişleri kontrol eder.
    /// </summary>
    /// <param name="message">Kullanıcıya gösterilecek prompt mesajı</param>
    /// <returns>Kullanıcının girdiği geçerli integer değeri</returns>
    /// <remarks>
    /// Validasyonlar:
    /// - Null veya boş giriş kontrolü
    /// - Int.TryParse ile tam sayı kontrolü
    /// - Hata durumunda kullanıcıya açıklayıcı mesaj
    /// - Sonsuz döngü ile geçerli değer alınana kadar devam eder
    /// </remarks>
    private static int GetIntegerInput(string message)
    {
        while (true)
        {
            Console.Write($"\n{message}");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
            {
                Message(ConsoleColor.Red, "Lütfen bir değer giriniz!");
                continue;
            }

            if (int.TryParse(input, out int value))
            {
                return value;
            }
            else
            {
                Message(ConsoleColor.Red, "Lütfen geçerli bir tam sayı giriniz!");
            }
        }
    }

    /// <summary>
    /// Hafıza işlemleri için alt menüyü konsola yazdırır. 4 farklı hafıza
    /// işlemi seçeneği sunar ve konsolu temizler.
    /// </summary>
    /// <remarks>
    /// Menü seçenekleri:
    /// 1. Hafızaya ekle
    /// 2. Hafızadan çıkar
    /// 3. Hafızayı getir
    /// 4. Hafızayı sıfırla
    /// 
    /// İşlem: Console.Clear() ile ekranı temizler, sonra menüyü yazdırır.
    /// </remarks>
    private static void DisplayMemoryTransaction()
    {
        Console.Clear();

        Operation("1", "Hafızaya ekle");
        Operation("2", "Hafızadan çıkar");
        Operation("3", "Hafızayı getir");
        Operation("4", "Hafızayı sıfırla");
    }

    /// <summary>
    /// Ana hesap makinesi menüsünü konsola yazdırır. 12 farklı matematiksel
    /// işlem seçeneği sunar ve konsolu temizler.
    /// </summary>
    /// <remarks>
    /// Menü seçenekleri:
    /// 1-4: Temel işlemler (toplama, çıkarma, çarpma, bölme)
    /// 5-6: Üs ve kök alma
    /// 7: Faktöriyel
    /// 8: Mod alma
    /// 9: Logaritma
    /// 10: Trigonometri
    /// 11: Hafıza işlemleri
    /// 12: Çıkış
    /// 
    /// Her seçenek emoji ile görsel olarak desteklenir.
    /// </remarks>
    private static void DisplayMenu()
    {
        Console.Clear();

        Operation(" 1", "Toplama           ➕");
        Operation(" 2", "Çıkarma           ➖");
        Operation(" 3", "Çarpma            ✖️");
        Operation(" 4", "Bölme             ➗");
        Operation(" 5", "Üs alma           xⁿ");
        Operation(" 6", "Kök alma          ⁿ√x");
        Operation(" 7", "Faktöriyel        ❗");
        Operation(" 8", "Mod alma           %");
        Operation(" 9", "Logaritma        logx(y)");
        Operation("10", "Trigonometri       📐");
        Operation("11", "Hafıza işlemleri   🧠");
        Operation("12", "Exit               🔚");
    }

    /// <summary>
    /// Trigonometrik fonksiyonlar için alt menüyü konsola yazdırır. 6 farklı
    /// trigonometrik fonksiyon seçeneği sunar ve konsolu temizler.
    /// </summary>
    /// <remarks>
    /// Menü seçenekleri:
    /// 1. Sine (sin)
    /// 2. Cosine (cos)
    /// 3. Tangent (tan)
    /// 4. Cotangent (cot)
    /// 5. Secant (sec)
    /// 6. Cosecant (csc)
    /// 
    /// İşlem: Console.Clear() ile ekranı temizler, sonra menüyü yazdırır.
    /// </remarks>
    private static void DisplayTrigonometryMenu()
    {
        Console.Clear();

        Operation("1", "Sine");
        Operation("2", "Cosine");
        Operation("3", "Tangent");
        Operation("4", "Cotangent");
        Operation("5", "Secant");
        Operation("6", "Cosecant");
    }

    /// <summary>
    /// Standart hata mesajı formatı oluşturur. Verilen mesajı uyarı emojisi
    /// ve standart format ile birleştirir.
    /// </summary>
    /// <param name="exceptionMessage">Hata mesajının içeriği</param>
    /// <returns>Formatlanmış hata mesajı string'i</returns>
    /// <remarks>
    /// Format: "\n⚠️ Bir hata oluştu : {exceptionMessage}"
    /// 
    /// Kullanım: Tüm hata mesajları için tutarlı format sağlar.
    /// </remarks>
    private static string ExceptionMessage(string exceptionMessage)
    {
        return $"\n⚠️ Bir hata oluştu : {exceptionMessage}";
    }

    /// <summary>
    /// Uygulamadan çıkış işlemini gerçekleştirir. Kullanıcıya onay sorar
    /// ve yanıta göre uygulamayı kapatır veya devam eder.
    /// </summary>
    /// <remarks>
    /// İşlem akışı:
    /// 1. Kullanıcıya "Çıkmak istediğinize emin misiniz (E/H) : " sorar
    /// 2. "E" veya "e" girilirse Environment.Exit(0) ile uygulamayı kapatır
    /// 3. "H" veya "h" girilirse işlemi iptal eder
    /// 4. Geçersiz giriş için hata mesajı gösterir
    /// 
    /// Null/boş giriş kontrolü yapılır.
    /// </remarks>
    private static void Exit()
    {
        Console.Write("\nÇıkmak istediğinize emin misiniz (E/H) : ");
        string? act = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(act))
        {
            Message(ConsoleColor.Red, ExceptionMessage("Geçersiz bir işlem yaptınız!")); 
            return; 
        }
        else
        {
            if (act.ToLower() == "e")
                Environment.Exit(0);
            else if (act.ToLower() == "h")
                return;
            else
                Message(ConsoleColor.Red, ExceptionMessage("Geçersiz bir işlem yaptınız!"));
        }
    }

    /// <summary>
    /// Konsola belirtilen renkte mesaj yazdırır. Mesajı yazdıktan sonra
    /// konsol rengini varsayılan haline döndürür.
    /// </summary>
    /// <param name="color">Mesajın yazılacağı konsol rengi</param>
    /// <param name="message">Yazılacak mesaj içeriği</param>
    /// <remarks>
    /// İşlem sırası:
    /// 1. Console.ForegroundColor = color ile rengi ayarlar
    /// 2. Console.WriteLine(message) ile mesajı yazdırır
    /// 3. Console.ResetColor() ile rengi varsayılan haline döndürür
    /// 
    /// Kullanım: Hata mesajları (kırmızı), başarı mesajları (yeşil) için.
    /// </remarks>
    private static void Message(ConsoleColor color, string message)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// <summary>
    /// Menü seçeneklerini konsola yazdırır. Sıra numarasını kırmızı, işlem
    /// adını beyaz renkte gösterir.
    /// </summary>
    /// <param name="queue">Sıra numarası (kırmızı renkte gösterilir)</param>
    /// <param name="operation">İşlem adı (beyaz renkte gösterilir)</param>
    /// <remarks>
    /// Format: "{queue}. {operation}"
    /// 
    /// Renk düzeni:
    /// - Sıra numarası: ConsoleColor.Red
    /// - İşlem adı: ConsoleColor.White
    /// 
    /// Kullanım: Tüm menü seçenekleri için tutarlı görünüm sağlar.
    /// </remarks>
    private static void Operation(string queue, string operation)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{queue}. ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{operation}");
    }

    /// <summary>
    /// Matematiksel işlem sonucunu konsola yazdırır. NaN değerleri için
    /// özel hata mesajı gösterir, geçerli sonuçlar için yeşil renkte
    /// başarı mesajı yazdırır.
    /// </summary>
    /// <param name="result">Gösterilecek işlem sonucu</param>
    /// <remarks>
    /// Kontroller:
    /// - double.IsNaN(result) kontrolü
    /// - NaN ise kırmızı renkte "❌ İşlem başarısız!" mesajı
    /// - Geçerli sonuç ise yeşil renkte "✅ İşleminin sonucu: {result}"
    /// 
    /// Kullanım: Tüm matematiksel işlemlerin sonuçlarını göstermek için.
    /// </remarks>
    private static void ShowResult(double result)
    {
        if (double.IsNaN(result))
        {
            Message(ConsoleColor.Red, "\n❌ İşlem başarısız!");
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n✅ İşleminin sonucu: {result}");
        Console.ResetColor();
    }

    /// <summary>
    /// Kullanıcıdan herhangi bir tuşa basmasını bekler. Ekranda sarı renkte
    /// bilgilendirme mesajı gösterir ve tuş basılana kadar bekler.
    /// </summary>
    /// <remarks>
    /// İşlem sırası:
    /// 1. Console.CursorVisible = false ile imleci gizler
    /// 2. Sarı renkte "⌛ Devam etmek için lütfen bir tuşa basınız" mesajı
    /// 3. Console.ReadKey() ile tuş bekler
    /// 4. Console.CursorVisible = true ile imleci tekrar gösterir
    /// 5. SpinnerAnimation() ile geçiş animasyonu çalıştırır
    /// 
    /// Kullanım: Her işlem sonrası kullanıcının sonucu görmesi için bekletme.
    /// </remarks>
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
    /// Konsolda basit bir dönen animasyon gösterir. 4 farklı karakter
    /// (-, \, |, /) kullanarak dönen efekt oluşturur.
    /// </summary>
    /// <remarks>
    /// Animasyon detayları:
    /// - 4 karakter: '-', '\\', '|', '/'
    /// - Her karakter 50ms gösterilir
    /// - Toplam 5 döngü (20 karakter değişimi)
    /// - Toplam süre: 1 saniye
    /// 
    /// İşlem:
    /// 1. Console.Clear() ile ekranı temizler
    /// 2. Console.CursorVisible = false ile imleci gizler
    /// 3. Döngü ile karakterleri sırayla gösterir
    /// 4. Console.CursorVisible = true ile imleci tekrar gösterir
    /// 
    /// Kullanım: İşlemler arası geçiş için görsel efekt.
    /// </remarks>
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

    #endregion
}
