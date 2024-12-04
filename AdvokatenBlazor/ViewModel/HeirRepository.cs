using AdvokatenBlazor.Helper;
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
            List<Heir> heirList = new List<Heir>();

            foreach (var heir in Heirs)
            {
                if (heir != null && heir.HeirType == HeirType.Kid)
                {
                    heirList.Add(heir);
                }
            }

            return heirList;
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

        public void GenerateHeir(HeirType type)
        {
            if (type == HeirType.Kid)
            {
                Heir h = new Heir { HeirType = HeirType.Kid };
                h.MaxInheritancePercentage = KidCalculations.MaxPercentage();
                Heirs.Add(h);
            }

            else if (type == HeirType.Spouse)
            {
                if (!Heirs.Any(s => s.HeirType == HeirType.Spouse))
                {
                    Heir h = new Heir { HeirType = HeirType.Spouse };
                    h.MaxInheritancePercentage = SpouseCalculations.MaxPercentage();
                    Heirs.Add(h);
                    Spouse = h;
                }
            }

            else if (type == HeirType.Other)
            {
                Heir h = new Heir { HeirType = HeirType.Other };
                Heirs.Add(h);
            }

        }

        public void DeleteKid(Heir heir)
        {
            Heirs.Remove(heir);

            foreach (var h in Heirs)
            {
                h.MaxInheritancePercentage -= KidCalculations.MaxPercentage();
            }
        }

    }
}
