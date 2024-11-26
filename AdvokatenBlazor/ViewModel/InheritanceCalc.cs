using AdvokatenBlazor.Model;
using System.Diagnostics;

namespace AdvokatenBlazor.ViewModel
{
    public class InheritanceCalc
    {
        public static double CalculateInheritancePercentageForMarried()
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

        public static double CalculateInheritancePercentageForKid()
        {
            if (!Client.Married && Client.KidsAmount == 1)
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

        public static double CalculateForcedInheritancePercentageForMarried()
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

        public static double CalculateForcedInheritancePercentageForKid()
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

        public static double CalculateForcedInheritanceAmountForKid()
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

        public static double CalculateInheritanceAmountForKid()
        {
            double amount = Client.TotalValue * 0.75;
            double amountWithForced = amount + Client.TotalValue * 0.125;
            return Math.Truncate(amountWithForced);

        }

        public static double CalculateForcedInheritanceAmountForMarried()
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

        public static double CalculateInheritanceAmountForMarried()
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


        private static double TruncateToDecimalPlaces(double value, int decimalPlaces)
        {
            double factor = Math.Pow(10, decimalPlaces);
            return Math.Truncate(value * factor) / factor;
        }
    }
}
