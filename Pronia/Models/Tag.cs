using Pronia.Models;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public List<PlantTag> PlantTags { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
