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
    public async Task<IActionResult> Webhook([FromQuery] string? type, [FromQuery] string? data_id, [FromBody] System.Text.Json.JsonElement body, CancellationToken ct)
    {
        // Mercado Pago can send info in different ways depending on the notification version
        // 1. Via Query String: ?type=payment&data.id=123
        // 2. Via Body: { "type": "payment", "data": { "id": "123" } }

        string? notificationType = type;
        string? paymentIdStr = data_id;

        if (string.IsNullOrEmpty(notificationType) && body.TryGetProperty("type", out var typeProp))
        {
            notificationType = typeProp.GetString();
        }

        if (string.IsNullOrEmpty(paymentIdStr) && body.TryGetProperty("data", out var dataProp) && dataProp.TryGetProperty("id", out var idProp))
        {
            paymentIdStr = idProp.GetString();
        }

        if (notificationType == "payment" && long.TryParse(paymentIdStr, out long paymentId))
        {
            await _paymentService.ProcessWebhookAsync(paymentId, "approved", ct);
        }

        // Always return 200 or 201 to Mercado Pago
        return Ok();
    }
}
