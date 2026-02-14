using System.ComponentModel.DataAnnotations.Schema;

namespace bugdgetwarsapi.Models;

public class Expense
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Amount { get; set; }

    public DateTime Date { get; set; }

    public string? UserId { get; set; }

    [ForeignKey("UserId")] public ApplicationUser User { get; set; }
}