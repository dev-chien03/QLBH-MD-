using System;
using System.Windows.Forms;
using System.Drawing;

namespace GUI.Forms
{
    partial class frmSanPham
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnlHeader;
        private Label lblTitle;
        private Panel pnlSearch;
        private TextBox txtSearchBox;
        private Button btnFind;
        private Button btnRefresh;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Panel pnlGrid;
        private DataGridView dgvProducts;
        private Panel pnlInfo;
        private Label lblInfoTitle;
        private Label lblMaSP;
        private Label lblTenSP;
        private Label lblLoai;
        private Label lblDonGia;
        private Label lblDonViTinh;
        private Label lblSoLuong;
        private TextBox txtMaSPInfo;
        private TextBox txtTenSPInfo;
        private ComboBox cboLoaiInfo;
        private TextBox txtGiaInfo;
        private TextBox txtDVTInfo;
        private TextBox txtSoLuongInfo;
        private Button btnSave;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Header Panel
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();

            // Search Panel
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.txtSearchBox = new System.Windows.Forms.TextBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();

            // Grid Panel
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dgvProducts = new System.Windows.Forms.DataGridView();

            // Info Panel
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblInfoTitle = new System.Windows.Forms.Label();
            this.lblMaSP = new System.Windows.Forms.Label();
            this.lblTenSP = new System.Windows.Forms.Label();
            this.lblLoai = new System.Windows.Forms.Label();
            this.lblDonGia = new System.Windows.Forms.Label();
            this.lblDonViTinh = new System.Windows.Forms.Label();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.txtMaSPInfo = new System.Windows.Forms.TextBox();
            this.txtTenSPInfo = new System.Windows.Forms.TextBox();
            this.cboLoaiInfo = new System.Windows.Forms.ComboBox();
            this.txtGiaInfo = new System.Windows.Forms.TextBox();
            this.txtDVTInfo = new System.Windows.Forms.TextBox();
            this.txtSoLuongInfo = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(200, 240, 180);
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 50;
            this.pnlHeader.Name = "pnlHeader";

            // lblTitle
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Text = "QUẢN LÝ SẢN PHẨM";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // pnlSearch
            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSearch.Controls.Add(this.btnDelete);
            this.pnlSearch.Controls.Add(this.btnEdit);
            this.pnlSearch.Controls.Add(this.btnAdd);
            this.pnlSearch.Controls.Add(this.btnRefresh);
            this.pnlSearch.Controls.Add(this.btnFind);
            this.pnlSearch.Controls.Add(this.txtSearchBox);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Height = 70;
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(12);
            this.pnlSearch.Name = "pnlSearch";

            this.txtSearchBox.Location = new System.Drawing.Point(70, 10);
            this.txtSearchBox.Size = new System.Drawing.Size(160, 24);

            this.btnFind.Location = new System.Drawing.Point(240, 10);
            this.btnFind.Size = new System.Drawing.Size(60, 24);
            this.btnFind.Text = "Tìm";
            this.btnFind.BackColor = System.Drawing.Color.LightBlue;

            this.btnRefresh.Location = new System.Drawing.Point(310, 10);
            this.btnRefresh.Size = new System.Drawing.Size(60, 24);
            this.btnRefresh.Text = "Tải lại";

            this.btnAdd.Location = new System.Drawing.Point(240, 40);
            this.btnAdd.Size = new System.Drawing.Size(60, 24);
            this.btnAdd.Text = "Thêm";
            this.btnAdd.BackColor = System.Drawing.Color.LightCoral;

            this.btnEdit.Location = new System.Drawing.Point(310, 40);
            this.btnEdit.Size = new System.Drawing.Size(60, 24);
            this.btnEdit.Text = "Sửa";
            this.btnEdit.BackColor = System.Drawing.Color.LightYellow;

            this.btnDelete.Location = new System.Drawing.Point(380, 40);
            this.btnDelete.Size = new System.Drawing.Size(60, 24);
            this.btnDelete.Text = "Xóa";
            this.btnDelete.BackColor = System.Drawing.Color.LightPink;

            // pnlGrid
            this.pnlGrid.BackColor = System.Drawing.Color.White;
            this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pnlGrid.Controls.Add(this.pnlInfo);
            this.pnlGrid.Controls.Add(this.dgvProducts);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(12);
            this.pnlGrid.Name = "pnlGrid";

