namespace Ps2ImageViewer
{
    partial class IIFFormat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IIFFormat));
            this.SaveButton = new System.Windows.Forms.Button();
            this.FormatsComboBox = new System.Windows.Forms.ComboBox();
            this.FormatsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            resources.ApplyResources(this.SaveButton, "SaveButton");
            this.SaveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // FormatsComboBox
            // 
            resources.ApplyResources(this.FormatsComboBox, "FormatsComboBox");
            this.FormatsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FormatsComboBox.FormattingEnabled = true;
            this.FormatsComboBox.Name = "FormatsComboBox";
            // 
            // FormatsLabel
            // 
            resources.ApplyResources(this.FormatsLabel, "FormatsLabel");
            this.FormatsLabel.Name = "FormatsLabel";
            // 
            // IIFFormat
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.FormatsComboBox);
            this.Controls.Add(this.FormatsLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "IIFFormat";
            this.Load += new System.EventHandler(this.IIFFormat_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button SaveButton;
        internal System.Windows.Forms.ComboBox FormatsComboBox;
        internal System.Windows.Forms.Label FormatsLabel;
    }
}