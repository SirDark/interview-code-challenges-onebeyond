namespace OneBeyondApi.Exceptions;

public class BorrowerNotFoundException : Exception
{
    public BorrowerNotFoundException(string message) : base(message) { }
}
