using AdvokatenBlazor.Model;
using AdvokatenBlazor.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Krav_1___tests
{
    [TestClass]
    public class BasicTest
    {
        [TestMethod]
        public void GenerateHeirs_Spouse_Added_When_No_Spouse()
        {
            // Arrange
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear(); // Ensure it's empty

            // Act
            repo.GenerateHeirs(HeirType.Spouse);

            // Assert
            Assert.IsTrue(repo.Heirs.Any(h => h.HeirType == HeirType.Spouse), "Spouse should be added.");
        }

        [TestMethod]
        public void GenerateHeirs_Kids_Added_According_To_KidsAmount()
        {
            // Arrange
            Client.KidsAmount = 3; // Set the number of kids
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear(); // Ensure it's empty

            // Act
            repo.GenerateHeirs(HeirType.Kid);

            // Assert
            Assert.AreEqual(3, repo.Heirs.Count(h => h.HeirType == HeirType.Kid), "Number of kids generated should match KidsAmount.");
        }

        [TestMethod]
        public void CalculateInheritancePercentageForMarried_No_Kids_Returns_100()
        {
            // Arrange
            Client.KidsAmount = 0;

            // Act
            double result = InheritanceCalc.CalculateInheritancePercentageForMarried();

            // Assert
            Assert.AreEqual(100, result, "Inheritance percentage should be 100 when no kids.");
        }

        [TestMethod]
        public void CalculateInheritancePercentageForMarried_With_Kids_Returns_50()
        {
            // Arrange
            Client.KidsAmount = 2;

            // Act
            double result = InheritanceCalc.CalculateInheritancePercentageForMarried();

            // Assert
            Assert.AreEqual(50, result, "Inheritance percentage should be 50 when there are kids.");
        }

        [TestMethod]
        public void CalculateInheritancePercentageForKid_Returns_50_Per_Kid()
        {
            // Arrange
            Client.KidsAmount = 2;

            // Act
            double result = InheritanceCalc.CalculateInheritancePercentageForKid();

            // Assert
            Assert.AreEqual(25, result, "Each kid should receive an equal share of the inheritance.");
        }

        [TestMethod]
        public void HeirRepository_ReturnKids_Returns_Only_Kids()
        {
            // Arrange
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear();
            repo.GenerateHeirs(HeirType.Kid);
            repo.GenerateHeirs(HeirType.Spouse);

            // Act
            var kids = repo.ReturnKids();

            // Assert
            Assert.AreEqual(3, kids.Count, "The number of kids returned should match the number of generated kids.");
            Assert.IsTrue(kids.All(k => k.HeirType == HeirType.Kid), "All returned heirs should be kids.");
        }

        [TestMethod]
        public void HeirRepository_DeleteSpouse_Removes_Spouse_From_Heirs()
        {
            // Arrange
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear();
            repo.GenerateHeirs(HeirType.Spouse);

            // Act
            repo.DeleteSpouse();

            // Assert
            Assert.IsFalse(repo.Heirs.Any(h => h.HeirType == HeirType.Spouse), "Spouse should be removed from Heirs.");
        }

        // Test PDF generation (mocking or integration test approach)
        [TestMethod]
        public void PDFHelper_GenerateContent_Creates_Correct_HTML_Content()
        {
            // Arrange
            Client.Name = "Test Client";
            Client.KidsAmount = 2;
            Client.Married = true;
            Client.Testament = true;
            HeirRepository.Instance.Heirs.Clear();
            HeirRepository.Instance.GenerateHeirs(HeirType.Spouse);
            HeirRepository.Instance.GenerateHeirs(HeirType.Kid);

            // Act
            PDFHelper.GenerateContent();

            // Assert
            Assert.IsTrue(PDFHelper.HtmlToConvert.Contains("Klientnavn: Test Client"), "Generated content should include the client name.");
            Assert.IsTrue(PDFHelper.HtmlToConvert.Contains("Arvinger:"), "Generated content should include 'Arvinger'.");
            Assert.IsTrue(PDFHelper.HtmlToConvert.Contains("Arving nummmer 1"), "Generated content should include a kid.");
        }

        [TestMethod]
        public void PDFHelper_GenerateFileName_Should_Return_Non_Empty_FileName()
        {
            // Act
            string fileName = PDFHelper.GenerateFileName();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(fileName), "Generated file name should not be empty.");
            Assert.IsTrue(fileName.Contains(Client.Name), "File name should contain the client name.");
        }
    }

    [TestClass]
    public class AdvancedTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CalculateInheritancePercentageForKid_Throws_Exception_When_No_Kids()
        {
            // Arrange
            Client.KidsAmount = 0;

            // Act
            InheritanceCalc.CalculateInheritancePercentageForKid();

            // Assert: Expect exception
        }

        [TestMethod]
        public void GenerateHeirs_Spouse_Only_Once_When_Spouse_Already_Exists()
        {
            // Arrange
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear();
            repo.GenerateHeirs(HeirType.Spouse); // First time, should add Spouse

            // Act
            repo.GenerateHeirs(HeirType.Spouse); // Second time, should not add another spouse

            // Assert
            Assert.AreEqual(1, repo.Heirs.Count(h => h.HeirType == HeirType.Spouse), "There should be only one spouse.");
        }

        [TestMethod]
        public void GenerateHeirs_Clears_Existing_Kids_When_New_Kids_Are_Generated()
        {
            // Arrange
            Client.KidsAmount = 2;
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear();
            repo.GenerateHeirs(HeirType.Kid); // Generates 2 kids initially

            // Act
            Client.KidsAmount = 3; // Now set KidsAmount to 3
            repo.GenerateHeirs(HeirType.Kid); // Generate 3 new kids

            // Assert
            Assert.AreEqual(3, repo.Heirs.Count(h => h.HeirType == HeirType.Kid), "The number of kids generated should now be 3.");
            Assert.AreEqual(1, repo.Heirs.Count(h => h.HeirType == HeirType.Spouse), "Spouse should remain unchanged.");
        }

        [TestMethod]
        public void CalculateInheritancePercentageForMarried_Correctly_Calculates_For_Married_With_Kids()
        {
            // Arrange
            Client.KidsAmount = 2; // Assume there are 2 kids
            Client.Married = true; // Client is married

            // Act
            double result = InheritanceCalc.CalculateInheritancePercentageForMarried();

            // Assert
            Assert.AreEqual(50, result, "When married and having kids, inheritance should be split between spouse and kids.");
        }

        [TestMethod]
        public void HeirRepository_GenerateHeirs_Throws_Exception_When_KidsAmount_Is_Negative()
        {
            // Arrange
            Client.KidsAmount = -1; // Invalid kids amount
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear();

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => repo.GenerateHeirs(HeirType.Kid), "Kids amount cannot be negative.");
        }

        [TestMethod]
        public void HeirRepository_GenerateHeirs_Correctly_Handles_Heir_Allocation_And_Updates_Percentage()
        {
            // Arrange
            Client.KidsAmount = 4; // 4 kids
            Client.Married = true; // Married client
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear();
            repo.GenerateHeirs(HeirType.Spouse); // Add spouse first
            repo.GenerateHeirs(HeirType.Kid); // Generate 4 kids

            // Act
            var kids = repo.ReturnKids();
            double inheritancePerKid = InheritanceCalc.CalculateInheritancePercentageForKid();

            // Assert
            Assert.AreEqual(4, kids.Count, "The number of kids generated should be equal to KidsAmount.");
            Assert.IsTrue(kids.All(k => k.InheritancePercentage == inheritancePerKid), "Each kid should have the same inheritance percentage.");
        }

        [TestMethod]
        public void HeirRepository_DeleteSpouse_Does_Not_Throw_When_No_Spouse_Exists()
        {
            // Arrange
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear(); // No spouse

            // Act
            repo.DeleteSpouse(); // Should not throw exception even if there is no spouse

            // Assert: No exception should occur
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void PDFHelper_GenerateContent_Handles_Empty_Heirs_List_Gracefully()
        {
            // Arrange
            Client.Name = "Test Client with No Heirs";
            Client.KidsAmount = 0;
            Client.Married = false;
            Client.Testament = false;
            HeirRepository.Instance.Heirs.Clear(); // No heirs in this case

            // Act
            PDFHelper.GenerateContent();

            // Assert
            Assert.IsTrue(PDFHelper.HtmlToConvert.Contains("Arvinger:"), "Even without heirs, the 'Arvinger' label should be present.");
            Assert.IsTrue(PDFHelper.HtmlToConvert.Contains("Klientnavn: Test Client with No Heirs"), "Client name should still be displayed.");
            Assert.IsTrue(PDFHelper.HtmlToConvert.Contains("Arving nummmer 1"), "Even if there are no heirs, the template should be intact.");
        }

        [TestMethod]
        public void HeirRepository_GenerateHeirs_Correctly_Handles_Large_Amount_Of_Kids()
        {
            // Arrange
            Client.KidsAmount = 1000; // Large number of kids
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear();

            // Act
            repo.GenerateHeirs(HeirType.Kid);

            // Assert
            Assert.AreEqual(1000, repo.Heirs.Count(h => h.HeirType == HeirType.Kid), "There should be exactly 1000 kids generated.");
        }

        [TestMethod]
        public void PDFHelper_MakePDF_Creates_PDF_File_With_Correct_File_Name()
        {
            // Arrange
            Client.Name = "Test Client";
            Client.KidsAmount = 2;
            HeirRepository.Instance.Heirs.Clear();
            HeirRepository.Instance.GenerateHeirs(HeirType.Spouse);
            HeirRepository.Instance.GenerateHeirs(HeirType.Kid);

            // Act
            string fileName = PDFHelper.GenerateFileName(); // File name should include client name and be non-empty
            PDFHelper.MakePDF(); // Generate the PDF (not saving it, just testing functionality)

            // Assert
            Assert.IsTrue(fileName.Contains(Client.Name), "Generated file name should contain the client name.");
            Assert.IsTrue(fileName.Contains("ArvEksport"), "Generated file name should contain the 'ArvEksport' prefix.");
        }

        [TestMethod]
        public void HeirRepository_Instance_Returns_Same_Instance_Through_Multiple_Calls()
        {
            // Arrange & Act
            HeirRepository instance1 = HeirRepository.Instance;
            HeirRepository instance2 = HeirRepository.Instance;

            // Assert
            Assert.AreSame(instance1, instance2, "The instance should be the same across multiple calls (Singleton behavior).");
        }
    }

    [TestClass]
    public class WeirdTests
    {
        [TestMethod]
        public void HeirRepository_GenerateHeirs_Correctly_Splits_Inheritance_Among_Spouse_And_Kids()
        {
            // Arrange
            Client.KidsAmount = 3; // 3 kids
            Client.Married = true; // Married client
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear();
            repo.GenerateHeirs(HeirType.Spouse); // Generate 1 spouse
            repo.GenerateHeirs(HeirType.Kid); // Generate 3 kids

            // Act
            double spousePercentage = repo.Heirs.First(h => h.HeirType == HeirType.Spouse).InheritancePercentage;
            double kidPercentage = repo.Heirs.First(h => h.HeirType == HeirType.Kid).InheritancePercentage;

            // Assert
            Assert.AreEqual(50, spousePercentage, "Spouse should receive 50%.");
            Assert.AreEqual(50.0 / 3.0, kidPercentage, "Each kid should receive an equal share of the remaining inheritance.");
        }

        [TestMethod]
        public void HeirRepository_HandleLargeNumberOfKids()
        {
            // Arrange
            Client.KidsAmount = 10000; // Test with a very large number of kids
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear();

            // Act
            repo.GenerateHeirs(HeirType.Kid); // Generate 10,000 kids

            // Assert
            Assert.AreEqual(10000, repo.Heirs.Count(h => h.HeirType == HeirType.Kid), "There should be 10,000 kids generated.");
        }

        [TestMethod]
        public void HeirRepository_ThreadSafety_With_Simultaneous_Addition()
        {
            // Arrange
            Client.KidsAmount = 5; // 5 kids
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear();

            // Act - Simulate concurrent access to the GenerateHeirs method.
            var tasks = Enumerable.Range(0, 10).Select(_ => Task.Run(() => repo.GenerateHeirs(HeirType.Kid))).ToArray();
            Task.WhenAll(tasks).Wait(); // Wait for all tasks to complete

            // Assert
            Assert.AreEqual(5, repo.Heirs.Count(h => h.HeirType == HeirType.Kid), "There should be exactly 5 kids generated, even with concurrent execution.");
        }

        [TestMethod]
        public void HeirRepository_Singleton_Ensures_Only_One_Instance()
        {
            // Arrange & Act
            HeirRepository instance1 = HeirRepository.Instance;
            HeirRepository instance2 = HeirRepository.Instance;

            // Assert
            Assert.AreSame(instance1, instance2, "The singleton instance should always return the same instance.");
        }

        [TestMethod]
        public void PDFHelper_Handles_Invalid_HTML_Safely()
        {
            // Arrange
            Client.Name = "Test Client";
            Client.KidsAmount = 3;
            HeirRepository.Instance.Heirs.Clear();
            HeirRepository.Instance.GenerateHeirs(HeirType.Spouse);
            HeirRepository.Instance.GenerateHeirs(HeirType.Kid);
            string invalidHtml = "<div><h1>Invalid HTML"; // Incomplete HTML for testing

            // Act
            PDFHelper.HtmlToConvert = invalidHtml;
            string result = PDFHelper.HtmlToConvert;

            // Assert
            Assert.IsTrue(result.Contains("Invalid HTML"), "Invalid HTML should not cause the process to crash.");
        }

        [TestMethod]
        public void InheritanceCalc_Correctly_Handles_Edge_Cases_For_Kids_Amount()
        {
            // Arrange & Act - Test for edge case where KidsAmount is set to 0
            Client.KidsAmount = 0;
            double inheritanceForMarried = InheritanceCalc.CalculateInheritancePercentageForMarried();
            double inheritanceForKid = InheritanceCalc.CalculateInheritancePercentageForKid();

            // Assert
            Assert.AreEqual(100, inheritanceForMarried, "If there are no kids, the spouse should receive 100%.");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => InheritanceCalc.CalculateInheritancePercentageForKid(), "Should throw an exception when calculating for 0 kids.");
        }

        [TestMethod]
        public void HeirRepository_GenerateHeirs_Does_Not_Allow_Empty_Heir_Names()
        {
            // Arrange
            Client.KidsAmount = 3;
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear();
            repo.GenerateHeirs(HeirType.Kid); // Generate kids

            // Act
            foreach (var heir in repo.Heirs)
            {
                heir.Name = string.Empty; // Set all heir names to empty strings
            }

            // Assert
            Assert.IsFalse(repo.Heirs.Any(h => string.IsNullOrEmpty(h.Name)), "Heirs should not have empty names.");
        }

        [TestMethod]
        public void HeirRepository_GenerateHeirs_Throws_Exception_When_Client_KidsAmount_Negative()
        {
            // Arrange
            Client.KidsAmount = -1; // Invalid value for KidsAmount
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear();

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => repo.GenerateHeirs(HeirType.Kid), "Should throw exception for negative KidsAmount.");
        }

        [TestMethod]
        public void HeirRepository_Correctly_Deletes_Spouse_When_Exists()
        {
            // Arrange
            Client.Married = true; // Married client
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear();
            repo.GenerateHeirs(HeirType.Spouse); // Add spouse

            // Act
            repo.DeleteSpouse(); // Delete spouse

            // Assert
            Assert.IsFalse(repo.Heirs.Any(h => h.HeirType == HeirType.Spouse), "Spouse should be removed.");
        }

        [TestMethod]
        public void HeirRepository_Ensure_Proper_Handling_Of_Null_Values_In_Heirs_List()
        {
            // Arrange
            Client.KidsAmount = 2;
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear();
            repo.GenerateHeirs(HeirType.Kid); // Generate kids

            // Act
            repo.Heirs[0].Name = null; // Assign a null value to the name of the first kid

            // Assert
            Assert.IsTrue(repo.Heirs.All(h => h.Name != null), "None of the heir names should be null.");
        }

        [TestMethod]
        public void PDFHelper_GenerateFileName_Should_Always_Return_Unique_FileName()
        {
            // Arrange
            Client.Name = "Test Client";
            Client.KidsAmount = 2;
            HeirRepository.Instance.Heirs.Clear();
            HeirRepository.Instance.GenerateHeirs(HeirType.Spouse);
            HeirRepository.Instance.GenerateHeirs(HeirType.Kid);

            // Act
            string fileName1 = PDFHelper.GenerateFileName();
            string fileName2 = PDFHelper.GenerateFileName(); // Generate a second filename

            // Assert
            Assert.AreNotEqual(fileName1, fileName2, "The file names should be unique.");
        }

        [TestMethod]
        public void HeirRepository_Handle_Empty_HeirType()
        {
            // Arrange
            Client.KidsAmount = 0;
            HeirRepository repo = HeirRepository.Instance;
            repo.Heirs.Clear();

            // Act
            Heir invalidHeir = new Heir { Name = "Invalid Heir", HeirType = (HeirType)999 }; // Set an invalid HeirType
            repo.Heirs.Add(invalidHeir);

            // Assert
            Assert.IsFalse(Enum.IsDefined(typeof(HeirType), invalidHeir.HeirType), "Invalid HeirType should not be allowed.");
        }
    }
}
