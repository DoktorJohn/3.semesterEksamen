using AdvokatenBlazor.Model;
using System.Diagnostics;

namespace AdvokatenBlazor.Model
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
                return 50;
            }
        }

        public static double CalculateInheritancePercentageForKid()
        {
            if (!Client.Married && Client.KidsAmount > 0)
            {
                return 100 / (double)Client.KidsAmount;
            }

            else if (Client.Married)
            {
                return 50 / (double)Client.KidsAmount;
            }

            return 0;
        }
    }
}
