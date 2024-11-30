using AdvokatenBlazor.Model;
using AdvokatenBlazor.ViewModel;

namespace AdvokatenBlazor.Helper
{
    public class SpouseCalculations
    {
        public static void CalculateAll()
        {
            HeirRepository.Instance.Spouse.MinInheritancePercentage = MinPercentage();
            HeirRepository.Instance.Spouse.MaxInheritancePercentage = MaxPercentage();
            HeirRepository.Instance.Spouse.MinInheritanceAmount = MinAmount();
            HeirRepository.Instance.Spouse.MaxInheritanceAmount = MaxAmount();
        }
        public static double MaxPercentage()
        {
            if (Client.KidsAmount <= 0)
            {
                return 100;
            }

            else
            {
                return 87.5;
            }
        }

        public static double MinPercentage()
        {
            if (Client.KidsAmount <= 0)
            {
                return 100 * 0.25;
            }

            else
            {
                return 50 * 0.25;
            }
        }

        public static double MinAmount()
        {
            if (Client.KidsAmount <= 0)
            {
                return Client.TotalValue * 0.25;
            }

            else
            {
                double amount = Client.TotalValue * 0.25;
                return amount * 0.5;
            }
        }

        public static double MaxAmount()
        {
            if (Client.KidsAmount <= 0)
            {
                return Client.TotalValue;
            }

            else
            {
                double amount = Client.TotalValue;
                double highestAmount = amount * 0.75;
                double amountWithForced = highestAmount + Client.TotalValue * 0.125;
                return amountWithForced;
            }
        }
    }
}
