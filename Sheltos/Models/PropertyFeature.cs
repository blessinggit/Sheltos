using System.ComponentModel.DataAnnotations.Schema;

namespace Sheltos.Models
{
    public class PropertyFeature
    {
        public int PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public Property Property { get; set; }
        public int FeatureId { get; set; }
        [ForeignKey("FeatureId")]
        public Feature Feature { get; set; }
    }
}
