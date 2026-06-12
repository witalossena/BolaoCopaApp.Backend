using BolaoCopaApp.Domain.Enums;
using BolaoCopaApp.Domain.ValueObjects;

namespace BolaoCopaApp.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public Handle Handle { get; set; } = default!;
    public Email Email { get; set; } = default!;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsPaid { get; set; }
    public decimal PaidAmount { get; set; } = 0;
    public bool IsPredictionUnlocked { get; set; } = false;
    public Role Role { get; set; } = Role.Player;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
