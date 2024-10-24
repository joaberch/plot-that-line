using plot_that_lines;

namespace plot_that_lines_test
{
	[TestClass]
	public class UnitTest1
	{

		[TestMethod]
		public void TestGetCountries()
		{
			//Arrange
			var form = new Form1();

			//Act
			var countries = form.GetCountries();
			var sortedCountries = countries.OrderBy(x => x).ToList();

			//Assert
			Assert.IsNotNull(countries, "GetCountries do not return anything");
			Assert.IsTrue(countries.Count > 0, "GetCountries do not return any value");
			Assert.AreEqual(countries, sortedCountries, "GetCountries do not return sorted value");
		}
	}
}