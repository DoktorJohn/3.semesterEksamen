using AdvokatenBlazor.Model;

namespace AdvokatenBlazor.ViewModel
{
    public class AssetRepository
    {
        public List<Asset> assets {  get; set; }

        private static AssetRepository instance;
        public static AssetRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AssetRepository();
                }

                return instance;
            }
        }

        public AssetRepository()
        {
            assets = new List<Asset>();
        }


        public static void RemoveRow(Asset a, HeirRow h)
        {
            a.HeirRows.Remove(h);
        }

        public double ReturnTotalValue()
        {
            double debt = 0;

            foreach (var a in assets)
            {
                debt += a.Debt;
            }

            return Client.TotalValue - debt;
        }

        public double ReturnMaxValue()
        {
            double debt = 0;

            foreach (var a in assets)
            {
                debt += a.Debt;
            }


            return (Client.TotalValue - debt) * 0.75;
        }
    }
}
