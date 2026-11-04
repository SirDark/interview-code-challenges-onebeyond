using OneBeyondApi.Model;

namespace OneBeyondApi.DTOs;

public class ActiveBorrowers
{
    public required Borrower BorrowerInformation {  get; set; }
    public required string BookTitle { get; set; }

    public static List<ActiveBorrowers> fromBookStock(List<BookStock> bookStocks)
    {
        return bookStocks.Select(x => new ActiveBorrowers
        {
            BorrowerInformation = x.OnLoanTo!,
            BookTitle = x.Book.Name
        }).ToList();
    }
}
