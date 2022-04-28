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
    public IEnumerable<VerificationResult> Verify()
    {
        var result = _disbursementsVerifier.Verify();
        return result;
    }
}