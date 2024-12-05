namespace AdvokatenBlazor.Model
{
    public enum HeirType { Spouse, Kid, Other };

    public enum AssetType
    {
        Property,
        Vehicle,
        Item,
        Stock,
        Money,
        None
    }

    public static class HeirToString
    {
        public static string Stringify(this HeirType type)
        {
            switch (type)
            {
                case HeirType.Spouse:
                    return "ægtefælle";
                case HeirType.Kid:
                    return "barn";
                case HeirType.Other:
                    return "andet";
                default:
                    return "Fejl";
            }
        }
    }
}