using OneBeyondApi.DataAccess;
using OneBeyondApi.Exceptions;
using OneBeyondApi.Model;

namespace OneBeyondApi.Services;

public class LoanService : ILoanService
{
    private readonly ICatalogueRepository _catalogueRepository;
    private readonly IBorrowerRepository _borrowerRepository;
    private readonly ILogger<ILoanService> _logger;
    public LoanService(ICatalogueRepository catalogueRepository,IBorrowerRepository borrowerRepository, ILogger<ILoanService> logger)
    {
        _catalogueRepository = catalogueRepository;
        _borrowerRepository = borrowerRepository;
        _logger = logger;
    }

    public void ReturnBook(Guid bookStockId)
    {
        _logger.LogInformation($"Getting bookstock with id: {bookStockId}");
        var stock = _catalogueRepository.GetBookStock( bookStockId );
        if (stock.OnLoanTo == null)
        {
            _logger.LogError($"Bookstock is not load with id: {bookStockId}");
            throw new BookNotLoanedException($"Bookstock is not loaned with Id: {bookStockId}");
        }

        //normally these 2 should be in a transaction if anything goes wrong
        if (stock.LoanEndDate < DateTime.Now)
        {
            _logger.LogInformation($"Late return on id: {bookStockId} a fine is raised");
            Fine fine = new Fine
            {
                Amount = FineTypes.LateReturn.Amount,
                BorrowerId = stock.OnLoanTo!.Id,
                Id = Guid.NewGuid(),
                IssuedAt = DateTime.Now,
                Reason = FineTypes.LateReturn.Reason
            };
            _borrowerRepository.AddFine(fine);
        }

        _logger.LogInformation($"Returning book with stock id: {bookStockId}");
        _catalogueRepository.ReturnBook(bookStockId);

        
    }
}
