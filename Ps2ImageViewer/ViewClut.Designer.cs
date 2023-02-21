namespace Ps2ImageViewer
{
    partial class ViewClut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewClut));
            this.clut = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.clut)).BeginInit();
            this.SuspendLayout();
            // 
            // clut
            // 
            resources.ApplyResources(this.clut, "clut");
            this.clut.BackColor = System.Drawing.Color.White;
            this.clut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clut.Name = "clut";
            this.clut.TabStop = false;
            // 
            // ViewClut
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.clut);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ViewClut";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewClut_FormClosing);
            this.Load += new System.EventHandler(this.ViewClut_Load);
            ((System.ComponentModel.ISupportInitialize)(this.clut)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.PictureBox clut;
    }
}