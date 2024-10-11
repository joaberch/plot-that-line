using Microsoft.VisualBasic.ApplicationServices;
using System.Diagnostics;
using System.Text.Json;

namespace plot_that_lines
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            _ = GetUserById();

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        /// <summary>
        /// This asynchronous method sends a GET request to
        /// a REST API endpoint and retrieves a user by id.
        static async Task<User?> GetUserById()
        {
            string apiKey = "0";
            StreamReader sr2 = new StreamReader(".env");
            string line2;

            while ((line2 = sr2.ReadLine()) != null)
            {
                if (line2.Contains("APIKEY")) {
                    apiKey = line2.Split(" = ").Last();
                }
            }

            try
            {
                using var client = new HttpClient();
                var url = $"https://api.getgeoapi.com/v2/currency/convert?api_key={apiKey}";

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<User>(responseContent);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        static async Task<User?> Convert(string inputCurrency, string convertToCurrency, int amount)
        {
            string apiKey = "0";
            StreamReader sr2 = new StreamReader(".env");
            string line2;

            while ((line2 = sr2.ReadLine()) != null)
            {
                if (line2.Contains("APIKEY"))
                {
                    apiKey = line2.Split(" = ").Last();
                }
            }

            try
            {
                using var client = new HttpClient();
                var url = $"https://api.getgeoapi.com/v2/currency/convert?api_key={apiKey}&from={inputCurrency}&to={convertToCurrency}&amount={amount}&format=json";

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<User>(responseContent);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}