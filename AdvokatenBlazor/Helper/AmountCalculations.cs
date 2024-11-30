using AdvokatenBlazor.Model;
using AdvokatenBlazor.ViewModel;
namespace AdvokatenBlazor.Helper
{
    public class AmountCalculations
    {
        public static void CalculateCurrentAmount()
        {
            foreach (var heir in HeirRepository.Instance.Heirs)
            {
                heir.CurrentInheritanceAmount = 0;
            }

            foreach (var asset in AssetRepository.Instance.assets)
            {
                foreach (var heirRow in asset.HeirRows)
                {
                    if (heirRow.SelectedHeirId.HasValue)
                    {
                        var selectedHeir = HeirRepository.Instance.Heirs
                            .FirstOrDefault(h => h.Id == heirRow.SelectedHeirId.Value);

                        if (selectedHeir != null)
                        {
                            double clientPercentage = asset.PercentageOwned / 100.0;
                            double heirPercentage = heirRow.Percentage / 100.0;
                            double effectivePercentage = heirPercentage * clientPercentage;

                            selectedHeir.CurrentInheritanceAmount += asset.Value * effectivePercentage;
                        }
                    }
                }
            }

        }
    }
}
