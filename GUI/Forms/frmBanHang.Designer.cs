namespace GUI.Forms
{
    partial class frmBanHang
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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 750);
            this.Text = "Bán Hàng";
            
            System.Windows.Forms.Panel pnlHeader = new System.Windows.Forms.Panel();
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Height = 50;
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(200, 240, 180);
            
            System.Windows.Forms.Label lblTitle = new System.Windows.Forms.Label();
            lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            lblTitle.Text = "BÁN HÀNG";
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblTitle.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
            pnlHeader.Controls.Add(lblTitle);
            
            this.Controls.Add(pnlHeader);
        }

        #endregion
    }
}