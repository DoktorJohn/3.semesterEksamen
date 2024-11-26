using AdvokatenBlazor.Model;
namespace AdvokatenBlazor.ViewModel
{
    public class HeirInheritanceCalc
    {
        public static void CalculateCurrentInheritanceAmountForHeir()
        {
            foreach (var heir in HeirRepository.Instance.Heirs)
            {
                foreach (var asset in AssetRepository.Instance.assets)
                {
                    if (asset.HeirPercentage.TryGetValue(heir, out double percentage))
                    {
                        double inheritanceFromAsset = asset.Value * (percentage / 100.0);
                        heir.CurrentInheritanceAmount += inheritanceFromAsset;
                    }
                }
            }

        }
    }
}
