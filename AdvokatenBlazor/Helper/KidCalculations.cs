using AdvokatenBlazor.Model;
using AdvokatenBlazor.ViewModel;
using System.Diagnostics;

namespace AdvokatenBlazor.Helper
{
    public class KidCalculations
    {
        public static void CalculateAll()
        {
            foreach (Heir kid in HeirRepository.Instance.Heirs.Where(x => x.HeirType == HeirType.Kid))
            {
                kid.MaxInheritancePercentage = MaxPercentage();
                kid.MinInheritancePercentage = MinPercentage();
                kid.MaxInheritanceAmount = MaxAmount();
                kid.MinInheritanceAmount = MinAmount();
                AmountCalculations.CalculateCurrentAmount();
            }
            
        }

        public static double MaxPercentage()
        {
            if (Client.KidsAmount == 0)
            {
                return 0;
            }

            else if (!Client.Married && Client.KidsAmount == 1)
            {
                return 100;
            }

            else if (!Client.Married)
            {
                return 87.5;
            }

            else if (Client.Married)
            {
                double max_value = 75;
                double shared_value_with_kids = 12.5 / Client.KidsAmount;
                double final_amount = 75 + shared_value_with_kids;
                return TruncateToDecimalPlaces(final_amount, 2);
            }

            return 0;
        }



        public static double MinPercentage()
        {
            if (!Client.Married && Client.KidsAmount > 0)
            {
                double amount = 100 / (double)Client.KidsAmount;
                double percentageoff = amount * 0.25;
                return TruncateToDecimalPlaces(percentageoff, 2);
            }
            else if (Client.Married)
            {
                double amount = 50 / (double)Client.KidsAmount;
                double percentageoff = amount * 0.25;
                return TruncateToDecimalPlaces(percentageoff, 2);
            }

            return 0;
        }

        public static double MinAmount()
        {
            if (!Client.Married && Client.KidsAmount > 0)
            {
                double amount = Client.TotalValue / Client.KidsAmount;
                double percentageoff = amount * 0.25;
                return Math.Truncate(percentageoff);
            }

            else if (Client.Married)
            {
                double amount = Client.TotalValue / Client.KidsAmount;
                double marriedPercent = amount * 0.5;
                double percentageoff = marriedPercent * 0.25;
                return Math.Truncate(percentageoff);
            }

            return 0;
        }

        public static double MaxAmount()
        {
            double maxPercentage = MaxPercentage() / 100;
            double maxAmount = Client.TotalValue * maxPercentage;
            return Math.Truncate(maxAmount);

        }


        private static double TruncateToDecimalPlaces(double value, int decimalPlaces)
        {
            double factor = Math.Pow(10, decimalPlaces);
            return Math.Truncate(value * factor) / factor;
        }

        
    }
}
