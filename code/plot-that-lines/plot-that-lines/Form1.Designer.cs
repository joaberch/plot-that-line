﻿using ScottPlot.WinForms;
using System.Diagnostics;
using System.Linq;

namespace plot_that_lines
{
    partial class Form1
    {
		const string FILEPATH = "../../../../data/API_MS.MIL.XPND.CN_DS2_fr_csv_v2_3446916.csv";
		//TODO : automatically get first and last year
		//TODO : interface
		const int BEGINNINGYEAR = 1960; //year we start collecting data
		const int ENDINGYEAR = 2022;    //year we stop collecting data

        List<string> selectedCountries = new List<string>();
		FormsPlot formsPlot;

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
			formsPlot.Plot.Axes.SetLimitsX(BEGINNINGYEAR-5, ENDINGYEAR+5);

			// explanation 
			Label explanation = new Label()
            {
                AutoSize = true,
                Location = new Point(90, 380),
                Name = "label1",
                Size = new Size(603, 45),
                TabIndex = 1,
                Text = "Bienvenue, cette application permet d'afficher les dépenses militaires de pays dans leurs monnaies respectives.\n" +
				"Cliquer sur le nom d'un pays pour ouvrir une nouvelle fenêtre avec un graphique des dépenses militaires du pays.\n" +
				"Dans une fenêtre avec un graphique faites glisser votre souris sur un point pour afficher l'année et les dépenses de l'année.",
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

            List<string> countries = GetCountries();
            countries.ForEach(country => listBox.Items.Add(country));
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

            Name = "Plot that lines";
            Text = "Plot that lines";
            ResumeLayout(false);
            PerformLayout();
        }

		private void button_clicked(object sender, EventArgs e)
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

			foreach (var country in selectedCountries)
			{
				addPoint(country);
			}

			//Scale the plot
			autoScalePlot();
			formsPlot.Refresh();
		}

		private void addPoint(string countryName)
		{
			List<double> xPos = getYearData();
			List<double> yPos = getCountryXPos(countryName, FILEPATH, xPos.Count());

			var filteredPoints = xPos.Zip(yPos, (x, y) => new { X = x, Y = y })
				.Where(point => point.Y != 0 && point.X <= endFilter && point.X >= beginFilter)
				.ToList();

			if (filteredPoints.Any())
			{
				formsPlot.Plot.Add.Scatter(filteredPoints.Select(p => p.X).ToArray(), filteredPoints.Select(p => p.Y).ToArray());
			}
		}

		private void autoScalePlot()
		{
			var allPoints = selectedCountries.SelectMany(country =>
			{
				List<double> xPos = getYearData();
				List<double> yPos = getCountryXPos(country, FILEPATH, xPos.Count());

				return xPos.Zip(yPos, (x, y) => new { X = x, Y = y })
							.Where(point => point.Y != 0 && point.X <= endFilter && point.X >= beginFilter);
			}).ToList();

			if (allPoints.Any())
			{
				double minX = allPoints.Min(point => point.X);
				double maxX = allPoints.Max(point => point.X);
				double maxY = allPoints.Max(point => point.Y);

				formsPlot.Plot.Axes.SetLimitsX(minX-5, maxX+5);
				formsPlot.Plot.Axes.SetLimitsY(-maxY * 0.1, maxY * 1.4);
			}
		}

		private List<double> getCountryXPos(string name, string path, int length)
		{
			List<double> xPos = new List<double>();

			List<string> lines = new List<string>(File.ReadAllLines(path));
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
			return xPos.Take(length).ToList(); //TODO : 64 length
		}

		private List<double> getYearData()
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

		private List<string> GetCountries()
		{
			return File
                .ReadLines(FILEPATH)
                .Skip(1)
                .Select(line => line.Replace("\"", "").Split(",")[0])
                .OrderBy(country => country)
                .ToList();
		}
	}
}
