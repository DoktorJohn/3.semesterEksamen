using AdvokatenBlazor.Model;
namespace AdvokatenBlazor.ViewModel
{
    public class HeirInheritanceCalc
    {
        public static void CalculateCurrentInheritanceAmountForHeir()
        {
            foreach (var heir in HeirRepository.Instance.Heirs)
            {
                heir.CurrentInheritanceAmount = 0;
                foreach (var asset in AssetRepository.Instance.assets)
                {
                    if (asset.HeirPercentage.TryGetValue(heir, out double percentage))
                    {
                        double ClientPercentage = (double)asset.PercentageOwned / 100;
                        double percentage_multiplicate = (percentage / 100.0) * ClientPercentage;
                        heir.CurrentInheritanceAmount += asset.Value * percentage_multiplicate;
                    }
                }
            }

        }
    }
}
