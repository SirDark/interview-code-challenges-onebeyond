using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using OneBeyondApi.DataAccess;
using OneBeyondApi.DTOs;
using OneBeyondApi.Exceptions;
using OneBeyondApi.Model;
using OneBeyondApi.Services;

namespace OneBeyondApi.Controllers;

[Route("[controller]")]
[ApiController]
public class LoanController : ControllerBase
{
    private ICatalogueRepository _catalogueRepository;
    private ILogger<LoanController> _logger;
    private readonly ILoanService _loanService;
    public LoanController(ICatalogueRepository catalogueRepository,ILoanService loanService, ILogger<LoanController> logger)
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
        if(borrowers == null)
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
}



