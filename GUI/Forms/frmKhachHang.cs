using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class frmKhachHang : Form
    {
        private KhachHangBUS khBUS = new KhachHangBUS();

        private TextBox txtTimKiem;
        private DataGridView dgvKhachHang;
        private TextBox txtMaKH, txtTenKH, txtSDT, txtDiaChi, txtNgayThamGia;
        private Button btnThem, btnSua, btnXoa, btnLuu;

        public frmKhachHang()
        {
            InitializeComponent();
        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {
            BuildLayout();
            TaiDanhSachKhachHang();
        }

        private void BuildLayout()
        {
            SuspendLayout();
            Controls.Clear();

            Text = "Quản Lý Khách Hàng";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1000, 700);
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
                Text = "QUẢN LÝ KHÁCH HÀNG",
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

            // Main Content - 2 columns
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // Left Panel - Grid
            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 500,
                BackColor = Color.White,
                Padding = new Padding(12),
                BorderStyle = BorderStyle.None
            };

            Label lblGridTitle = new Label { Text = "Danh sách khách hàng", Left = 10, Top = 5, Width = 200, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };

            dgvKhachHang = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Top = 35
            };
            dgvKhachHang.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 149, 237);
            dgvKhachHang.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvKhachHang.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            leftPanel.Controls.Add(dgvKhachHang);
            leftPanel.Controls.Add(lblGridTitle);

            // Right Panel - Info
            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(12)
            };

            Label lblInfo = new Label { Text = "Thông tin khách hàng", Left = 10, Top = 10, Width = 200, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };

            Label lbl1 = new Label { Text = "Mã KH", Left = 10, Top = 40, Width = 80 };
            txtMaKH = new TextBox { Left = 100, Top = 38, Width = 160, Height = 22, ReadOnly = true };

            Label lbl2 = new Label { Text = "Họ Tên", Left = 10, Top = 70, Width = 80 };
            txtTenKH = new TextBox { Left = 100, Top = 68, Width = 160, Height = 22 };

            Label lbl3 = new Label { Text = "SDT", Left = 10, Top = 100, Width = 80 };
            txtSDT = new TextBox { Left = 100, Top = 98, Width = 160, Height = 22 };

            Label lbl4 = new Label { Text = "Địa chỉ", Left = 10, Top = 130, Width = 80 };
            txtDiaChi = new TextBox { Left = 100, Top = 128, Width = 160, Height = 50, Multiline = true };

            Label lbl5 = new Label { Text = "Ngày tham gia", Left = 10, Top = 190, Width = 80 };
            txtNgayThamGia = new TextBox { Left = 100, Top = 188, Width = 160, Height = 22 };

            Button btnLuu = new Button { Text = "Lưu", Left = 100, Top = 230, Width = 80, Height = 40, BackColor = Color.PeachPuff, Font = new Font("Segoe UI", 10F) };

            rightPanel.Controls.AddRange(new Control[] { lblInfo, lbl1, txtMaKH, lbl2, txtTenKH, lbl3, txtSDT, lbl4, txtDiaChi, lbl5, txtNgayThamGia, btnLuu });

            mainPanel.Controls.Add(rightPanel);
            mainPanel.Controls.Add(leftPanel);

            Controls.Add(mainPanel);
            Controls.Add(searchPanel);
            Controls.Add(headerPanel);

            // Events
            txtTimKiem.TextChanged += TxtTimKiem_TextChanged;
            btnTim.Click += (s, e) => TaiDanhSachKhachHang();
            btnRefresh.Click += (s, e) => { TaiDanhSachKhachHang(); LamMoiVungNhap(); };
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            dgvKhachHang.CellClick += DgvKhachHang_CellClick;

            ResumeLayout();
        }

        private void TaiDanhSachKhachHang()
        {
            try
            {
                List<KhachHang> danhSach = khBUS.LayDS();
                dgvKhachHang.DataSource = danhSach;
                dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
                dgvKhachHang.Columns["TenKH"].HeaderText = "Họ Tên";
                dgvKhachHang.Columns["SoDienThoai"].HeaderText = "SDT";
                dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa chỉ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Thông báo");
            }
        }

        private void TxtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                List<KhachHang> danhSach = khBUS.TimTheoSDT(txtTimKiem.Text);
                if (danhSach.Count == 0)
                    danhSach = khBUS.LayDS();
                dgvKhachHang.DataSource = danhSach;
            }
            catch { }
        }

        private void DgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMaKH.Text = dgvKhachHang.Rows[e.RowIndex].Cells["MaKH"].Value?.ToString() ?? "";
                txtTenKH.Text = dgvKhachHang.Rows[e.RowIndex].Cells["TenKH"].Value?.ToString() ?? "";
                txtSDT.Text = dgvKhachHang.Rows[e.RowIndex].Cells["SoDienThoai"].Value?.ToString() ?? "";
                txtDiaChi.Text = dgvKhachHang.Rows[e.RowIndex].Cells["DiaChi"].Value?.ToString() ?? "";
            }
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKH.Text) || string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã KH và Họ Tên", "Thông báo");
                return;
            }
            KhachHang kh = new KhachHang
            {
                MaKH = txtMaKH.Text,
                TenKH = txtTenKH.Text,
                SoDienThoai = txtSDT.Text,
                DiaChi = txtDiaChi.Text
            };
            try
            {
                khBUS.LuuKhachHang(kh);
                MessageBox.Show("Thêm khách hàng thành công", "Thông báo");
                TaiDanhSachKhachHang();
                LamMoiVungNhap();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo");
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKH.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa", "Thông báo");
                return;
            }
            KhachHang kh = new KhachHang
            {
                MaKH = txtMaKH.Text,
                TenKH = txtTenKH.Text,
                SoDienThoai = txtSDT.Text,
                DiaChi = txtDiaChi.Text
            };
            try
            {
                khBUS.SuaKhachHang(kh);
                MessageBox.Show("Cập nhật khách hàng thành công", "Thông báo");
                TaiDanhSachKhachHang();
                LamMoiVungNhap();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo");
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKH.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa", "Thông báo");
                return;
            }
            if (MessageBox.Show("Xác nhận xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    khBUS.XoaKhachHang(txtMaKH.Text);
                    MessageBox.Show("Xóa khách hàng thành công", "Thông báo");
                    TaiDanhSachKhachHang();
                    LamMoiVungNhap();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo");
                }
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKH.Text))
                BtnThem_Click(sender, e);
            else
                BtnSua_Click(sender, e);
        }

        private void LamMoiVungNhap()
        {
            txtMaKH.Clear();
            txtTenKH.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            txtNgayThamGia.Clear();
            txtTimKiem.Clear();
        }
    }
}
