using AdvokatenBlazor.Model;
using BlazorBootstrap;

namespace AdvokatenBlazor.Service
{
    public class ChartService
    {

        private readonly string[] _backgroundColors;
        private int _datasetsCount = 0;
        private int _dataLabelsCount = 0;
        private readonly Random _random = new();

        public ChartService()
        {
            _backgroundColors = ColorUtility.CategoricalTwelveColors;
        }

        public List<IChartDataset> GetDefaultDataSets(List<Asset> assets, int numberOfDatasets)
        {
            var datasets = new List<IChartDataset>();

            for (var index = 0; index < numberOfDatasets; index++)
            {
                datasets.Add(GetRandomPieChartDataset(assets));
            }

            return datasets;
        }

        private PieChartDataset GetRandomPieChartDataset(List<Asset> assets)
        {
            _datasetsCount += 1;
            return new PieChartDataset
            {
                Data = GetData(assets),
                BackgroundColor = GetRandomBackgroundColors()
            };
        }

        private List<double?> GetData(List<Asset> assets)
        {
            return assets.Select(asset => (double?)asset.Value).ToList();
        }

        public List<string> GetDefaultDataLabels(List<Asset> assets)
        {
            var labels = assets.Select(asset =>
            {
                return asset.AssetType switch
                {
                    AssetType.Property => $"{asset.Location} (ejendom)",
                    AssetType.Vehicle => $"{asset.Brand} (køretøj)",
                    AssetType.Money => $"{asset.Name} (kroner)",
                    AssetType.Item => $"{asset.Name} (ejendel)",
                    AssetType.Stock => $"{asset.Name} (værdipapir)",
                    _ => "Ukendt"
                };
            }).ToList();

            _dataLabelsCount = labels.Count; // Set the count here.
            return labels;
        }

        private List<string> GetRandomBackgroundColors()
        {
            return _backgroundColors.Take(_dataLabelsCount).ToList();
        }
    }
}

