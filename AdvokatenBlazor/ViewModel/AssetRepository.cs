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

    }
}
