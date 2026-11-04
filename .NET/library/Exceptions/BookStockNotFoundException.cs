namespace OneBeyondApi.Exceptions;

public class BookStockNotFoundException : Exception
{
    public BookStockNotFoundException(string message) : base(message) { }
}
