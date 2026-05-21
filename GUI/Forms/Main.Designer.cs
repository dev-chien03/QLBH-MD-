using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Button btnQuanLySanPham;
        private Button btnBanHang;

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
            btnQuanLySanPham = new Button();
            btnBanHang = new Button();
            SuspendLayout();
            // 
            // btnQuanLySanPham
            // 
            btnQuanLySanPham.Location = new Point(50, 50);
            btnQuanLySanPham.Name = "btnQuanLySanPham";
            btnQuanLySanPham.Size = new Size(200, 50);
            btnQuanLySanPham.TabIndex = 0;
            btnQuanLySanPham.Text = "Quản Lý Sản Phẩm";
            btnQuanLySanPham.UseVisualStyleBackColor = true;
            btnQuanLySanPham.Click += btnQuanLySanPham_Click;
            // 
            // btnBanHang
            // 
            btnBanHang.Location = new Point(50, 120);
            btnBanHang.Name = "btnBanHang";
            btnBanHang.Size = new Size(200, 50);
            btnBanHang.TabIndex = 1;
            btnBanHang.Text = "Bán Hàng";
            btnBanHang.UseVisualStyleBackColor = true;
            btnBanHang.Click += btnBanHang_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1441, 702);
            Controls.Add(btnQuanLySanPham);
            Controls.Add(btnBanHang);
            Name = "Main";
            Text = "Quản Lý Bán Hàng";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion
    }
}
