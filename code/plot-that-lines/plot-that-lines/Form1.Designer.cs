using System.Diagnostics;
using System.Linq;

namespace plot_that_lines
{
    partial class Form1
    {
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
            // 
            // label1
            // 
            Label label1 = new Label()
            {
                AutoSize = true,
                Location = new Point(90, 89),
                Name = "label1",
                Size = new Size(603, 45),
                TabIndex = 1,
                Text = "Bienvenue, cette application permet d'afficher les dépenses militaires de pays dans leurs monnaies respectives.\n" +
				"Cliquer sur le nom d'un pays pour ouvrir une nouvelle fenêtre avec un graphique des dépenses militaires du pays.\n" +
				"Dans une fenêtre avec un graphique faites glisser votre souris sur un point pour afficher l'année et les dépenses de l'année.",
		};
            // 
            // Titre
            // 
            Label Titre = new Label()
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 24F),
                Location = new Point(275, 23),
                Name = "Titre",
                Size = new Size(215, 45),
                TabIndex = 2,
                Text = "Plot that lines",
            };
            //
            //ListBox
            //
            ListBox listBox = new ListBox()
            {
                Location = new Point(150, 150),
                Width = 500,
                Height = 300,
            };
			listBox.SelectedIndexChanged += new EventHandler(button_clicked);

            List<string> countries = GetCountries();
            countries.ForEach(country => listBox.Items.Add(country));

            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(800, 450);
            Controls.Add(Titre);
            Controls.Add(listBox);
            Controls.Add(label1);

            Name = "Plot that lines";
            Text = "Plot that lines";
            ResumeLayout(false);
            PerformLayout();
        }

        private void button_clicked(object sender, EventArgs e)
        {
            if (sender is ListBox listBox)
            {
				string selectedCountry = listBox.SelectedItem.ToString();
				Debug.WriteLine($"Selected Country: {selectedCountry}");
				Form2 form2 = new Form2(selectedCountry);
                form2.ShowDialog();
            }
        }

		private List<string> GetCountries()
		{
            const string filePath = "../../../../data/API_MS.MIL.XPND.CN_DS2_fr_csv_v2_3446916.csv";

			return File.ReadLines(filePath).Skip(1).Select(line => line.Replace("\"", "").Split(",")[0]).OrderBy(country => country).ToList();
		}
	}
}