            // dgvProducts
            this.dgvProducts.BackgroundColor = System.Drawing.Color.White;
            this.dgvProducts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvProducts.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvProducts.Height = 250;
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.ReadOnly = true;

            // pnlInfo
            this.pnlInfo.BackColor = System.Drawing.Color.White;
            this.pnlInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInfo.Padding = new System.Windows.Forms.Padding(12);
            this.pnlInfo.Name = "pnlInfo";

            this.lblInfoTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblInfoTitle.Location = new System.Drawing.Point(10, 10);
            this.lblInfoTitle.Text = "Thông tin sản phẩm";
            this.lblInfoTitle.Width = 200;

            this.lblMaSP.Location = new System.Drawing.Point(10, 40);
            this.lblMaSP.Text = "Mã SP";
            this.lblMaSP.Width = 80;

            this.txtMaSPInfo.Location = new System.Drawing.Point(100, 38);
            this.txtMaSPInfo.Size = new System.Drawing.Size(160, 22);

            this.lblTenSP.Location = new System.Drawing.Point(10, 70);
            this.lblTenSP.Text = "Tên Sản Phẩm";
            this.lblTenSP.Width = 80;

            this.txtTenSPInfo.Location = new System.Drawing.Point(100, 68);
            this.txtTenSPInfo.Size = new System.Drawing.Size(160, 22);

            this.lblLoai.Location = new System.Drawing.Point(10, 100);
            this.lblLoai.Text = "Loại";
            this.lblLoai.Width = 80;

            this.cboLoaiInfo.Location = new System.Drawing.Point(100, 98);
            this.cboLoaiInfo.Size = new System.Drawing.Size(160, 22);

            this.lblDonGia.Location = new System.Drawing.Point(300, 40);
            this.lblDonGia.Text = "Đơn giá";
            this.lblDonGia.Width = 80;

            this.txtGiaInfo.Location = new System.Drawing.Point(390, 38);
            this.txtGiaInfo.Size = new System.Drawing.Size(160, 22);

            this.lblDonViTinh.Location = new System.Drawing.Point(300, 70);
            this.lblDonViTinh.Text = "Đơn vị tính";
            this.lblDonViTinh.Width = 80;

            this.txtDVTInfo.Location = new System.Drawing.Point(390, 68);
            this.txtDVTInfo.Size = new System.Drawing.Size(160, 22);

            this.lblSoLuong.Location = new System.Drawing.Point(300, 100);
            this.lblSoLuong.Text = "Số lượng tồn";
            this.lblSoLuong.Width = 80;

            this.txtSoLuongInfo.Location = new System.Drawing.Point(390, 98);
            this.txtSoLuongInfo.Size = new System.Drawing.Size(160, 22);

            this.btnSave.Location = new System.Drawing.Point(600, 60);
            this.btnSave.Size = new System.Drawing.Size(80, 40);
            this.btnSave.Text = "Lưu";
            this.btnSave.BackColor = System.Drawing.Color.PeachPuff;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F);

            this.pnlInfo.Controls.Add(this.btnSave);
            this.pnlInfo.Controls.Add(this.lblSoLuong);
            this.pnlInfo.Controls.Add(this.txtSoLuongInfo);
            this.pnlInfo.Controls.Add(this.lblDonViTinh);
            this.pnlInfo.Controls.Add(this.txtDVTInfo);
            this.pnlInfo.Controls.Add(this.lblDonGia);
            this.pnlInfo.Controls.Add(this.txtGiaInfo);
            this.pnlInfo.Controls.Add(this.lblLoai);
            this.pnlInfo.Controls.Add(this.cboLoaiInfo);
            this.pnlInfo.Controls.Add(this.lblTenSP);
            this.pnlInfo.Controls.Add(this.txtTenSPInfo);
            this.pnlInfo.Controls.Add(this.lblMaSP);
            this.pnlInfo.Controls.Add(this.txtMaSPInfo);
            this.pnlInfo.Controls.Add(this.lblInfoTitle);

            // frmSanPham
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 700);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlHeader);
            this.Name = "frmSanPham";
            this.Text = "Quản Lý Sản Phẩm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmSanPham_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
