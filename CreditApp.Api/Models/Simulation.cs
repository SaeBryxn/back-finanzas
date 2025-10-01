using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace CreditApp.Api.Models;

public class Simulation
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ClientId { get; set; }
    public Guid UnitId { get; set; }
    public Guid ConfigId { get; set; }

    public decimal Principal { get; set; }
    public int PlazoMeses { get; set; }
    public decimal TasaInput { get; set; }
    public TasaTipo TasaTipo { get; set; } = TasaTipo.EFECTIVA;
    public GraciaTipo GraciaTipo { get; set; } = GraciaTipo.NINGUNA;
    public int GraciaMeses { get; set; } = 0;
    public bool CapitalizaEnGracia { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // JSON que tu frontend envía/lee: resultados {..} y schedule [{..}]
    [Column(TypeName = "jsonb")]
    public JsonDocument? Resultados { get; set; }

    [Column(TypeName = "jsonb")]
    public JsonDocument? Schedule { get; set; }
}