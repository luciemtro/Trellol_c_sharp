using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trellol.Models
{
    // Model class representing a card
    public class Card
    {
        // Primary key for the card
        [Key]
        public int Id { get; set; }

        // Foreign key referencing the associated list
        public int ListId { get; set; }

        // Navigation property representing the associated list
        public List List { get; set; }

        // Name of the card
        public string Name { get; set; }

        // Description of the card, initialized as an empty string
        public string Description { get; set; } = string.Empty;
    }
}
