using System.ComponentModel.DataAnnotations;

namespace CreditApp.Api.Models;

public enum Moneda { PEN, USD }

public class Unit
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    public string Proyecto { get; set; } = default!;
    public string Torre { get; set; } = default!;
    public string Unidad { get; set; } = default!;
    public Moneda Moneda { get; set; } = Moneda.PEN;
    public decimal Precio { get; set; }
    public decimal PieInicial { get; set; }
    public decimal Gastos { get; set; }
    public decimal Seguros { get; set; }
    public decimal Comisiones { get; set; }
    public string? ImageUrl { get; set; }
}