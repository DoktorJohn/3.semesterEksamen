using AdvokatenBlazor.Model;

namespace AdvokatenBlazor.ViewModel
{
    public class HeirRepository
    {
        private static HeirRepository instance;
        
        HeirRepository()
        {
            Heirs = new List<Heir>();
        }

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


        public List<Heir> Heirs { get; set; }


        public Heir ReturnSpouse()
        {
            return Heirs.Where(s => s.HeirType == HeirType.Spouse).FirstOrDefault()!;
        }
    }
}
