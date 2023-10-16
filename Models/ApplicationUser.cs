using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Trellol.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public List<Card> Cards { get; set; } = new List<Card>();

        public string? Pseudo {  get; set; }
    }
}
