using System.ComponentModel.DataAnnotations;

namespace AdvokatenBlazor.Model
{
    public static class Client
    {
        [Required]
        [MaxLength(100)]
        public static string Name { get; set; }
        [Required]
        private static int _kidsAmount;

        public static int KidsAmount
        {
            get { return _kidsAmount; }
            set
            {
                if (value >= 0)
                {
                    _kidsAmount = value;
                }

                else
                {
                    _kidsAmount = 0;
                }
                
            }
        }

        [Required]
        public static bool Married { get; set; }
        [Required]
        public static bool Testament { get; set; }

        public static double TotalValue { get; set; }
        public static string AssetString { get; set; } = string.Empty;

    }
}
