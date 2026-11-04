namespace OneBeyondApi.Exceptions;

public class BookNotLoanedException : Exception
{
    public BookNotLoanedException(string message) : base(message) { }
}
