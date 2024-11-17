using System.ComponentModel.DataAnnotations;

namespace AdvokatenBlazor.Model
{
    public enum HeirType { Spouse, Kid };

    public class Heir
    {
        [Required]
        private string _name;

        public string Name
        {
            get
            {
                if (_name == null)
                {
                    return "";
                }
                return _name;
            }
            set
            {
                if (value != null)
                {
                    _name = value;
                }
            }
        }

        [Required]
        public double InheritancePercentage { get; set; }
        public double ForcedInheritancePercentage { get; set; }
        [Required]
        public HeirType HeirType { get; set; }
    }
}
