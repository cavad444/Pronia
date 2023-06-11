using Pronia.Models;

namespace Pronia.Models
{
    public class PlantImage
    {
        public Plant Plant { get; set; }

        public int Id { get; set; }
        public int PlantId { get; set; }
        public string ImageName { get; set; }
        public bool PosterStatus { get; set; }
    }
}
