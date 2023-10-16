using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Trellol.Data;

namespace Trellol.Models
{
    // Model class representing a board
    public class Board
    {
        // Primary key for the board
        [Key]
        public int Id { get; set; }

        // Required attribute ensures that the Name property is not null or empty
        public required string Name { get; set; }

        // Collection of List objects associated with the board, initialized as an empty list
        public List<List> Lists { get; set; } = new List<List>();
    }
}
