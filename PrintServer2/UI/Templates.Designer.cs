namespace PrintServer2.UI
{
    partial class Templates
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
            this.listTemplates = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listTemplates
            // 
            this.listTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTemplates.FormattingEnabled = true;
            this.listTemplates.ItemHeight = 12;
            this.listTemplates.Location = new System.Drawing.Point(0, 0);
            this.listTemplates.Name = "listTemplates";
            this.listTemplates.Size = new System.Drawing.Size(800, 450);
            this.listTemplates.TabIndex = 1;
            // 
            // Templates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listTemplates);
            this.Name = "Templates";
            this.Text = "Templates";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listTemplates;
    }
}