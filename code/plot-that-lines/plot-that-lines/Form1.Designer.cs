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
            // Scrollbar
            //
            //VScrollBar vScroller = new VScrollBar();
            //vScroller.Dock = DockStyle.Right;
            //vScroller.Width = 30;
            //vScroller.Height = 200;
            //vScroller.Name = "VScrollBar1";
            //
            // Buttons
            //
            //TODO : use myButton.Width instead of buttonWidth
            StreamReader sr = new StreamReader("API_MS.MIL.XPND.CN_DS2_fr_csv_v2_3446916.csv");
            string line;
            int xLocation = 60;
            int yLocation = 150;

            List<Button> buttons = new List<Button>();

            while ((line = sr.ReadLine()) != null)
            {
                string[] words = line.Replace("\"", "").Split(",");

                if (words[0] != "Country Name" && words[0] != "")
                {
                    double buttonWidth = 30 + words[0].Length * 5;

                    Button myButton = new Button
                    {
                        Location = new Point(xLocation, yLocation),
                        Text = words[0],
                        AutoSize = false,
                    };
                    myButton.Click += new EventHandler(button_clicked);

                    //Debug.Write(words[0]);
                    //Debug.WriteLine("   |   calc : " + buttonWidth + "    |   width : " + myButton.Width + "  |   locationX : " + myButton.Location.X);

                    if (buttonWidth > 90 && buttonWidth < 159) {
                        //2 case
                        xLocation += 200;
                        myButton.Width = 180;
                    } else if (buttonWidth >= 160)
                    {
                        //3 case
                        xLocation += 300;
                        myButton.Width = 280;
                    } else {
                        //1 case
                        xLocation += 100;
                        myButton.Width = 80;
                    }

                    if (buttonWidth > 90 && myButton.Location.X > 650)
                    {
                        xLocation = 60;
                        yLocation += 30;
                        myButton.Location = new Point(xLocation, yLocation);
                    }

                    if (xLocation > 700) {
                        xLocation = 60;
                        yLocation += 30;
                    }

                    buttons.Add(myButton);
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
            Controls.Add(label1);
            //Controls.Add(vScroller);
            //Add every buttons
            buttons.ForEach(button => { Controls.Add(button); });

            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        private void button_clicked(object sender, EventArgs e)
        {
            string[] countryName = sender.ToString().Split("Text: ");
            Form2 form2 = new Form2(countryName[1]);
            form2.ShowDialog();
        }

        //private Label label1;
        //private Label Titre;
    }
}
