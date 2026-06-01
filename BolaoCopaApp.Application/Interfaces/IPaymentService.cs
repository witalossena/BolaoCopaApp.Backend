namespace BolaoCopaApp.Application.Interfaces;

public record PixPaymentResult(string QrCodeBase64, string QrCodeCopyPaste, long PaymentId);

public interface IPaymentService
{
    Task<PixPaymentResult> CreatePixPaymentAsync(Guid userId, string email, string name, decimal amount, CancellationToken ct = default);
    Task<bool> ProcessWebhookAsync(long paymentId, string status, CancellationToken ct = default);
}
