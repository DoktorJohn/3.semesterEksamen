namespace AdvokatenBlazor.Model
{
    public enum HeirType {Spouse, Kid};

    public class Heir
    {
        public string Name { get; set; }
        public double InheritancePercentage { get; set; }
        public HeirType HeirType { get; set; }
    }
}
