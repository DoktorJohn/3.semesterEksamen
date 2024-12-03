using SelectPdf;
using AdvokatenBlazor.Model;
using AdvokatenBlazor.ViewModel;

namespace AdvokatenBlazor.Helper
{
    public static class PDFHelper
    {
        public static string HtmlToConvert { get; set; } = string.Empty;

        public static void GenerateContent()
        {
            int index = 0;

            HtmlToConvert +=
                $"Klientnavn: {Client.Name} <br>" +
                $"Klient i ægteskab? {Client.Married} <br><br><br>";

            foreach (var heir in HeirRepository.Instance.Heirs)
            {
                index++;

                HtmlToConvert +=
                    $"Arving nummmer: {index} <br>" +
                    $"Arving navn: {heir.Name} <br>" +
                    $"Arvingtype: {heir.HeirType} <br>" +
                    $"Arvefordeling {heir.MinInheritancePercentage}% - {heir.MaxInheritancePercentage}% <br>" +
                    $"Arvebeløb {heir.MinInheritanceAmount}DKK - {heir.MaxInheritanceAmount}DKK <br><br>";
            }
        }

        public static string GenerateFileName()
        {
            Random randomizer = new Random();
            int num_random = randomizer.Next(1, 10000);

            return $"ArvEksport{Client.Name}_{num_random}";
        }

        public static void MakePDF()
        {
            GenerateContent();
            var converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(HtmlToConvert);
            doc.Save($"{GenerateFileName()}.pdf".ToString());
            doc.Close();
        }

    }
}
