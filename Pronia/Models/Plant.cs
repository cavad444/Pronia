using Pronia.Models;
using Pronia.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pronia.Models
{
    public class Plant
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    
        [MaxLength(300)]
        public string Information { get; set; }
        [MaxLength(300)]

        public string Description { get; set; }
        public int CategoryId { get; set; }

        public string Size { get; set; }
        [Column(TypeName = "money")]
        public decimal SalePrice { get; set; }
        [Column(TypeName = "money")]
        public decimal CostPrice { get; set; }
        [Required]
        public bool IsFeatured { get; set; }
        [Required]
        public bool BestSeller { get; set; }
        [Required]
        public bool Latest { get; set; }
        [AllowedFileTypes("image/jpeg", "image/png")]
        [NotMapped]
        public IFormFile PosterImage { get; set; }
        [AllowedFileTypes("image/jpeg", "image/png")]
        [NotMapped]
        public IFormFile HoverPosterImage { get; set; }
        [NotMapped]
        [AllowedFileTypes("image/jpeg", "image/png")]
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        public Category Category { get; set; }
        public List<PlantImage> PlantImages { get; set; } = new List<PlantImage>();
        public List<PlantTag> PlantTags { get; set; }


    }
}
