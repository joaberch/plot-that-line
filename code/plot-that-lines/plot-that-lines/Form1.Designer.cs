using ScottPlot.WinForms;
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

			// If the country is already in the list we remove it else we add it
			List<string> updatedCountries = selectedCountries.Contains(selectedItem)
				? selectedCountries.Where(country => country != selectedItem).ToList()
				: selectedCountries.Append(selectedItem).ToList();

			// Mise à jour de la liste des pays sélectionnés
			selectedCountries.Clear();
			selectedCountries.AddRange(updatedCountries);

			// Définir le titre du graphique en fonction des pays sélectionnés
			string countryList = selectedCountries.Count == 0
				? "Aucun pays sélectionné"
				: string.Join(", ", selectedCountries);

			// Mise à jour du titre et rafraîchissement du graphique
			formsPlot.Plot.Title(countryList);
			formsPlot.Refresh();
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
