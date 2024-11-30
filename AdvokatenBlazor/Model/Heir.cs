using System.ComponentModel.DataAnnotations;

namespace AdvokatenBlazor.Model
{
    public class Heir
    {
        private static int _idCounter = 1;

        public int Id { get; private set; }

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
        public double MaxInheritancePercentage { get; set; }
        [Required]
        public double MinInheritancePercentage { get; set; }
        [Required]
        public double MaxInheritanceAmount { get; set; }
        [Required]
        public double MinInheritanceAmount { get; set; }
        [Required]
        public double CurrentInheritanceAmount { get; set; }
        [Required]
        public HeirType HeirType { get; set; }

        public Heir()
        {
            Id = _idCounter++;
        }
    }
}
