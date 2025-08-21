public class MathOperationException : Exception
{
    public MathOperationException() { }

    public MathOperationException(string message) : base(message) { }

    public MathOperationException(string message, Exception inner) : base(message, inner) { }
}
