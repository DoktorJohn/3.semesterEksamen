using System.ComponentModel.DataAnnotations;

namespace AdvokatenBlazor.Model
{
    public static class Client
    {
        [Required]
        [MaxLength(100)]
        public static string Name { get; set; }
        [Required]
        public static int KidsAmount { get; set; }
        [Required]
        public static bool Married { get; set; }
        [Required]
        public static bool Testament {  get; set; }

    }
}
