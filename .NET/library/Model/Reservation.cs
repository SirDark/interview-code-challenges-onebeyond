namespace OneBeyondApi.Model;

public class Reservation
{
    //right now this only has one property but if we expand the scope in the future so that they can reserve for specific dates than this class will make more sense
    public Guid Id { get; set; }
    public required Guid ReservedTo { get; set; }
}
