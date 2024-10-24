using plot_that_lines;
using System.Windows.Forms;

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

		[TestMethod]
		public void TestGetCurrencies()
		{
			// Arrange
			var form = new Form1();

			// Act
			var actualCurrencies = form.GetCurrencies();

			// Assert
			Assert.IsNotNull(actualCurrencies, "GetCurrencies do not return anything");
			Assert.IsTrue(actualCurrencies.Count > 0, "GetCurrencies do not return any value");
			Assert.IsTrue(actualCurrencies.All(cur => cur.Length <= 3), "GetCurrencies return currency longer than 3 characters");
		}

		[TestMethod]
		public void TestGetCurrency()
		{
			// Arrange
			var form = new Form1();
			string validCountryName = "France";
			string invalidCountryName = "invalid input";

			// Act
			var validCurrency = form.GetCurrency(validCountryName);
			var invalidCurrency = form.GetCurrency(invalidCountryName);

			// Assert
			Assert.IsNotNull(validCurrency, "GetCurrency do not return anything");
			Assert.IsTrue(validCurrency.Length > 0, "GetCurrency return empty value");
			Assert.AreEqual("FRA", validCurrency); //Warning : Fix value in the file
			Assert.IsNull(invalidCurrency, "GetCurrency always return something");
		}
	}
}