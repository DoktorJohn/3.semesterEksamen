using AdvokatenBlazor;
using AdvokatenBlazor.Model;

namespace Krav_1___tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GenerateHeirs()
        {
            //Arrange
            Heir h = new Heir();
            h.Name = "A";
            h.HeirType = HeirType.Spouse;
            h.InheritancePercentage = 100;

            //Act


        }
    }
}