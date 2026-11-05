using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;
using OneBeyondApi.DTOs;
using OneBeyondApi.Exceptions;
using OneBeyondApi.Services;

namespace OneBeyondApi.Controllers;

[Route("[controller]")]
[ApiController]
public class LoanController : ControllerBase
{
    private readonly ICatalogueRepository _catalogueRepository;
    private readonly ILogger<LoanController> _logger;
    private readonly ILoanService _loanService;
    public LoanController(ICatalogueRepository catalogueRepository, ILoanService loanService, ILogger<LoanController> logger)
    {
        _catalogueRepository = catalogueRepository;
        _logger = logger;
        _loanService = loanService;
    }

    [HttpGet("OnLoan")]
    public ActionResult<List<ActiveBorrowers>> GetLoans()
    {
        _logger.LogInformation("Getting borrowers information");

        var borrowers = _catalogueRepository.GetBorrowers();
        if (borrowers == null)
        {
            _logger.LogInformation("No borrowers, returning empty list");
            return new List<ActiveBorrowers>();
        }
        var activeBorrowers = ActiveBorrowers.fromBookStock(borrowers);

        return Ok(activeBorrowers);
    }

    [HttpPost("OnLoan")]
    public IActionResult ReturnLoan([FromBody] Guid bookStockId)
    {
        try
        {
            _logger.LogInformation($"Returning book with stock id: {bookStockId}");
            _loanService.ReturnBook(bookStockId);
        }
        catch (BookStockNotFoundException)
        {
            _logger.LogError($"Did not find bookstock with id: {bookStockId}");
            return NotFound($"Did not find book stock with id: {bookStockId}");
        }
        catch (BookNotLoanedException)
        {
            _logger.LogError($"Book stock is not loaned with id: {bookStockId}");
            return BadRequest($"Book stock is not loaned with id: {bookStockId}");
        }

        return Ok();
    }
    /*I went with using bookstockId here since only one bookstock should should exist for 1 book 
     * I did see in SeedData.cs that bookOnLoanUntilToday and bookNotOnLoan both use the clayBook 
     * I do not know if it was a mistake or you wanted Bookstock to store Loans retrospectively, in that case multiple BookStocks can exist in a single book
     * If it was the later option then I would change my implementation but for now I will go with this solution. 
     * (in a real life situation I would just write a message and ask)
     */
    [HttpGet("Reservation")]
    public ActionResult<ReservationInfo> GetBookReservation(Guid bookStockId)
    {
        try
        {
            _logger.LogInformation($"getting bookstock with Id: {bookStockId}");
            return Ok(
            ReservationInfo.fromBookStock(
                _catalogueRepository.GetBookStock(bookStockId)
                ));
        }
        catch (BookStockNotFoundException)
        {
            _logger.LogError($"no BookStock found with id: {bookStockId}");
            return NotFound($"no BookStock found with id: {bookStockId}");
        }

    }
    [HttpPost("Reservation")]
    public IActionResult CreateBookReservation(Guid bookStockId, Guid borrowerId)
    {
        try
        {
            _loanService.Reserve(bookStockId, borrowerId);
            return Ok();
        }
        catch (BookStockNotFoundException)
        {
            _logger.LogError($"BookStockNotFound {bookStockId}");
            return NotFound($"Book Stock Not Found with id: {bookStockId}");
        }
        catch (AlreadyReservedException)
        {
            _logger.LogError($"AlreadyReserved {bookStockId}");
            return BadRequest($"Book Stock already reserved id: {bookStockId}");
        }
        catch (BorrowerNotFoundException)
        {
            _logger.LogError($"BorrowerNotFound {bookStockId}");
            return NotFound($"Borrower not found with id: {bookStockId}");
        }
    }
}



