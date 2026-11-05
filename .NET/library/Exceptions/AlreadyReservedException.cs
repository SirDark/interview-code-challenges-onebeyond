namespace OneBeyondApi.Exceptions;

public class AlreadyReservedException : Exception
{
    public AlreadyReservedException(string message) : base(message) { }
}
