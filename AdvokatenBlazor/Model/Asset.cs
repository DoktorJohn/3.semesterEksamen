using System.ComponentModel.DataAnnotations;

namespace AdvokatenBlazor.Model
{
    public enum AssetType
    {
        Property,
        Vehicle,
        Item,
        Stock,
        Money,
        None
    }

    public class Asset
    {
        public double Value { get; set; }
        private int _percentageOwned;

        public int PercentageOwned
        {
            get { return _percentageOwned; }
            set {

                if (value > 100)
                {
                    _percentageOwned = 100;
                }
                else if (value < 0)
                {
                    _percentageOwned = 0;
                }
                else
                {
                    _percentageOwned = value;
                }
            }
        }

        public AssetType AssetType { get; set; }

        public string? Location { get; set; }
        public string? Brand { get; set; }
        public string? Name { get; set; }
    }
}
