namespace OneBeyondApi.Model;

public class Fine
{
    public required Guid Id { get; set; }
    public required Guid BorrowerId { get; set; }
    public required float Amount { get; set; }
    public required string Reason { get; set; }
    public required DateTime IssuedAt { get; set; }
    public DateTime? PaidAt { get; set; }
}
