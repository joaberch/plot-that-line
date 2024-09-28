using ScottPlot.WinForms;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace plot_that_lines
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent(string headerTitle)
        {
            //Form2
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Text = headerTitle;

            //ScottPlott
            SuspendLayout();
            FormsPlot formsPlot = new FormsPlot
            {
                DisplayScale = 10000,
                Location = new Point(100, 50),
                Name = headerTitle,
                Size = new Size(800, 400),
                TabIndex = 0,
            };
            ResumeLayout(false);

            List<double> xPos = getYearData();
            List<double> yPos = getCountryXPos(headerTitle);
            List<double> filteredXPos = new List<double>();
            List<double> filteredYPos = new List<double>();

			for (int i = 0; i < yPos.Count; i++)
			{
				if (yPos[i] != 0)
				{
					filteredXPos.Add(xPos[i]);
					filteredYPos.Add(yPos[i]);
				}
			}

			formsPlot.Plot.Add.Scatter(filteredXPos, filteredYPos);
            formsPlot.Plot.XLabel("Année");
            formsPlot.Plot.YLabel("Dépense militaire (selon unités de devises locales)");
            formsPlot.Plot.Title(headerTitle);

            //add to forms
            Controls.Add(formsPlot);
        }

        private List<double> getCountryXPos(string name)
        {
            List<double> xPos = new List<double>();
            const string filePath = "../../../../data/API_MS.MIL.XPND.CN_DS2_fr_csv_v2_3446916.csv";

            List<string> lines = new List<string>(File.ReadAllLines(filePath));
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
                } catch
                {
                    Debug.WriteLine(item);
                }
			}
            return xPos;
        }
        /// <summary>
        /// Get the 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<double> getYearData()
        {
            List<double> yPos = new List<double>();
			const string filePath = "../../../../data/API_MS.MIL.XPND.CN_DS2_fr_csv_v2_3446916.csv";

			string[] lines = new List<string>(File.ReadAllLines(filePath)).FirstOrDefault().Split(",");
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
	}
}