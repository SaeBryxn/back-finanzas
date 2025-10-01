using System.ComponentModel.DataAnnotations;

namespace CreditApp.Api.Models;

public class Client
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    public string Nombres { get; set; } = default!;
    public string Apellidos { get; set; } = default!;
    public string Documento { get; set; } = default!;
    public string Telefono { get; set; } = default!;
    public string Email { get; set; } = default!;
    public decimal IngresosMensuales { get; set; }
    public int Dependientes { get; set; }
    public string Empleo { get; set; } = default!;
}