using BolaoCopaApp.Application.Interfaces;
using BolaoCopaApp.Domain.Interfaces;
using BolaoCopaApp.Domain.Interfaces.Repositories;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using Microsoft.Extensions.Configuration;

namespace BolaoCopaApp.Infrastructure.Services;

public class MercadoPagoService : IPaymentService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _uow;

    public MercadoPagoService(IConfiguration configuration, IUserRepository userRepository, IUnitOfWork uow)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _uow = uow;
        
        MercadoPagoConfig.AccessToken = _configuration["MercadoPago:AccessToken"];
    }

    public async Task<PixPaymentResult> CreatePixPaymentAsync(Guid userId, string email, string name, decimal amount, CancellationToken ct = default)
    {
        var client = new PaymentClient();
        var request = new PaymentCreateRequest
        {
            TransactionAmount = amount,
            Description = "Inscrição Bolão Copa 2026",
            PaymentMethodId = "pix",
            ExternalReference = userId.ToString(),
            Payer = new PaymentPayerRequest
            {
                Email = email,
                FirstName = name.Split(' ')[0],
                LastName = name.Contains(' ') ? name.Substring(name.IndexOf(' ') + 1) : ""
            }
        };

        Payment payment = await client.CreateAsync(request, cancellationToken: ct);

        return new PixPaymentResult(
            payment.PointOfInteraction.TransactionData.QrCodeBase64,
            payment.PointOfInteraction.TransactionData.QrCode,
            payment.Id ?? 0
        );
    }

    public async Task<bool> ProcessWebhookAsync(long paymentId, string status, CancellationToken ct = default)
    {
        // Even if status comes in webhook, we should fetch from MP for security
        var client = new PaymentClient();
        Payment payment = await client.GetAsync(paymentId, cancellationToken: ct);

        if (payment.Status == "approved" && Guid.TryParse(payment.ExternalReference, out Guid userId))
        {
            var user = await _userRepository.GetByIdAsync(userId, ct);
            if (user != null && !user.IsPaid)
            {
                user.IsPaid = true;
                _userRepository.Update(user);
                await _uow.SaveChangesAsync(ct);
                return true;
            }
        }

        return false;
    }
}
