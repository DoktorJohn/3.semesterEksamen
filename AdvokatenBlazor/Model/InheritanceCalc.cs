using AdvokatenBlazor.Model;

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
            return 50 / (double)Client.KidsAmount;
        }
    }
}
