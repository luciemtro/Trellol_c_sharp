using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Trellol.Models
{
    // Custom ApplicationUser class that extends IdentityUser
    public class ApplicationUser : IdentityUser
    {
        // Required attribute ensures that Cards property is not null
        [Required]
        // Collection of Card objects associated with the user, initialized as an empty list
        public List<Card> Cards { get; set; } = new List<Card>();

        // Nullable string property representing the user's pseudonym
        public string? Pseudo { get; set; }
    }
}

