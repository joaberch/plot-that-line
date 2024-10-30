﻿using Microsoft.VisualBasic.ApplicationServices;
using ScottPlot.WinForms;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace plot_that_lines
{
    partial class Form1
    {
        //TODO : add certain const in the .env instead of there
        const string FILEPATH = "../../../../data/API_MS.MIL.XPND.CN_DS2_fr_csv_v2_3446916.csv"; //path of the data file
        const int BEGINNINGYEAR = 1960; //year we start collecting data
        const int ENDINGYEAR = 2022;    //year we stop collecting data
        const string ENVFILEPATH = ".env"; //path of the .env file

        List<string> selectedCountries = new List<string>(); //list of the countries selected to display

        FormsPlot formsPlot;
        ComboBox comboBox;

        int beginFilter = BEGINNINGYEAR;
        int endFilter = ENDINGYEAR;

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            //ScottPlott
            formsPlot = new FormsPlot
            {
                DisplayScale = 100,
                Location = new Point(100, 20),
                Size = new Size(600, 300),
                TabIndex = 0,
            };
            formsPlot.Plot.XLabel("Année");
            formsPlot.Plot.YLabel("Dépense militaire");
            formsPlot.Plot.Title("Aucun pays sélectionné");
            formsPlot.Plot.Axes.SetLimitsX(BEGINNINGYEAR - 5, ENDINGYEAR + 5);

            // explanation 
            Label explanation = new Label()
            {
                AutoSize = true,
                Location = new Point(90, 380),
                Name = "label1",
                Size = new Size(603, 45),
                TabIndex = 1,
                Text = "Bienvenue dans cette application d'affichage des dépenses militaires par pays.\n" +
                "Vous pouvez afficher les dépenses militaires d'un pays dans sa devise d'origine ou les convertir dans une autre devise au choix.\n" +
                "Sélectionnez un pays pour ouvrir une fenêtre avec un graphique détaillant ses dépenses militaires par année.\n" +
                "Dans la fenêtre graphique, passez la souris sur un point pour afficher l'année et le montant des dépenses de cette année.\n" +
                "Vous pouvez également ajuster les années de début et de fin pour filtrer les données affichées et sélectionner une devise de conversion via le menu déroulant.",
            };

            // title
            Label title = new Label()
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 24F),
                Location = new Point(275, 330),
                Name = "Titre",
                Size = new Size(215, 45),
                TabIndex = 2,
                Text = "Plot that lines",
            };

            //ListBox
            ListBox listBox = new ListBox()
            {
                Location = new Point(700, 150),
                Width = 200,
                Height = 200,
            };
            listBox.SelectedIndexChanged += new EventHandler(button_clicked);

            if (!File.Exists(FILEPATH))
            {
                MessageBox.Show("Fichier csv introuvable");
            }
            else
            {
                List<string> countries = GetCountries();
                countries.ForEach(country => listBox.Items.Add(country));
            }


            //Label beginFilter
            Label beginFilterLabel = new Label()
            {
                Name = "beginFilter",
                Text = "Année de début",
                Location = new System.Drawing.Point(700, 50)
            };
            //TextBox beginFilter
            TextBox beginFilter = new TextBox()
            {
                Location = new System.Drawing.Point(700, 70),
                PlaceholderText = $"Date de début ({BEGINNINGYEAR})",
                Width = 150,
            };
            //beginFilter.TextChanged += new EventHandler();

            //Label endFilter
            Label endFilterLabel = new Label()
            {
                Name = "endFilter",
                Text = "Année de fin",
                Location = new System.Drawing.Point(700, 90)
            };
            //TextBox beginFilter
            TextBox endFilter = new TextBox()
            {
                Location = new System.Drawing.Point(700, 110),
                PlaceholderText = $"Date de fin ({ENDINGYEAR})",
                Width = 150,
            };
            //endFilter.TextChanged += new EventHandler((sender, e) => ChangeFilter(sender, e, headerTitle));

            //currency Label
            Label currencyLabel = new Label()
            {
                Text = "Devise",
                Location = new System.Drawing.Point(820, 351)
            };

            //ComboBox
            comboBox = new ComboBox()
            {
                Location = new System.Drawing.Point(700, 350),
                DropDownHeight = 300,
            };

            if (File.Exists(FILEPATH))
            {
                List<string> currencies = GetCurrencies();
                currencies.ForEach(cur => comboBox.Items.Add(cur));
            }

            // Form1
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1000, 450);

            Controls.Add(formsPlot);
            Controls.Add(title);
            Controls.Add(listBox);
            Controls.Add(explanation);
            Controls.Add(beginFilterLabel);
            Controls.Add(beginFilter);
            Controls.Add(endFilterLabel);
            Controls.Add(endFilter);
            Controls.Add(currencyLabel);
            Controls.Add(comboBox);

            Name = "Plot that lines";
            Text = "Plot that lines";
            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>
        /// When a country is selected in the comboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_clicked(object sender, EventArgs e)
        {
            if (sender is not ListBox listBox) return;

            string selectedItem = listBox.SelectedItem.ToString();

            // Update selected countries
            List<string> updatedCountries = selectedCountries.Contains(selectedItem)
                ? selectedCountries.Where(country => country != selectedItem).ToList()
                : selectedCountries.Append(selectedItem).ToList();

            selectedCountries.Clear();
            selectedCountries.AddRange(updatedCountries);

            // Update title
            string countryList = selectedCountries.Count == 0
                ? "Aucun pays sélectionné"
                : string.Join(", ", selectedCountries);

            formsPlot.Plot.Title(countryList);

            formsPlot.Plot.Clear();

            //Check if the user has selected a currency
            if (comboBox.SelectedItem == null)
            {
                MessageBox.Show("Merci de sélectionner une devise");
                return;
            }

            //Check if a .env file exist
            if (!File.Exists(ENVFILEPATH))
            {
                MessageBox.Show("Fichier .env introuvable");
                return;
            }

            foreach (var country in selectedCountries)
            {
                addPoint(country);
            }

            //Scale the plot
            autoScalePlot();
            formsPlot.Refresh();
        }

        /// <summary>
        /// Get the currency type of the country
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        public string GetCurrencyForCountry(string countryName)
        {
            foreach (var line in File.ReadLines(FILEPATH))
            {
                if (line.Contains(countryName))
                {
                    var currency = line.Split("\",")[1].Split("\"")[1];
                    return currency;
                }
            }
            return null;
        }

        /// <summary>
        /// Display only one graph with the local data of the country
        /// </summary>
        /// <param name="countryName"></param>
        /// <param name="filteredPoints"></param>
        private void addLocalPoint(string countryName, List<(double X, double Y)> filteredPoints)
        {
            selectedCountries.Clear();
            formsPlot.Plot.Add.Scatter(
                filteredPoints.Select(p => p.X).ToArray(),
                filteredPoints.Select(p => p.Y).ToArray()
            );
            formsPlot.Refresh();
        }

        /// <summary>
        /// Display the country with the amount converted to the currency wanted
        /// </summary>
        /// <param name="countryName"></param>
        private async void addPoint(string countryName)
        {
            string inputCurrency = GetCurrencyForCountry(countryName);

            string convertToCurrency = comboBox.SelectedItem.ToString();

            List<double> xPos = getYearData();
            List<double> yPos = getCountryXPos(countryName, xPos.Count());

            List<(double X, double Y)> filteredPoints = xPos.Zip(yPos, (x, y) => (X: x, Y: y))
                .Where(point => point.Y != 0 && point.X <= endFilter && point.X >= beginFilter)
                .ToList();

            List<(double x, double y)> filteredConvertedPoints = await ConvertCurrency(filteredPoints, inputCurrency, convertToCurrency);

            if (filteredConvertedPoints.Any())
            {
                formsPlot.Plot.Add.Scatter(
                    filteredConvertedPoints.Select(p => p.x).ToArray(),
                    filteredConvertedPoints.Select(p => p.y).ToArray()
                );
                formsPlot.Refresh();
            }
            else
            {
                MessageBox.Show("Problème avec l'API, affichage du graphique dans la devise locale du pays sélectionné");
                addLocalPoint(countryName, filteredPoints);
            }
        }

        /// <summary>
        /// Convert value depending with the currency wanted
        /// </summary>
        /// <param name="points"></param>
        /// <param name="inputCurrency"></param>
        /// <param name="convertToCurrency"></param>
        /// <returns></returns>
        static async Task<List<(double, double)>> ConvertCurrency(List<(double, double)> points, string inputCurrency, string convertToCurrency)
        {
            //TODO : only one request and then use the rate
            string apiKey = "0";
            StreamReader sr2 = new StreamReader(".env");
            string line2;
            List<(double x, double y)> convertedPoints = new List<(double x, double y)>();

            while ((line2 = sr2.ReadLine()) != null)
            {
                if (line2.Contains("APIKEY"))
                {
                    apiKey = line2.Split("=").Last();
                }
            }

            try
            {
                using var client = new HttpClient();
                foreach (var point in points)
                {
                    string url = $"https://api.getgeoapi.com/v2/currency/convert?api_key={apiKey}&from={inputCurrency}&to={convertToCurrency}&amount={point.Item2}&format=json";
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Response : {responseContent}");

                        string key = "rate_for_amount\":\"";
                        double rateForAmount = Convert.ToDouble(responseContent.Split(key)[1].Split("\"").First(), CultureInfo.InvariantCulture);
                        double originalAmount = Convert.ToDouble(point.Item2.ToString("0.0", CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                        var value = rateForAmount * originalAmount;
                        convertedPoints.Add((point.Item1, value));
                    }
                };
                return convertedPoints;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Debug : {ex.ToString()}");
            }
            return convertedPoints;
        }

        /// <summary>
        /// Scale the graph depending on the value displayed
        /// </summary>
        private void autoScalePlot()
        {
            var allPoints = selectedCountries.SelectMany(country =>
            {
                List<double> xPos = getYearData();
                List<double> yPos = getCountryXPos(country, xPos.Count());

                return xPos.Zip(yPos, (x, y) => new { X = x, Y = y })
                            .Where(point => point.Y != 0 && point.X <= endFilter && point.X >= beginFilter);
            }).ToList();

            if (allPoints.Any())
            {
                double minX = allPoints.Min(point => point.X);
                double maxX = allPoints.Max(point => point.X);
                double maxY = allPoints.Max(point => point.Y);

                formsPlot.Plot.Axes.SetLimitsX(minX - 5, maxX + 5);
                formsPlot.Plot.Axes.SetLimitsY(-maxY * 0.1, maxY * 1.4);
            }
        }

        /// <summary>
        /// Get the data of the country from the csv
        /// </summary>
        /// <param name="name"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public List<double> getCountryXPos(string name, int length)
        {
            List<double> xPos = new List<double>();

            List<string> lines = new List<string>(File.ReadAllLines(FILEPATH));
            List<string> selectedLine = new List<string>(lines.Where(line => line.Contains(name)));

            string[] data = selectedLine[0].ToString().Split(",");

            foreach (var item in data)
            {
                try
                {
                    string pos = item.Replace("\\\"", "").Replace("\"", "").Replace(".", ",");
                    //Need to have the same x and y value
                    if (string.IsNullOrEmpty(pos)) { xPos.Add(0); };
                    xPos.Add(Convert.ToDouble(pos));
                }
                catch
                {
                    Debug.WriteLine(item);
                }
            }
            return xPos.Take(length).ToList();
        }

        /// <summary>
        /// Get the year length in which the data are displayed
        /// </summary>
        /// <returns></returns>
        public List<double> getYearData()
        {
            List<double> yPos = new List<double>();

            string[] lines = new List<string>(File.ReadAllLines(FILEPATH)).FirstOrDefault().Split(",");
            foreach (var item in lines)
            {
                try
                {
                    string year = item.Replace("\\\"", "").Replace("\"", "").Replace(".", ",");
                    yPos.Add(Convert.ToDouble(year));
                }
                catch
                {
                    Debug.WriteLine(item);
                }
            }
            return yPos;
        }

        /// <summary>
        /// Get every countries from the csv
        /// </summary>
        /// <returns></returns>
        public List<string> GetCountries()
        {
            try
            {
                return File
                    .ReadLines(FILEPATH)
                    .Skip(1)
                    .Select(line => line.Replace("\"", "").Split(",")[0])
                    .OrderBy(country => country)
                    .ToList();
            }
            catch { return null; }
        }

        /// <summary>
        /// Get every currencies from the csv
        /// </summary>
        /// <returns></returns>
        public List<string> GetCurrencies()
        {
            try
            {
                return File.ReadLines(FILEPATH)
                    .Skip(1)
                    .Select(line => line.Replace("\"", "").Split(","))
                    .Select(parts => parts[1])
                    .Distinct()
                    .OrderBy(currency => currency)
                    .Where(currency => currency.Length <= 3)
                    .ToList();
            }
            catch { return null; }
        }

        //private void ChangeFilter(object sender, EventArgs e, string headerTitle)
        //{
        //	if (sender is TextBox textbox && textbox.Text.Length > 0)
        //	{
        //		//Change year filter
        //		try
        //		{
        //			int year = Convert.ToInt32(((System.Windows.Forms.TextBox)sender).Text);

        //			textbox.ForeColor = Color.Black;
        //			if (textbox.PlaceholderText.Contains("début"))
        //			{
        //				beginFilter = year;
        //			}
        //			else if (textbox.PlaceholderText.Contains("fin"))
        //			{
        //				endFilter = year;
        //			}

        //		}
        //		catch { }
        //		//In case char
        //		StringBuilder text = new StringBuilder();
        //		foreach (char item in textbox.Text)
        //		{
        //			try
        //			{
        //				if (Char.IsDigit(item))
        //					text.Append(item.ToString());
        //			}
        //			catch { }
        //		}
        //		textbox.Text = text.ToString();

        //		//Update graph
        //		formsPlot.Plot.Clear();

        //		if (beginFilter < BEGINNINGYEAR)
        //		{
        //			beginFilter = BEGINNINGYEAR;
        //		}
        //		if (endFilter < beginFilter)
        //		{
        //			endFilter = ENDINGYEAR;
        //		}

        //		(List<double> filteredX, List<double> filteredY) filtered = addPoint(headerTitle, beginFilter, endFilter);

        //		formsPlot.Plot.Add.Scatter(filtered.filteredX, filtered.filteredY);

        //	}
        //	formsPlot.Refresh();
        //}

    }
}
