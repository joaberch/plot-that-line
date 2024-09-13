using ScottPlot.WinForms;
using System.Diagnostics;

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

        #region Windows Form Designer generated code

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

            StreamReader sr = new StreamReader("API_MS.MIL.XPND.CN_DS2_fr_csv_v2_3446916.csv");
            string line;

            List<double> xPos = new List<double>();
            List<double> yPos = new List<double>();

            while ((line = sr.ReadLine()) != null)
            {
                string[] words = line.Replace("\"", "").Split(",");

                //get yPos
                if (headerTitle == words[0]) {
                    for (int i = 0;  i < words.Length-4; i++)
                    {
                        try
                        {
                            yPos.Add(Convert.ToDouble(words[i + 4]));
                        } catch {
                            Debug.WriteLine($"yPos unreadeable : \"{words[i+4]}\"");
                        }
                    }
                }

                //get xPos
                if (line.Contains("\"Country Name\",\"Country Code\",\"Indicator Name\",\"Indicator Code\""))
                {
                    for (int i = 0; i < words.Length - 4; i++)
                    {
                        try
                        {
                            xPos.Add(Convert.ToDouble(words[i + 4]));
                        }
                        catch
                        {
                            Debug.WriteLine($"xPos unreadeable : \"{words[i + 4]}\"");
                        }
                    }
                }
            }

            formsPlot.Plot.Add.ScatterLine(xPos, yPos);
            formsPlot.Plot.XLabel("Année");
            formsPlot.Plot.YLabel("Dépense militaire (selon unités de devises locales)");
            formsPlot.Plot.Title(headerTitle);

            //add to forms
            Controls.Add(formsPlot);
        }

        #endregion
    }
}