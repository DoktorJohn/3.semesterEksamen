using System.ComponentModel.DataAnnotations;

namespace AdvokatenBlazor.Model
{
    public enum HeirType {Spouse, Kid};

    public class Heir
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double InheritancePercentage { get; set; }
        [Required]
        public HeirType HeirType { get; set; }
    }
}
