using System.Diagnostics;

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
            Label label1 = new Label();
            label1.AutoSize = true;
            label1.Location = new Point(90, 89);
            label1.Name = "label1";
            label1.Size = new Size(603, 45);
            label1.TabIndex = 1;
            label1.Text = "Bienvenue, cette application permet d'afficher les dépenses militaires de pays dans leurs monnaies respectives.\n" +
                "Cliquer sur le nom d'un pays pour ouvrir une nouvelle fenêtre avec un graphique des dépenses militaires du pays.\n" +
                "Dans une fenêtre avec un graphique faites glisser votre souris sur un point pour afficher l'année et les dépenses de l'année.";
            label1.Click += label1_Click;
            // 
            // Titre
            // 
            Label Titre = new Label();
            Titre.AutoSize = true;
            Titre.Font = new Font("Segoe UI", 24F);
            Titre.Location = new Point(275, 23);
            Titre.Name = "Titre";
            Titre.Size = new Size(215, 45);
            Titre.TabIndex = 2;
            Titre.Text = "Plot that lines";
            //
            //ListBox
            //
            ListBox listBox = new ListBox();
            StreamReader sr2 = new StreamReader("API_MS.MIL.XPND.CN_DS2_fr_csv_v2_3446916.csv");
            string line2;

            listBox.Location = new Point(150, 150);
            listBox.Width = 500;
            listBox.Height = 300;
            listBox.SelectedIndexChanged += new EventHandler(button_clicked);
            while ((line2 = sr2.ReadLine()) != null)
            {
                string[] words = line2.Replace("\"", "").Split(",");
                if (words[0] != "Country Name")
                {
                    listBox.Items.Add(words[0]);
                }
            }

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
                Form2 form2 = new Form2(selectedCountry);
                form2.ShowDialog();
            }
        }
    }
}
