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
			foreach (var c in countries)
			{
				bool isEqual = false;
				foreach (var s in sortedCountries)
				{
					if (s == c) { isEqual = true; break; }
				}
				Assert.IsTrue(isEqual, $"getCountries returns {c} not sorted");
			}
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
			string validCountryName = "France"; //Warning : Fix value in the file
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

		[TestMethod]
		public void TestGetCountryXPos()
		{
			// Arrange
			var form = new Form1();
			string validCountryName = "France"; // Warning : Fix value in the file
			string invalidCountryName = "invalid input";

			int expectedLength = 64;

			// Act
			var positions = form.getCountryXPos(validCountryName, expectedLength);

			// Assert
			Assert.IsNotNull(positions, "GetCountryXPos doesn't return anything");
			Assert.AreEqual(expectedLength, positions.Count, "GetCountryXPos doesn't return the right amount of point");
			Assert.IsTrue(positions.All(pos => pos >= 0), "GetCountryXPos return negative value");
		}

		[TestMethod]
		public void TestGetYearData()
		{
			// Arrange
			var form = new Form1();
			const int BEGINNINGYEAR = 1960; //Warning : Fix value depending the file
			const int ENDINGYEAR = 2023; //Warning : Fix value depending the file
			int expectedCount = ENDINGYEAR - BEGINNINGYEAR + 1; // Nombre d'années entre 1960 et 2022
			var yearsFromConst = Enumerable.Range(BEGINNINGYEAR, expectedCount);

			// Act
			var years = form.getYearData();

			// Assert
			Assert.IsNotNull(years, "getYearData doesn't return any value");
			Assert.AreEqual(expectedCount, years.Count, "getYearData doesn't return the right number of value");
			foreach ( var y in years )
			{
				bool isIn = false;
				foreach ( var x in yearsFromConst )
				{
					if (x == y ) { isIn = true; break; }
				}
				Assert.IsTrue(isIn, $"getYearData returns {y}");
			}
		}
	}
}