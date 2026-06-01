using System.Security.Claims;
using BolaoCopaApp.Application.Interfaces;
using BolaoCopaApp.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BolaoCopaApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IUserRepository _userRepository;

    public PaymentController(IPaymentService paymentService, IUserRepository userRepository)
    {
        _paymentService = paymentService;
        _userRepository = userRepository;
    }

    [Authorize]
    [HttpPost("pix")]
    public async Task<ActionResult<PixPaymentResult>> GeneratePix(CancellationToken ct)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

        var user = await _userRepository.GetByIdAsync(userId, ct);
        if (user == null) return NotFound("User not found");

        if (user.IsPaid) return BadRequest("User already paid");

        // Amount is fixed at R$ 30,00 for this bolão
        var result = await _paymentService.CreatePixPaymentAsync(user.Id, user.Email.Value, user.Name, 30.00m, ct);
        return Ok(result);
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook([FromQuery] string type, [FromBody] dynamic data, CancellationToken ct)
    {
        // Mercado Pago sends different types of notifications. We only care about "payment"
        if (type == "payment")
        {
            long paymentId = (long)data.data.id;
            await _paymentService.ProcessWebhookAsync(paymentId, "approved", ct);
        }

        // Always return 200/201 to MP to acknowledge receipt
        return Ok();
    }
}
