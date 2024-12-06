using AdvokatenBlazor.Model;
using AdvokatenBlazor.ViewModel;
using BlazorBootstrap;

namespace AdvokatenBlazor.Service
{
    public class ChartService
    {
        private readonly string[] _backgroundColors;
        private readonly Random _random = new();

        public ChartService()
        {
            _backgroundColors = ColorUtility.CategoricalTwelveColors;
        }

        public List<IChartDataset> GetDefaultDataSets(List<Asset> assets, int numberOfDatasets, int dataCount)
        {
            var datasets = new List<IChartDataset>();

            for (var index = 0; index < numberOfDatasets; index++)
            {
                datasets.Add(GetRandomPieChartDataset(assets, dataCount));
            }

            return datasets;
        }

        public List<IChartDataset> GetDefaultDataSets(List<Heir> heirs, int numberOfDatasets, int dataCount)
        {
            var datasets = new List<IChartDataset>();

            for (var index = 0; index < numberOfDatasets; index++)
            {
                datasets.Add(GetRandomPieChartDataset(heirs, dataCount));
            }

            return datasets;
        }

        private PieChartDataset GetRandomPieChartDataset(List<Asset> assets, int dataCount)
        {
            return new PieChartDataset
            {
                Data = GetData(assets),
                BackgroundColor = GetRandomBackgroundColors(dataCount)
            };
        }

        private PieChartDataset GetRandomPieChartDataset(List<Heir> heirs, int dataCount)
        {
            return new PieChartDataset
            {
                Data = GetData(heirs),
                BackgroundColor = GetRandomBackgroundColors(dataCount)
            };
        }

        private List<double?> GetData(List<Asset> assets)
        {
            return assets.Select(asset => ((double?)asset.Value - asset.Debt) * ((double)asset.PercentageOwned / 100)).ToList();
        }

        private List<double?> GetData(List<Heir> heirs)
        {
            return heirs.Select(heirs => (double?)heirs.CurrentInheritanceAmount).ToList();
        }

        public List<string> GetDefaultDataLabels(List<Asset> assets)
        {
            return assets.Select(asset =>
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
        }

        public List<string> GetDefaultDataLabelsForHeirs()
        {
            return HeirRepository.Instance.Heirs.Select(heir => heir.Name).ToList();
        }

        private List<string> GetRandomBackgroundColors(int count)
        {
            return _backgroundColors.Take(count).ToList();
        }
    }
}

