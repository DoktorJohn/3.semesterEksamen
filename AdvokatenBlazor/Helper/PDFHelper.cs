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
            HtmlToConvert = string.Empty;

            // Begin building the HTML string for the table
            HtmlToConvert += @"
<div class='table table-striped'>
    <table style='border-collapse: collapse; width: 100%; text-align: left;'>
        <thead>
            <tr style='background-color: #f2f2f2;'>
                <th style='padding: 8px; border: 1px solid #ddd;'>Aktiv/passiv</th>";

            // Add headers for each heir
            foreach (var heir in HeirRepository.Instance.Heirs)
            {
                HtmlToConvert += $@"
                <th style='padding: 8px; border: 1px solid #ddd;'>{heir.Name}</th>";
            }

            HtmlToConvert += @"
                <th style='padding: 8px; border: 1px solid #ddd;'>Passiv</th>
            </tr>
        </thead>
        <tbody>";

            double totalPassive = 0;
            var heirTotals = HeirRepository.Instance.Heirs.ToDictionary(heir => heir.Id, heir => 0.0);

            // Add rows for each asset
            foreach (var asset in AssetRepository.Instance.assets)
            {
                HtmlToConvert += "<tr>";

                // Display asset name or details
                HtmlToConvert += "<td style='padding: 8px; border: 1px solid #ddd;'>";

                if (asset.AssetType == AssetType.Item || asset.AssetType == AssetType.Stock)
                {
                    HtmlToConvert += asset.Name;
                }
                else if (asset.AssetType == AssetType.Property)
                {
                    HtmlToConvert += asset.Location;
                }
                else if (asset.AssetType == AssetType.Vehicle)
                {
                    HtmlToConvert += asset.Brand;
                }
                else if (asset.AssetType == AssetType.Money)
                {
                    HtmlToConvert += "Likvide midler";
                }

                HtmlToConvert += "</td>";

                // Generate a column for each heir with calculated values
                foreach (var heir in HeirRepository.Instance.Heirs)
                {
                    HtmlToConvert += "<td style='padding: 8px; border: 1px solid #ddd;'>";

                    // Find the corresponding row for the current heir
                    var heirRow = asset.HeirRows.FirstOrDefault(row => row.SelectedHeirId == heir.Id);
                    if (heirRow != null)
                    {
                        double percentage = (double)heirRow.Percentage / 100;
                        double calculatedShare = asset.Value * percentage;
                        double calculatedDebtShare = asset.Debt * percentage; // Share of debt for this heir
                        double netValue = calculatedShare - calculatedDebtShare;

                        HtmlToConvert += netValue.ToString("N2");

                        // Add the net value to the heir total
                        heirTotals[heir.Id] += netValue;
                    }
                    else
                    {
                        HtmlToConvert += "-"; // Display a dash if no value exists
                    }

                    HtmlToConvert += "</td>";
                }

                // Add the passive (debt) column
                HtmlToConvert += $"<td style='padding: 8px; border: 1px solid #ddd;'>-{asset.Debt.ToString("N2")}</td>";

                // Add to total passive
                totalPassive += asset.Debt;

                HtmlToConvert += "</tr>";
            }

            // Add a summary row for totals
            HtmlToConvert += "<tr style='font-weight: bold; background-color: #e6e6e6;'>";

            // Empty cell for "Aktiv/passiv"
            HtmlToConvert += "<td style='padding: 8px; border: 1px solid #ddd;'>Opgjort bodel</td>";

            // Total for each heir
            foreach (var heir in HeirRepository.Instance.Heirs)
            {
                HtmlToConvert += $"<td style='padding: 8px; border: 1px solid #ddd;'>{heirTotals[heir.Id].ToString("N2")}</td>";
            }

            // Total passive (empty cell for this row)
            HtmlToConvert += "<td style='padding: 8px; border: 1px solid #ddd;'></td>";

            HtmlToConvert += "</tr>";

            HtmlToConvert += @"
        </tbody>
    </table>
</div>";
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
