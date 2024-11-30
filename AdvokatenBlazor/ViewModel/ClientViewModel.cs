using AdvokatenBlazor.Model;

namespace AdvokatenBlazor.ViewModel
{
    public class ClientViewModel
    {
        private static ClientViewModel instance;
        public static ClientViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClientViewModel();
                }

                return instance;
            }
        }

        private bool _isMarried;

        public bool IsMarried
        {
            get { return _isMarried; }
            set 
            { 
                _isMarried = value;
                Client.Married = _isMarried;
            }
        }


        private int _kidsAmount;

        public int KidsAmount
        {
            get { return _kidsAmount; }
            set 
            {
                _kidsAmount = value; 
                Client.KidsAmount = _kidsAmount;
            }
        }

        private double _totalAmount;

        public double TotalAmount
        {
            get 
            {
                _totalAmount = Client.TotalValue;
                return _totalAmount;
            }
            set { _totalAmount = value; }
        }



        //Sætter kundes ægteskabstilstand og genererer eller sletter ægtefælle.
        public void SetMaritalStatus(bool married)
        {
            IsMarried = married;

            if (Client.Married)
            {
                HeirRepository.Instance.GenerateHeir(HeirType.Spouse);
            }

            else if (!Client.Married)
            {
                HeirRepository.Instance.DeleteSpouse();
            }
        }

        //Sætter kundes antal af børn og genererer et barn.
        public void SetNumberOfKids(int numberOfKids)
        {
            KidsAmount = numberOfKids;
            HeirRepository.Instance.GenerateHeir(HeirType.Kid);
        }

        public static void UpdateAsset()
        {
            Client.TotalValue = 0;

            foreach (var asset in AssetRepository.Instance.assets)
            {
                double percentageOwnedAsDecimal = asset.PercentageOwned / 100.0;

                Client.TotalValue += asset.Value * percentageOwnedAsDecimal;
            }

        }
    }
}
