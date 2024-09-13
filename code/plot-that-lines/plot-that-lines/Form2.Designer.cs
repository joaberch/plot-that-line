using ScottPlot.WinForms;

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
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = headerTitle;

            //ScottPlott
            SuspendLayout();
            FormsPlot formsPlot = new FormsPlot
            {
                DisplayScale = 10000,
                Location = new Point(100, 50),
                Name = headerTitle,
                Size = new Size(600, 300),
                TabIndex = 0,
            };
            ResumeLayout(false);

            //title
            Label title = new Label();
            title.AutoSize = true;
            title.Font = new Font("Segoe UI", 24F);
            title.Location = new Point(275, 5);
            title.Name = "Titre";
            title.Size = new Size(215, 45);
            title.TabIndex = 2;
            title.Text = headerTitle;

            //add to forms
            Controls.Add(formsPlot);
            Controls.Add(title);
        }

        #endregion
    }
}