using ScottPlot.WinForms;
using System.Diagnostics;
using System.Linq;

namespace plot_that_lines
{
    partial class Form2
    {

        const string FILEPATH = "../../../../data/API_MS.MIL.XPND.CN_DS2_fr_csv_v2_3446916.csv";
        //TODO : automatically get first and last year
        //TODO : interface
        const int BEGINNINGYEAR = 1960; //year we start collecting data
        const int ENDINGYEAR = 2022;    //year we stop collecting data

        int beginFilter = BEGINNINGYEAR;
        int endFilter = ENDINGYEAR;

        //ScottPlott
        FormsPlot formsPlot = new FormsPlot
        {
            DisplayScale = 10000,
            Location = new Point(100, 50),
            Size = new Size(800, 400),
            TabIndex = 0,
        };

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
            this.ClientSize = new System.Drawing.Size(1200, 600);
            this.Text = headerTitle;

            //Label beginFilter
            Label beginFilterLabel = new Label()
            {
                Name = "beginFilter",
                Text = "Année de début",
                Location = new System.Drawing.Point(900, 100)
            };
            //TextBox beginFilter
            TextBox beginFilter = new TextBox()
            {
                Location = new System.Drawing.Point(900, 125),
                PlaceholderText = $"Date de début ({BEGINNINGYEAR})",
                Width = 150,
            };
            beginFilter.TextChanged += new EventHandler((sender, e) => ChangeFilter(sender, e, headerTitle));

            //Label endFilter
            Label endFilterLabel = new Label()
            {
                Name = "endFilter",
                Text = "Année de fin",
                Location = new System.Drawing.Point(900, 170)
            };
            //TextBox beginFilter
            TextBox endFilter = new TextBox()
            {
                Location = new System.Drawing.Point(900, 195),
                PlaceholderText = $"Date de fin ({ENDINGYEAR})",
                Width = 150,
            };
            endFilter.TextChanged += new EventHandler((sender, e) => ChangeFilter(sender, e, headerTitle));

            (List<double> filteredX, List<double> filteredY) filtered = AddPoint(headerTitle, BEGINNINGYEAR, ENDINGYEAR);

            formsPlot.Plot.Add.Scatter(filtered.filteredX, filtered.filteredY);
            formsPlot.Plot.Axes.SetLimits(filtered.filteredX[0] - 2, filtered.filteredX[filtered.filteredY.Count - 1] + 2);

            formsPlot.Name = headerTitle;
            formsPlot.Plot.XLabel("Année");
            formsPlot.Plot.YLabel("Dépense militaire (selon unités de devises locales)");
            formsPlot.Plot.Title(headerTitle);

            //add to forms
            Controls.Add(formsPlot);
            Controls.Add(beginFilterLabel);
            Controls.Add(beginFilter);
            Controls.Add(endFilterLabel);
            Controls.Add(endFilter);
        }

        private void ChangeFilter(object sender, EventArgs e, string headerTitle)
        {
            if (sender is TextBox textbox)
            {
                try
                {
                    int year = Convert.ToInt32(((System.Windows.Forms.TextBox)sender).Text);
                    if (year >= BEGINNINGYEAR && year <= ENDINGYEAR)
                    {
                        textbox.ForeColor = Color.Black;
                        if (textbox.PlaceholderText.Contains("début"))
                        {
                            beginFilter = year;
                            formsPlot.Plot.Clear();

                            (List<double> filteredX, List<double> filteredY) filtered = AddPoint(headerTitle, beginFilter, endFilter);

                            formsPlot.Plot.Add.Scatter(filtered.filteredX, filtered.filteredY);
                        }
                        else if (textbox.PlaceholderText.Contains("fin"))
                        {
                            endFilter = year;
                            formsPlot.Plot.Clear();

                            List<double> xPos = getYearData();
                            List<double> yPos = getCountryXPos(headerTitle, FILEPATH);
                            List<double> filteredXPos = new List<double>();
                            List<double> filteredYPos = new List<double>();

                            //var filter = yPos.Zip(xPos).Where(item => item.First != 0);
                            for (int i = 0; i < yPos.Count; i++)
                            {
                                if (yPos[i] != 0 && xPos[i] <= endFilter && xPos[i] >= beginFilter)
                                {
                                    filteredXPos.Add(xPos[i]);
                                    filteredYPos.Add(yPos[i]);
                                }
                            }

                            formsPlot.Plot.Add.Scatter(filteredXPos, filteredYPos);
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"{year} is off limit");
                    }

                }
                catch
                {

                    textbox.Text = "";
                }
            }
            formsPlot.Refresh();
        }

        private (List<double>, List<double>) AddPoint(string countryName, int beginFilter, int endFilter)
        {
            List<double> xPos = getYearData();
            List<double> yPos = getCountryXPos(countryName, FILEPATH);
            (List<double> filteredXPos, List<double> filteredYPos) filter = new(new List<double>(), new List<double>());

            //var filter = yPos.Zip(xPos).Where(item => item.First != 0);
            for (int i = 0; i < yPos.Count; i++)
            {
                if (yPos[i] != 0 && xPos[i] <= endFilter && xPos[i] >= beginFilter)
                {
                    filter.filteredXPos.Add(xPos[i]);
                    filter.filteredYPos.Add(yPos[i]);
                }
            }
            return filter;
        }

        private List<double> getCountryXPos(string name, string path)
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
            return xPos;
        }
        /// <summary>
        /// Get every years on the csv
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