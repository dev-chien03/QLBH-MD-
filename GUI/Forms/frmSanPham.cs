using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class frmSanPham : Form
    {
        private SanPhamBUS spBUS = new SanPhamBUS();
        private LoaiSPBUS loaiBUS = new LoaiSPBUS();

        private TextBox txtTimKiem;
        private DataGridView dgvSanPham;
        private TextBox txtMaSP, txtTenSP, txtGiaBan, txtSoLuong, txtDVT;
        private ComboBox cboLoaiSP;
        private Button btnThem, btnSua, btnXoa, btnLuu;

        public frmSanPham()
        {
            InitializeComponent();
        }

        private void frmSanPham_Load(object sender, EventArgs e)
        {
            BuildLayout();
            TaiDanhMucLoaiSP();
            TaiDanhSachSanPham();
        }

        private void BuildLayout()
        {
            SuspendLayout();
            Controls.Clear();

            Text = "Quản Lý Sản Phẩm";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(900, 700);
            BackColor = Color.White;

            // Header
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(200, 240, 180),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label title = new Label
            {
                Dock = DockStyle.Fill,
                Text = "QUẢN LÝ SẢN PHẨM",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.Black
            };
            headerPanel.Controls.Add(title);

            // Search Panel
            Panel searchPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(12)
            };

            Label lblSearch = new Label { Text = "Tìm kiếm", Left = 10, Top = 10, Width = 60 };
            txtTimKiem = new TextBox { Left = 70, Top = 8, Width = 160, Height = 24 };
            Button btnTim = new Button { Text = "Tìm", Left = 240, Top = 8, Width = 60, Height = 24, BackColor = Color.LightBlue };
            Button btnRefresh = new Button { Text = "Tải lại", Left = 310, Top = 8, Width = 60, Height = 24, BackColor = Color.LightGray };

            btnThem = new Button { Text = "Thêm", Left = 240, Top = 40, Width = 60, Height = 24, BackColor = Color.LightCoral };
            btnSua = new Button { Text = "Sửa", Left = 310, Top = 40, Width = 60, Height = 24, BackColor = Color.LightYellow };
            btnXoa = new Button { Text = "Xóa", Left = 380, Top = 40, Width = 60, Height = 24, BackColor = Color.LightPink };

            searchPanel.Controls.AddRange(new Control[] { lblSearch, txtTimKiem, btnTim, btnRefresh, btnThem, btnSua, btnXoa });

            // Grid Panel
            Panel gridPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(12),
                BorderStyle = BorderStyle.None
            };

            dgvSanPham = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 250,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            dgvSanPham.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 149, 237);
            dgvSanPham.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSanPham.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // Info Panel
            Panel infoPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(12)
            };

            Label lblInfo = new Label { Text = "Thông tin sản phẩm", Left = 10, Top = 10, Width = 200, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };

            Label lbl1 = new Label { Text = "Mã SP", Left = 10, Top = 40, Width = 80 };
            txtMaSP = new TextBox { Left = 100, Top = 38, Width = 160, Height = 22 };

            Label lbl2 = new Label { Text = "Tên Sản Phẩm", Left = 10, Top = 70, Width = 80 };
            txtTenSP = new TextBox { Left = 100, Top = 68, Width = 160, Height = 22 };

            Label lbl3 = new Label { Text = "Loại", Left = 10, Top = 100, Width = 80 };
            cboLoaiSP = new ComboBox { Left = 100, Top = 98, Width = 160, Height = 22, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lbl4 = new Label { Text = "Đơn giá", Left = 300, Top = 40, Width = 80 };
            txtGiaBan = new TextBox { Left = 390, Top = 38, Width = 160, Height = 22 };

            Label lbl5 = new Label { Text = "Đơn vị tính", Left = 300, Top = 70, Width = 80 };
            txtDVT = new TextBox { Left = 390, Top = 68, Width = 160, Height = 22 };

            Label lbl6 = new Label { Text = "Số lượng tồn", Left = 300, Top = 100, Width = 80 };
            txtSoLuong = new TextBox { Left = 390, Top = 98, Width = 160, Height = 22 };

            btnLuu = new Button { Text = "Lưu", Left = 600, Top = 60, Width = 80, Height = 40, BackColor = Color.PeachPuff, Font = new Font("Segoe UI", 10F) };

            infoPanel.Controls.AddRange(new Control[] { lblInfo, lbl1, txtMaSP, lbl2, txtTenSP, lbl3, cboLoaiSP, lbl4, txtGiaBan, lbl5, txtDVT, lbl6, txtSoLuong, btnLuu });

            gridPanel.Controls.Add(infoPanel);
            gridPanel.Controls.Add(dgvSanPham);

            Controls.Add(gridPanel);
            Controls.Add(searchPanel);
            Controls.Add(headerPanel);

            // Events
            txtTimKiem.TextChanged += TxtTimKiem_TextChanged;
            btnTim.Click += (s, e) => TaiDanhSachSanPham();
            btnRefresh.Click += (s, e) => { TaiDanhSachSanPham(); LamMoiVungNhap(); };
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            dgvSanPham.CellClick += DgvSanPham_CellClick;

            ResumeLayout(true);
        }

        private void TaiDanhMucLoaiSP()
        {
            cboLoaiSP.DataSource = loaiBUS.LayDS();
            cboLoaiSP.DisplayMember = "TenLoai";
            cboLoaiSP.ValueMember = "MaLoai";
        }

        private void TaiDanhSachSanPham()
        {
            try
            {
                var data = spBUS.LayDS();
                dgvSanPham.DataSource = data;

                if (dgvSanPham.Columns["MaSP"] != null) dgvSanPham.Columns["MaSP"].HeaderText = "Mã SP";
                if (dgvSanPham.Columns["TenSP"] != null) dgvSanPham.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                if (dgvSanPham.Columns["GiaBan"] != null) dgvSanPham.Columns["GiaBan"].HeaderText = "Đơn giá";
                if (dgvSanPham.Columns["SoLuongTon"] != null) dgvSanPham.Columns["SoLuongTon"].HeaderText = "Số lượng tồn";
                if (dgvSanPham.Columns["DonViTinh"] != null) dgvSanPham.Columns["DonViTinh"].HeaderText = "Đơn vị tính";
                if (dgvSanPham.Columns["MaLoai"] != null) dgvSanPham.Columns["MaLoai"].HeaderText = "Loại";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim().ToLower();
                var data = spBUS.LayDS();
                var filtered = data.FindAll(x => x.TenSP.ToLower().Contains(keyword) || x.MaSP.ToLower().Contains(keyword));
                dgvSanPham.DataSource = filtered;
            }
            catch { }
        }

        private void DgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvSanPham.Rows[e.RowIndex];
            txtMaSP.Text = row.Cells["MaSP"].Value?.ToString();
            txtTenSP.Text = row.Cells["TenSP"].Value?.ToString();
            txtGiaBan.Text = row.Cells["GiaBan"].Value?.ToString();
            txtSoLuong.Text = row.Cells["SoLuongTon"].Value?.ToString();
            txtDVT.Text = row.Cells["DonViTinh"].Value?.ToString();
            if (row.Cells["MaLoai"].Value != null) cboLoaiSP.SelectedValue = row.Cells["MaLoai"].Value;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text) || string.IsNullOrWhiteSpace(txtTenSP.Text))
            {
                MessageBox.Show("Vui lòng nhập mã và tên sản phẩm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sp = new SanPham
            {
                MaSP = txtMaSP.Text.Trim(),
                TenSP = txtTenSP.Text.Trim(),
                GiaBan = decimal.TryParse(txtGiaBan.Text, out decimal g) ? g : 0,
                SoLuongTon = int.TryParse(txtSoLuong.Text, out int s) ? s : 0,
                DonViTinh = txtDVT.Text.Trim(),
                MaLoai = cboLoaiSP.SelectedValue?.ToString() ?? ""
            };

            if (spBUS.LuuSanPham(sp))
            {
                MessageBox.Show("Thêm sản phẩm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachSanPham();
                LamMoiVungNhap();
            }
            else
                MessageBox.Show("Thêm thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sp = new SanPham
            {
                MaSP = txtMaSP.Text.Trim(),
                TenSP = txtTenSP.Text.Trim(),
                GiaBan = decimal.TryParse(txtGiaBan.Text, out decimal g) ? g : 0,
                SoLuongTon = int.TryParse(txtSoLuong.Text, out int s) ? s : 0,
                DonViTinh = txtDVT.Text.Trim(),
                MaLoai = cboLoaiSP.SelectedValue?.ToString() ?? ""
            };

            if (spBUS.SuaSanPham(sp))
            {
                MessageBox.Show("Cập nhật sản phẩm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachSanPham();
                LamMoiVungNhap();
            }
            else
                MessageBox.Show("Cập nhật thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xóa sản phẩm {txtMaSP.Text}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (spBUS.XoaSanPham(txtMaSP.Text.Trim()))
            {
                MessageBox.Show("Xóa sản phẩm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachSanPham();
                LamMoiVungNhap();
            }
            else
                MessageBox.Show("Xóa thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                BtnThem_Click(null, null);
            else
                BtnSua_Click(null, null);
        }

        private void LamMoiVungNhap()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtGiaBan.Clear();
            txtSoLuong.Clear();
            txtDVT.Clear();
            if (cboLoaiSP.Items.Count > 0) cboLoaiSP.SelectedIndex = 0;
            txtTimKiem.Clear();
        }
    }
}
