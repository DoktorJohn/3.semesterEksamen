using AdvokatenBlazor.Model;

namespace AdvokatenBlazor.ViewModel
{
    public class HeirRepository
    {
        public List<Heir> Heirs { get; set; }
        public Heir Spouse { get; set; }
        
        
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

        HeirRepository()
        {
            Heirs = new List<Heir>();
            Spouse = new Heir();
        }


        public List<Heir> ReturnKids()
        {
            return Heirs.Where(s => s.HeirType == HeirType.Kid).ToList();
        }
            
        public List<Heir> ReturnOther()
        {
            return Heirs.Where(s => s.HeirType == HeirType.Other).ToList();
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
                    h.Id++;
                    Heirs.Add(h);
                }
            }

            else if (type == HeirType.Spouse)
            {
                if (!Heirs.Any(s => s.HeirType == HeirType.Spouse))
                {
                    Heir h = new Heir { HeirType = HeirType.Spouse };
                    h.InheritancePercentage = InheritanceCalc.CalculateInheritancePercentageForMarried();
                    h.Id++;
                    Heirs.Add(h);
                    Spouse = h;
                }
            }

            else if (type == HeirType.Other)
            {
                    Heir h = new Heir { HeirType = HeirType.Other };
                    h.Id++;
                    Heirs.Add(h);
            }

        }

        public Heir CreatePlaceholderHeir()
        {
            Heir heir = new Heir();
            heir.Id++;
            return heir;
        }
    }
}
