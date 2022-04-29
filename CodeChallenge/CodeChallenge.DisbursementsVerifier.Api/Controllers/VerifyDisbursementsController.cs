using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.DisbursementsVerifier.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VerifyDisbursementsController : ControllerBase
{
    private readonly IDisbursementsVerifier _disbursementsVerifier;
    private readonly ILogger<VerifyDisbursementsController> _logger;

    public VerifyDisbursementsController(
        IDisbursementsVerifier disbursementsVerifier,
        ILogger<VerifyDisbursementsController> logger)
    {
        _disbursementsVerifier = disbursementsVerifier;
        _logger = logger;
    }

    [HttpGet(Name = "Verify")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<VerificationResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Verify()
    {
        _logger.LogInformation("VerifyDisbursements endpoint");
        
        var result = await _disbursementsVerifier.Verify();
        return Ok(result);
    }
}