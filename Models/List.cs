using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trellol.Models
{
    // Model class representing a list
    public class List
    {
        // Primary key for the list
        [Key]
        public int Id { get; set; }

        // Foreign key referencing the associated board
        public int BoardId { get; set; }

        // Navigation property representing the associated board
        public Board Board { get; set; }

        // Name of the list
        public string Name { get; set; }

        // Collection of Card objects associated with the list, initialized as an empty list
        public List<Card> Cards { get; set; } = new List<Card>();
    }
}
