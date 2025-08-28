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

            Console.Write("\nEnter the operation you want to perform numerically (1-12) : ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || !short.TryParse(input, out short inputOperation)) 
            { 
                Message(ConsoleColor.Red, ExceptionMessage("You performed an invalid operation!")); 
                continue; 
            }

            switch (inputOperation)
            {
                case 1: { double result = PerformOperation("Addition", (val1, val2) => val1 + val2); ShowResult(result); break; }
                case 2: { double result = PerformOperation("Subtraction", (val1, val2) => val1 - val2); ShowResult(result); break; }
                case 3: { double result = PerformOperation("Multiplication", (val1, val2) => val1 * val2); ShowResult(result); break; }
                case 4: { double result = PerformOperation("Division", (val1, val2) => val1 / val2); ShowResult(result); break; }
                case 5: { double result = Exponentiation((baseNum, exponent) => Math.Pow(baseNum, exponent)); ShowResult(result); break; }
                case 6: { double result = Root((radicand, expansion) => Math.Pow(radicand, 1.0 / expansion)); ShowResult(result); break; }
                case 7: Factorial(); break;
                case 8: { double result = Modulus((dividend, divisor) => dividend % divisor); ShowResult(result); break; }
                case 9: { double result = Logarithm((baseNum, argument) => Math.Log(baseNum, argument)); ShowResult(result); break; }
                case 10: Trigonometry(); break;
                case 11: MemoryTransaction(); break;
                case 12: Exit(); break;
                default: Message(ConsoleColor.Red, ExceptionMessage("Invalid selection!")); break;
            }

            WaitingScreen();
        }
    }

    #region Mathematical Operations

    /// <summary>
    /// Performs exponentiation operation. Takes base and exponent values from user,
    /// performs mathematical validations and returns the result.
    /// </summary>
    /// <param name="operation">Delegate function that performs exponentiation (Math.Pow)</param>
    /// <returns>Calculated exponent value. Returns double.NaN in case of error.</returns>
    /// <remarks>
    /// Validations:
    /// - Returns error for 0^0 as it is undefined
    /// - Returns error for negative number with fractional power as it is undefined
    /// - Returns error if result is infinite
    /// </remarks>
    private static double Exponentiation(Func<double, double, double> operation)
    {
        try
        {
            double baseNum = GetDoubleInput("⬇️ Enter the base number : ");
            double exponent = GetDoubleInput("⬆️ Enter the exponent : ");

            if (baseNum == 0 && exponent == 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("0^0 UNDEFINED!"));
                return double.NaN;
            }

            if (baseNum < 0 && exponent != Math.Floor(exponent))
            {
                Message(ConsoleColor.Red, ExceptionMessage("Negative number with fractional power is UNDEFINED!"));
                return double.NaN;
            }

            double result = operation(baseNum, exponent);
            
            if (double.IsInfinity(result))
            {
                Message(ConsoleColor.Red, ExceptionMessage("Result is infinite!"));
                return double.NaN;
            }

            return result;
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"Error in exponentiation operation: {ex.Message}"));
            return double.NaN;
        }
    }

    /// <summary>
    /// Calculates the factorial of a positive integer. Takes a number from user,
    /// performs validations and prints the result to screen.
    /// </summary>
    /// <remarks>
    /// Validations:
    /// - Returns error for negative numbers
    /// - Returns error for numbers greater than 20 due to overflow risk
    /// - Shows special error message for OverflowException
    /// 
    /// Performance: Uses long data type to support larger numbers.
    /// </remarks>
    private static void Factorial()
    {
        try
        {
            int number = GetIntegerInput("Enter the number whose factorial you want to calculate : ");

            if (number < 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("The number you entered must be positive!"));
                return;
            }

            if (number > 20)
            {
                Message(ConsoleColor.Red, ExceptionMessage("Factorial cannot be calculated for numbers greater than 20 (overflow risk)!"));
                return;
            }

            long result = 1; // using long to support larger numbers

            for (int i = number; i > 0; i--)
            {
                result *= i;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✅ Operation result: {result}");
            Console.ResetColor();
        }
        catch (OverflowException)
        {
            Message(ConsoleColor.Red, ExceptionMessage("Result is too large, cannot be calculated!"));
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"Error in factorial operation: {ex.Message}"));
        }
    }

    /// <summary>
    /// Calculates logarithm for specified base and argument. Takes base and
    /// the number whose logarithm will be calculated from user, performs mathematical validations.
    /// </summary>
    /// <param name="operation">Logarithm calculation function (Math.Log)</param>
    /// <returns>Calculated logarithm value. Returns double.NaN in case of error.</returns>
    /// <remarks>
    /// Validations:
    /// - Base must be positive and cannot be 1
    /// - The number whose logarithm will be calculated must be positive
    /// - Returns error if result is infinite or NaN
    /// </remarks>
    private static double Logarithm(Func<double, double, double> operation)
    {
        try
        {
            double baseNum = GetDoubleInput("⬇️ Enter the base number : ");
            double argument = GetDoubleInput("⬆️ Enter the number whose logarithm will be calculated : ");

            if (baseNum <= 0 || baseNum == 1)
            {
                Message(ConsoleColor.Red, ExceptionMessage("Base must be positive and cannot be 1!"));
                return double.NaN;
            }

            if (argument <= 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("The number whose logarithm will be calculated must be positive!"));
                return double.NaN;
            }

            double result = operation(argument, baseNum);
            
            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                Message(ConsoleColor.Red, ExceptionMessage("Invalid logarithm operation!"));
                return double.NaN;
            }

            return result;
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"Error in logarithm operation: {ex.Message}"));
            return double.NaN;
        }
    }

    /// <summary>
    /// Performs modulo (remainder) operation. Takes dividend and divisor
    /// values from user and calculates the remainder.
    /// </summary>
    /// <param name="operation">Delegate function that performs modulo operation</param>
    /// <returns>Calculated remainder value. Returns double.NaN if divisor is 0.</returns>
    /// <remarks>
    /// Validations:
    /// - Divisor cannot be 0 (division by zero error)
    /// - Shows detailed message in case of error
    /// </remarks>
    private static double Modulus(Func<double, double, double> operation)
    {
        try
        {
            double dividend = GetDoubleInput("🔢 Enter the dividend : ");
            double divisor = GetDoubleInput("✂️ Enter the divisor : ");

            if (divisor == 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("Divisor cannot be 0!"));
                return double.NaN;
            }

            double result = operation(dividend, divisor);
            return result;
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"Error in mod operation: {ex.Message}"));
            return double.NaN;
        }
    }

    /// <summary>
    /// Performs basic mathematical operations (addition, subtraction, multiplication, division).
    /// Takes two numbers from user and applies the specified operation.
    /// </summary>
    /// <param name="operationName">Name of the operation (used for error messages)</param>
    /// <param name="operation">Mathematical operation delegate to be performed</param>
    /// <returns>Operation result. Returns double.NaN in case of error.</returns>
    /// <remarks>
    /// Validations:
    /// - Division by zero check for division operation
    /// - Returns error if result is infinite
    /// - General error catching and reporting
    /// </remarks>
    private static double PerformOperation(string operationName, Func<double, double, double> operation)
    {
        try
        {
            double val1 = GetDoubleInput("➡️ Please enter the first number : ");
            double val2 = GetDoubleInput("➡️ Please enter the second number : ");

            if (operationName.ToLower() == "division" && val2 == 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("Divisor cannot be 0"));
                return double.NaN;
            }

            double result = operation(val1, val2);
            
            if (double.IsInfinity(result))
            {
                Message(ConsoleColor.Red, ExceptionMessage("Result is infinite!"));
                return double.NaN;
            }

            return result;
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"Error in {operationName} operation: {ex.Message}"));
            return double.NaN;
        }
    }

    /// <summary>
    /// Performs root operation. Takes radicand and root degree from user,
    /// performs mathematical validations and returns the result.
    /// </summary>
    /// <param name="operation">Delegate function that performs root operation</param>
    /// <returns>Calculated root value. Returns double.NaN in case of error.</returns>
    /// <remarks>
    /// Validations:
    /// - Negative number is undefined for even degree root
    /// - 0^0 is undefined
    /// - Negative number with fractional root is undefined
    /// - Root degree cannot be 0
    /// - Returns error if result is infinite or NaN
    /// </remarks>
    private static double Root(Func<double, double, double> operation)
    {
        try
        {
            double radicand = GetDoubleInput("↘️ Enter the radicand : ");
            double expansion = GetDoubleInput("↖️ Enter the root degree : ");

            if (radicand < 0 && expansion % 2 == 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("Negative number is UNDEFINED for even degree root!"));
                return double.NaN;
            }
            else if (radicand == 0 && expansion == 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("0^0 UNDEFINED!"));
                return double.NaN;
            }
            else if (radicand < 0 && expansion % 1 != 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("Negative number with fractional root is UNDEFINED!"));
                return double.NaN;
            }
            else if (expansion == 0)
            {
                Message(ConsoleColor.Red, ExceptionMessage("Root degree cannot be 0!"));
                return double.NaN;
            }

            double result = operation(radicand, expansion);
            
            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                Message(ConsoleColor.Red, ExceptionMessage("Invalid root operation!"));
                return double.NaN;
            }

            return result;
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"Error in root operation: {ex.Message}"));
            return double.NaN;
        }
    }

    /// <summary>
    /// Prints the current memory value to console. Displayed in green color.
    /// </summary>
    /// <remarks>
    /// Memory value is read from global _memory variable.
    /// Color format: Green text, then return to normal color.
    /// </remarks>
    private static void ShowMemory()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nMemory : {_memory}");
        Console.ResetColor();
    }

    /// <summary>
    /// Calculates trigonometric functions (sin, cos, tan, cot, sec, csc).
    /// Takes angle in degrees from user, converts to radians and applies the selected
    /// trigonometric function.
    /// </summary>
    /// <remarks>
    /// Features:
    /// - Input in degrees, calculation in radians
    /// - Special controls for undefined values (90°, 0° etc.)
    /// - Support for 6 different trigonometric functions
    /// - Precise zero control (1e-10 tolerance)
    /// 
    /// Undefined cases:
    /// - Tan(90°), Cot(0°), Sec(90°), Csc(0°)
    /// </remarks>
    private static void Trigonometry()
    {
        try
        {
            DisplayTrigonometryMenu();

            int choice = GetIntegerInput("Select the trigonometric operation you want to perform numerically (1-6) : ");
            double degree = GetDoubleInput("Enter the degree for the trigonometric operation you want to perform : ");
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
                    if (Math.Abs(Math.Cos(radian)) < 1e-10) // Cos ≈ 0 check
                    {
                        Message(ConsoleColor.Red, ExceptionMessage("Tan(90°) UNDEFINED!"));
                        return;
                    }
                    result = Math.Tan(radian);
                    funcName = "Tan";
                    break;
                case 4:
                    if (Math.Abs(Math.Sin(radian)) < 1e-10) // Sin ≈ 0 check
                    {
                        Message(ConsoleColor.Red, ExceptionMessage("Cot(0°) UNDEFINED!"));
                        return;
                    }
                    result = 1.0 / Math.Tan(radian);
                    funcName = "Cot";
                    break;
                case 5:
                    if (Math.Abs(Math.Cos(radian)) < 1e-10) // Cos ≈ 0 check
                    {
                        Message(ConsoleColor.Red, ExceptionMessage("Sec(90°) UNDEFINED!"));
                        return;
                    }
                    result = 1.0 / Math.Cos(radian);
                    funcName = "Sec";
                    break;
                case 6:
                    if (Math.Abs(Math.Sin(radian)) < 1e-10) // Sin ≈ 0 check
                    {
                        Message(ConsoleColor.Red, ExceptionMessage("Csc(0°) UNDEFINED!"));
                        return;
                    }
                    result = 1.0 / Math.Sin(radian);
                    funcName = "Csc";
                    break;
                default:
                    Message(ConsoleColor.Red, ExceptionMessage("Invalid trigonometric operation!"));
                    return;
            }

            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                Message(ConsoleColor.Red, ExceptionMessage($"{funcName}({degree}°) UNDEFINED!"));
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✅ {funcName}({degree}°) = {result}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"Error in trigonometric operation: {ex.Message}"));
        }
    }

    /// <summary>
    /// Manages memory operations. Offers 4 options to user: add to memory,
    /// subtract from memory, display memory and reset memory.
    /// </summary>
    /// <remarks>
    /// Operations:
    /// 1. Add to memory: Adds number to current memory value
    /// 2. Subtract from memory: Subtracts number from current memory value
    /// 3. Get memory: Shows current memory value
    /// 4. Clear memory: Sets memory value to 0
    /// 
    /// Memory value is stored in global _memory variable.
    /// </remarks>
    private static void MemoryTransaction()
    {
        try
        {
            DisplayMemoryTransaction();

            int choice = GetIntegerInput("Select the operation you want to perform numerically (1-4) : ");

            switch (choice)
            {
                case 1:
                    double addAmount = GetDoubleInput("Enter the number : ");
                    _memory += addAmount;
                    ShowMemory();
                    break;
                case 2:
                    double subAmount = GetDoubleInput("Enter the number : ");
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
                    Message(ConsoleColor.Red, ExceptionMessage("Invalid selection!"));
                    break;
            }
        }
        catch (Exception ex)
        {
            Message(ConsoleColor.Red, ExceptionMessage($"Error in memory operation: {ex.Message}"));
        }
    }

    #endregion

    #region Other Operations

    /// <summary>
    /// Takes numeric value of double type from user. Requests re-entry for invalid inputs
    /// and checks for null/empty inputs.
    /// </summary>
    /// <param name="message">Prompt message to be shown to user</param>
    /// <returns>Valid double value entered by user</returns>
    /// <remarks>
    /// Validations:
    /// - Null or empty input check
    /// - Numeric value check with Double.TryParse
    /// - Explanatory message to user in case of error
    /// - Continues with infinite loop until valid value is obtained
    /// </remarks>
    private static double GetDoubleInput(string message)
    {
        while (true)
        {
            Console.Write($"\n{message}");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
            {
                Message(ConsoleColor.Red, "Please enter a value!");
                continue;
            }

            if (double.TryParse(input, out double value))
            {
                return value;
            }
            else
            {
                Message(ConsoleColor.Red, "Please enter a valid number!");
            }
        }
    }

    /// <summary>
    /// Takes integer value from user. Requests re-entry for invalid inputs
    /// and checks for null/empty inputs.
    /// </summary>
    /// <param name="message">Prompt message to be shown to user</param>
    /// <returns>Valid integer value entered by user</returns>
    /// <remarks>
    /// Validations:
    /// - Null or empty input check
    /// - Integer check with Int.TryParse
    /// - Explanatory message to user in case of error
    /// - Continues with infinite loop until valid value is obtained
    /// </remarks>
    private static int GetIntegerInput(string message)
    {
        while (true)
        {
            Console.Write($"\n{message}");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
            {
                Message(ConsoleColor.Red, "Please enter a value!");
                continue;
            }

            if (int.TryParse(input, out int value))
            {
                return value;
            }
            else
            {
                Message(ConsoleColor.Red, "Please enter a valid integer!");
            }
        }
    }

    /// <summary>
    /// Prints memory operations submenu to console. Offers 4 different memory
    /// operation options and clears the console.
    /// </summary>
    /// <remarks>
    /// Menu options:
    /// 1. Add to memory
    /// 2. Subtract from memory
    /// 3. Get memory
    /// 4. Clear memory
    /// 
    /// Process: Clears screen with Console.Clear(), then prints menu.
    /// </remarks>
    private static void DisplayMemoryTransaction()
    {
        Console.Clear();

        Operation("1", "Add to memory");
        Operation("2", "Subtract from memory");
        Operation("3", "Get memory");
        Operation("4", "Clear memory");
    }

    /// <summary>
    /// Prints main calculator menu to console. Offers 12 different mathematical
    /// operation options and clears the console.
    /// </summary>
    /// <remarks>
    /// Menu options:
    /// 1-4: Basic operations (addition, subtraction, multiplication, division)
    /// 5-6: Exponentiation and root extraction
    /// 7: Factorial
    /// 8: Modulo
    /// 9: Logarithm
    /// 10: Trigonometry
    /// 11: Memory operations
    /// 12: Exit
    /// 
    /// Each option is visually supported with emoji.
    /// </remarks>
    private static void DisplayMenu()
    {
        Console.Clear();

        Operation(" 1", "Addition           ➕");
        Operation(" 2", "Subtraction       ➖");
        Operation(" 3", "Multiplication    ✖️");
        Operation(" 4", "Division          ➗");
        Operation(" 5", "Exponentiation    xⁿ");
        Operation(" 6", "Root Extraction   ⁿ√x");
        Operation(" 7", "Factorial         ❗");
        Operation(" 8", "Modulo            %");
        Operation(" 9", "Logarithm        logx(y)");
        Operation("10", "Trigonometry      📐");
        Operation("11", "Memory Operations 🧠");
        Operation("12", "Exit              🔚");
    }

    /// <summary>
    /// Prints trigonometric functions submenu to console. Offers 6 different
    /// trigonometric function options and clears the console.
    /// </summary>
    /// <remarks>
    /// Menu options:
    /// 1. Sine (sin)
    /// 2. Cosine (cos)
    /// 3. Tangent (tan)
    /// 4. Cotangent (cot)
    /// 5. Secant (sec)
    /// 6. Cosecant (csc)
    /// 
    /// Process: Clears screen with Console.Clear(), then prints menu.
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
    /// Creates standard error message format. Combines the given message with warning emoji
    /// and standard format.
    /// </summary>
    /// <param name="exceptionMessage">Content of the error message</param>
    /// <returns>Formatted error message string</returns>
    /// <remarks>
    /// Format: "\n⚠️ An error occurred : {exceptionMessage}"
    /// 
    /// Usage: Provides consistent format for all error messages.
    /// </remarks>
    private static string ExceptionMessage(string exceptionMessage)
    {
        return $"\n⚠️ An error occurred : {exceptionMessage}";
    }

    /// <summary>
    /// Performs exit operation from application. Asks user for confirmation
    /// and closes application or continues based on response.
    /// </summary>
    /// <remarks>
    /// Process flow:
    /// 1. Asks user "Are you sure you want to exit (Y/N) : "
    /// 2. If "Y" or "y" is entered, closes application with Environment.Exit(0)
    /// 3. If "N" or "n" is entered, cancels the operation
    /// 4. Shows error message for invalid input
    /// 
    /// Null/empty input check is performed.
    /// </remarks>
    private static void Exit()
    {
        Console.Write("\nAre you sure you want to exit (Y/N) : ");
        string? act = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(act))
        {
            Message(ConsoleColor.Red, ExceptionMessage("You performed an invalid operation!")); 
            return; 
        }
        else
        {
            if (act.ToLower() == "y")
                Environment.Exit(0);
            else if (act.ToLower() == "n")
                return;
            else
                Message(ConsoleColor.Red, ExceptionMessage("You performed an invalid operation!"));
        }
    }

    /// <summary>
    /// Prints message to console in specified color. After printing the message,
    /// returns console color to default state.
    /// </summary>
    /// <param name="color">Console color in which the message will be written</param>
    /// <param name="message">Content of the message to be written</param>
    /// <remarks>
    /// Process sequence:
    /// 1. Sets color with Console.ForegroundColor = color
    /// 2. Prints message with Console.WriteLine(message)
    /// 3. Returns color to default state with Console.ResetColor()
    /// 
    /// Usage: For error messages (red), success messages (green).
    /// </remarks>
    private static void Message(ConsoleColor color, string message)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// <summary>
    /// Prints menu options to console. Shows queue number in red, operation
    /// name in white color.
    /// </summary>
    /// <param name="queue">Queue number (shown in red color)</param>
    /// <param name="operation">Operation name (shown in white color)</param>
    /// <remarks>
    /// Format: "{queue}. {operation}"
    /// 
    /// Color scheme:
    /// - Queue number: ConsoleColor.Red
    /// - Operation name: ConsoleColor.White
    /// 
    /// Usage: Provides consistent appearance for all menu options.
    /// </remarks>
    private static void Operation(string queue, string operation)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{queue}. ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{operation}");
    }

    /// <summary>
    /// Prints mathematical operation result to console. Shows special error message
    /// for NaN values, prints success message in green color for valid results.
    /// </summary>
    /// <param name="result">Operation result to be displayed</param>
    /// <remarks>
    /// Checks:
    /// - double.IsNaN(result) check
    /// - If NaN, shows "❌ Operation failed!" message in red color
    /// - If valid result, shows "✅ Operation result: {result}" in green color
    /// 
    /// Usage: To display results of all mathematical operations.
    /// </remarks>
    private static void ShowResult(double result)
    {
        if (double.IsNaN(result))
        {
            Message(ConsoleColor.Red, "\n❌ Operation failed!");
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n✅ Operation result: {result}");
        Console.ResetColor();
    }

    /// <summary>
    /// Waits for user to press any key. Shows information message in yellow color
    /// on screen and waits until key is pressed.
    /// </summary>
    /// <remarks>
    /// Process sequence:
    /// 1. Hides cursor with Console.CursorVisible = false
    /// 2. Shows "⌛ Please press any key to continue" message in yellow color
    /// 3. Waits for key with Console.ReadKey()
    /// 4. Shows cursor again with Console.CursorVisible = true
    /// 5. Runs transition animation with SpinnerAnimation()
    /// 
    /// Usage: Waiting after each operation for user to see the result.
    /// </remarks>
    private static void WaitingScreen()
    {
        Console.CursorVisible = false;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("\n⌛ Please press any key to continue");
        Console.ResetColor();
        Console.ReadKey();
        Console.CursorVisible = true;
        SpinnerAnimation();
    }

    /// <summary>
    /// Shows a simple spinning animation in console. Creates spinning effect
    /// using 4 different characters (-, \, |, /).
    /// </summary>
    /// <remarks>
    /// Animation details:
    /// - 4 characters: '-', '\\', '|', '/'
    /// - Each character is shown for 50ms
    /// - Total 5 loops (20 character changes)
    /// - Total duration: 1 second
    /// 
    /// Process:
    /// 1. Clears screen with Console.Clear()
    /// 2. Hides cursor with Console.CursorVisible = false
    /// 3. Shows characters in sequence with loop
    /// 4. Shows cursor again with Console.CursorVisible = true
    /// 
    /// Usage: Visual effect for transitions between operations.
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
