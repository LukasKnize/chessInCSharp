namespace chess
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            imageSource = new Button();
            openGitHub = new Button();
            loadGameButton = new Button();
            newGameButton = new Button();
            checkLabel = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Control;
            panel1.Controls.Add(checkLabel);
            panel1.Controls.Add(imageSource);
            panel1.Controls.Add(openGitHub);
            panel1.Controls.Add(loadGameButton);
            panel1.Controls.Add(newGameButton);
            panel1.Location = new Point(800, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(200, 800);
            panel1.TabIndex = 0;
            // 
            // imageSource
            // 
            imageSource.Location = new Point(20, 730);
            imageSource.Name = "imageSource";
            imageSource.Size = new Size(160, 56);
            imageSource.TabIndex = 3;
            imageSource.Text = "zdroj obrázků";
            imageSource.UseVisualStyleBackColor = true;
            imageSource.Click += imageSource_Click;
            // 
            // openGitHub
            // 
            openGitHub.Location = new Point(20, 668);
            openGitHub.Name = "openGitHub";
            openGitHub.Size = new Size(160, 56);
            openGitHub.TabIndex = 2;
            openGitHub.Text = "GitHub";
            openGitHub.UseVisualStyleBackColor = true;
            openGitHub.Click += openGitHub_Click;
            // 
            // loadGameButton
            // 
            loadGameButton.Location = new Point(20, 222);
            loadGameButton.Name = "loadGameButton";
            loadGameButton.Size = new Size(160, 56);
            loadGameButton.TabIndex = 1;
            loadGameButton.Text = "Načíst hru";
            loadGameButton.UseVisualStyleBackColor = true;
            loadGameButton.Click += loadGameButton_Click;
            // 
            // newGameButton
            // 
            newGameButton.Location = new Point(20, 160);
            newGameButton.Name = "newGameButton";
            newGameButton.Size = new Size(160, 56);
            newGameButton.TabIndex = 0;
            newGameButton.Text = "Nová hra";
            newGameButton.UseVisualStyleBackColor = true;
            newGameButton.Click += newGameButton_Click;
            // 
            // checkLabel
            // 
            checkLabel.AutoSize = true;
            checkLabel.Location = new Point(20, 77);
            checkLabel.Name = "checkLabel";
            checkLabel.Size = new Size(0, 20);
            checkLabel.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 801);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Form1";
            Load += DrawGame;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button newGameButton;
        private Button loadGameButton;
        private Button imageSource;
        private Button openGitHub;
        private Label checkLabel;
    }
}
