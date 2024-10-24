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
			var value = form.GetCountries();

			//Assert
			Assert.IsNotNull(value);
		}
	}
}