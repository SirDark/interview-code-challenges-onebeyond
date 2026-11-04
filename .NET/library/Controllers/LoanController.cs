using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using OneBeyondApi.DataAccess;
using OneBeyondApi.DTOs;
using OneBeyondApi.Model;

namespace OneBeyondApi.Controllers;

[Route("[controller]")]
[ApiController]
public class LoanController : ControllerBase
{
    private ICatalogueRepository _catalogueRepository;
    private ILogger<LoanController> _logger;
    public LoanController(ICatalogueRepository catalogueRepository, ILogger<LoanController> logger)
    {
        _catalogueRepository = catalogueRepository;
        _logger = logger;
    }

    [HttpGet("OnLoan")]
    public ActionResult<List<ActiveBorrowers>> OnLoan()
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
}



