using Pronia.Models;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Category
    {
        public List<Plant> Plants { get; set; }

        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
