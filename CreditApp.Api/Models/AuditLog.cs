using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace CreditApp.Api.Models;

public class AuditLog
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    public string UserEmail { get; set; } = default!;
    public string Action { get; set; } = default!;
    public string Entity { get; set; } = default!;
    public string EntityId { get; set; } = default!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // JSON libre
    [Column(TypeName = "jsonb")]
    public JsonDocument? Payload { get; set; }
}