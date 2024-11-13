using AdvokatenBlazor.Model;

namespace AdvokatenBlazor.ViewModel
{
    public class HeirRepository
    {
        public List<Heir> Heirs { get; set; }
        public Heir Spouse { get; set; }
        
        HeirRepository()
        {
            Heirs = new List<Heir>();
            Spouse = new Heir();
        }

        private static HeirRepository instance;
        public static HeirRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HeirRepository();
                }

                return instance;
            }
        }

        
        public List<Heir> ReturnKids()
        {
            return Heirs.Where(s => s.HeirType == HeirType.Kid).ToList();
        }
            
        public void DeleteSpouse()
        {
            var spouse = Heirs.FirstOrDefault(s => s.HeirType == HeirType.Spouse);

            if (spouse != null)
            {
                Heirs.Remove(spouse);
            }
        }

        public void GenerateHeirs(HeirType type)
        {
            if (type == HeirType.Kid)
            {
                Heirs.RemoveAll(h => h.HeirType == HeirType.Kid);

                for (int i = 0; i < Client.KidsAmount; i++)
                {
                    Heir h = new Heir { HeirType = HeirType.Kid };
                    h.InheritancePercentage = InheritanceCalc.CalculateInheritancePercentageForKid();
                    Heirs.Add(h);
                }
            }

            else
            {
                if (!Heirs.Any(s => s.HeirType == HeirType.Spouse))
                {
                    Heir h = new Heir { HeirType = HeirType.Spouse };
                    h.InheritancePercentage = InheritanceCalc.CalculateInheritancePercentageForMarried();
                    Heirs.Add(h);
                    Spouse = h;
                }
            }

        }
    }
}
