namespace Ps2ImageViewer
{
    partial class RawProperties
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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RawProperties));
            this.Rgba32RadioButton = new System.Windows.Forms.RadioButton();
            this.Rgb24RadioButton = new System.Windows.Forms.RadioButton();
            this.HeightTextBox = new System.Windows.Forms.TextBox();
            this.HeightLabel = new System.Windows.Forms.Label();
            this.OkButton = new System.Windows.Forms.Button();
            this.WidthTextBox = new System.Windows.Forms.TextBox();
            this.WidthLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Rgba32RadioButton
            // 
            resources.ApplyResources(this.Rgba32RadioButton, "Rgba32RadioButton");
            this.Rgba32RadioButton.Name = "Rgba32RadioButton";
            this.Rgba32RadioButton.UseVisualStyleBackColor = true;
            // 
            // Rgb24RadioButton
            // 
            resources.ApplyResources(this.Rgb24RadioButton, "Rgb24RadioButton");
            this.Rgb24RadioButton.Checked = true;
            this.Rgb24RadioButton.Name = "Rgb24RadioButton";
            this.Rgb24RadioButton.TabStop = true;
            this.Rgb24RadioButton.UseVisualStyleBackColor = true;
            // 
            // HeightTextBox
            // 
            resources.ApplyResources(this.HeightTextBox, "HeightTextBox");
            this.HeightTextBox.Name = "HeightTextBox";
            // 
            // HeightLabel
            // 
            resources.ApplyResources(this.HeightLabel, "HeightLabel");
            this.HeightLabel.Name = "HeightLabel";
            // 
            // OkButton
            // 
            resources.ApplyResources(this.OkButton, "OkButton");
            this.OkButton.Name = "OkButton";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // WidthTextBox
            // 
            resources.ApplyResources(this.WidthTextBox, "WidthTextBox");
            this.WidthTextBox.Name = "WidthTextBox";
            this.WidthTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WidthTextBox_KeyPress);
            // 
            // WidthLabel
            // 
            resources.ApplyResources(this.WidthLabel, "WidthLabel");
            this.WidthLabel.Name = "WidthLabel";
            // 
            // RawProperties
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Rgba32RadioButton);
            this.Controls.Add(this.Rgb24RadioButton);
            this.Controls.Add(this.HeightTextBox);
            this.Controls.Add(this.HeightLabel);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.WidthTextBox);
            this.Controls.Add(this.WidthLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RawProperties";
            this.Opacity = 0.86D;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.RadioButton Rgba32RadioButton;
        internal System.Windows.Forms.RadioButton Rgb24RadioButton;
        internal System.Windows.Forms.TextBox HeightTextBox;
        internal System.Windows.Forms.Label HeightLabel;
        internal System.Windows.Forms.Button OkButton;
        internal System.Windows.Forms.TextBox WidthTextBox;
        internal System.Windows.Forms.Label WidthLabel;
    }
}