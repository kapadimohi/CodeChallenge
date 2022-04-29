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

    [HttpPost(Name = "Verify")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<VerificationResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Verify([FromBody] string fileName="SampleSuperData.xlsx")
    {
        _logger.LogInformation("VerifyDisbursements endpoint");

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentNullException(fileName, "Disbursement file name not provided");
        
        var result = await _disbursementsVerifier.Verify(fileName);
        return Ok(result);
    }
}