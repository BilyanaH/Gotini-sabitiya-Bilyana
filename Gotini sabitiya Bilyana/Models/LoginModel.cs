using System.ComponentModel.DataAnnotations;

namespace Gotini_sabitiya_Bilyana.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
