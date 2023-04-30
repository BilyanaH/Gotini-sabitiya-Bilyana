using Gotini_sabitiya_Bilyana.Models.Domain;
using Gotini_sabitiya_Bilyana.Models;
using System.ComponentModel.DataAnnotations;

public class Ticket
{
    [Key]
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public int Quantity { get; set; }
}
