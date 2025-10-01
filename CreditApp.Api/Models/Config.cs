using System.ComponentModel.DataAnnotations;

namespace CreditApp.Api.Models;

public enum TasaTipo { EFECTIVA, NOMINAL }
public enum GraciaTipo { NINGUNA, TOTAL, PARCIAL }

public class Config
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    public Moneda Moneda { get; set; } = Moneda.PEN;
    public TasaTipo TasaTipo { get; set; } = TasaTipo.EFECTIVA;
    public decimal EfectivaAnual { get; set; } = 12.5m;
    public GraciaTipo GraciaTipo { get; set; } = GraciaTipo.NINGUNA;
    public int GraciaMeses { get; set; } = 0;
    public string Entidad { get; set; } = string.Empty;
    public bool CapitalizaEnGracia { get; set; } = false;
}