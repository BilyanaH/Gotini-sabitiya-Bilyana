using Microsoft.AspNetCore.Identity;

namespace Gotini_sabitiya_Bilyana.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
