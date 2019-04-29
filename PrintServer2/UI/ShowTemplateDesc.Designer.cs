namespace PrintServer2.UI
{
    partial class ShowTemplateDesc
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
            this.listTemplateDesc = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listTemplateDesc
            // 
            this.listTemplateDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTemplateDesc.FormattingEnabled = true;
            this.listTemplateDesc.ItemHeight = 12;
            this.listTemplateDesc.Location = new System.Drawing.Point(0, 0);
            this.listTemplateDesc.Name = "listTemplateDesc";
            this.listTemplateDesc.Size = new System.Drawing.Size(382, 381);
            this.listTemplateDesc.TabIndex = 2;
            // 
            // TemplateDesc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 381);
            this.Controls.Add(this.listTemplateDesc);
            this.Name = "TemplateDesc";
            this.Text = "TemplateDesc";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listTemplateDesc;
    }
}