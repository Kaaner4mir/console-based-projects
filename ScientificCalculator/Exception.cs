public class MyException : Exception
{
    public MyException() : base("⚠️ Geçersiz bir işlem yaptınız!") { }
    public MyException(string message) : base(message) { }
    public MyException(string message, Exception innerException) : base(message, innerException) { }
}
