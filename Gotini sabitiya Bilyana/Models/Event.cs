using System;
using System.ComponentModel.DataAnnotations;

namespace Gotini_sabitiya_Bilyana.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Event Image")]
        public byte[] EventImage { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

    }
}
