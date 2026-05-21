using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class frmKhoHang : Form
    {
        private SanPhamBUS spBUS = new SanPhamBUS();

        private TextBox txtTimKiem;
        private DataGridView dgvKhoHang;

        public frmKhoHang()
        {
            InitializeComponent();
        }

        private void frmKhoHang_Load(object sender, EventArgs e)
        {
            BuildLayout();
            TaiDanhSachKho();
        }

        private void BuildLayout()
        {
            SuspendLayout();
            Controls.Clear();

            Text = "Quản Lý Kho Hàng";
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
                Text = "QUẢN LÝ KHO HÀNG",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.Black
            };
            headerPanel.Controls.Add(title);

            // Search & Action Panel
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

            Button btnNhapHang = new Button { Text = "Nhập hàng", Left = 240, Top = 40, Width = 80, Height = 24, BackColor = Color.LightSkyBlue };
            Button btnLichSuNhap = new Button { Text = "Lịch sử nhập hàng", Left = 330, Top = 40, Width = 120, Height = 24, BackColor = Color.Plum };

            searchPanel.Controls.AddRange(new Control[] { lblSearch, txtTimKiem, btnTim, btnRefresh, btnNhapHang, btnLichSuNhap });

            // Grid Panel
            Panel gridPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(12),
                BorderStyle = BorderStyle.None
            };

            dgvKhoHang = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            dgvKhoHang.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 149, 237);
            dgvKhoHang.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvKhoHang.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            gridPanel.Controls.Add(dgvKhoHang);

            Controls.Add(gridPanel);
            Controls.Add(searchPanel);
            Controls.Add(headerPanel);

            // Events
            txtTimKiem.TextChanged += TxtTimKiem_TextChanged;
            btnTim.Click += (s, e) => TaiDanhSachKho();
            btnRefresh.Click += (s, e) => { TaiDanhSachKho(); txtTimKiem.Clear(); };
            btnNhapHang.Click += (s, e) => MessageBox.Show("Chức năng nhập hàng sẽ được cập nhật", "Thông báo");
            btnLichSuNhap.Click += (s, e) => MessageBox.Show("Chức năng lịch sử nhập hàng sẽ được cập nhật", "Thông báo");

            ResumeLayout();
        }

        private void TaiDanhSachKho()
        {
            try
            {
                List<SanPham> danhSach = spBUS.LayDS();
                dgvKhoHang.DataSource = danhSach;
                dgvKhoHang.Columns["MaSP"].HeaderText = "Mã SP";
                dgvKhoHang.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                dgvKhoHang.Columns["SoLuongTon"].HeaderText = "Số lượng tồn";
                dgvKhoHang.Columns["DonViTinh"].HeaderText = "Đơn vị";
                dgvKhoHang.Columns["GiaBan"].HeaderText = "Đơn giá";
                dgvKhoHang.Columns["MaLoai"].HeaderText = "Loại";
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
                List<SanPham> danhSach = spBUS.TimTheoTen(txtTimKiem.Text);
                if (danhSach.Count == 0)
                    danhSach = spBUS.LayDS();
                dgvKhoHang.DataSource = danhSach;
            }
            catch { }
        }
    }
}
