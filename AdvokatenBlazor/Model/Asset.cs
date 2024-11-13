namespace AdvokatenBlazor.Model
{
    public abstract class Asset
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public double PercentageOwned { get; set; }
    }

    public class Property : Asset
    {
        public string Location { get; set; }
    }

    public class Vehicle : Asset
    {
        public string Brand { get; set; }
    }

    public class Item : Asset
    {
        public string Name { get; set; }
    }

    public class Stock : Asset
    {
        public string Name { get; set; }
    }
}
