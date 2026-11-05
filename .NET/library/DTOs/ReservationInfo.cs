using OneBeyondApi.Model;

namespace OneBeyondApi.DTOs;

public class ReservationInfo
{
    public required Book Book { get; set; }
    public required bool Reserved { get; set; }
    public DateTime? LoanEndDate { get; set; }

    public static ReservationInfo fromBookStock(BookStock bookStock)
    {
        return new ReservationInfo {
            Book = bookStock.Book, 
            LoanEndDate = bookStock.LoanEndDate, 
            Reserved = bookStock.Reservation != null 
        };
    }

}
