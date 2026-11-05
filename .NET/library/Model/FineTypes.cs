namespace OneBeyondApi.Model;


//normally I would store this in a db or in the configuration not burn it into the code but because this is a homweork I simplified it
public static class FineTypes
{
    public static readonly FineType LateReturn = new(10, "Late return");
}

public record FineType(int Amount, string Reason);
