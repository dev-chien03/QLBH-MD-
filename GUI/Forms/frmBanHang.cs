using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace GUI.Forms
{
    public partial class frmBanHang : Form
    {
        private SanPhamBUS spBUS = new SanPhamBUS();
        private KhachHangBUS khBUS = new KhachHangBUS();
        private HoaDonBUS hdBUS = new HoaDonBUS();

        private TextBox txtMaHD, txtNgayBan, txtTenKH, txtTimSP, txtDonGia, txtSoLuongTon, txtSoLuong;
        private DataGridView dgvChiTietHD;
        private Label lblTongTien;
        private List<SanPham> dsChiTiet = new List<SanPham>();

        public frmBanHang()
        {
            InitializeComponent();
        }

        private void frmBanHang_Load(object sender, EventArgs e)
        {
            BuildLayout();
        }

        private void BuildLayout()
        {
            SuspendLayout();
            Controls.Clear();

            Text = "Bán Hàng";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1100, 750);
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
                Text = "BÁN HÀNG",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.Black
            };
            headerPanel.Controls.Add(title);

            // Main Content
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(12)
            };

            // Top Section - Invoice Info & Search
            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Left: Thông tin hóa đơn
            Label lbl1 = new Label { Text = "Mã hóa đơn", Left = 10, Top = 10, Width = 80 };
            txtMaHD = new TextBox { Left = 100, Top = 8, Width = 120, Height = 22 };

            Label lbl2 = new Label { Text = "Ngày bán", Left = 10, Top = 40, Width = 80 };
            txtNgayBan = new TextBox { Left = 100, Top = 38, Width = 120, Height = 22 };

            Label lbl3 = new Label { Text = "Khách hàng", Left = 250, Top = 10, Width = 80 };
            txtTenKH = new TextBox { Left = 340, Top = 8, Width = 150, Height = 22 };

            // Right: Search products
            Label lbl4 = new Label { Text = "Tìm sản phẩm", Left = 700, Top = 10, Width = 80 };
            txtTimSP = new TextBox { Left = 790, Top = 8, Width = 150, Height = 22 };
            Button btnTimSP = new Button { Text = "Tìm", Left = 950, Top = 8, Width = 60, Height = 24, BackColor = Color.LightBlue };

            topPanel.Controls.AddRange(new Control[] { lbl1, txtMaHD, lbl2, txtNgayBan, lbl3, txtTenKH, lbl4, txtTimSP, btnTimSP });

            // Middle Section - Grid of Items
            Panel middlePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 280,
                BackColor = Color.White,
                Padding = new Padding(0, 12, 0, 0),
                BorderStyle = BorderStyle.None
            };

            Label lblGridTitle = new Label { Text = "Danh sách sản phẩm trong hóa đơn", Left = 12, Top = 0, Width = 350, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };

            dgvChiTietHD = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Top = 30
            };
            dgvChiTietHD.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 149, 237);
            dgvChiTietHD.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvChiTietHD.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            dgvChiTietHD.Columns.Add("STT", "STT");
            dgvChiTietHD.Columns.Add("MaSP", "Mã SP");
            dgvChiTietHD.Columns.Add("TenSP", "Tên Sản Phẩm");
            dgvChiTietHD.Columns.Add("DonGia", "Đơn giá");
            dgvChiTietHD.Columns.Add("SoLuong", "Số lượng");
            dgvChiTietHD.Columns.Add("ThanhTien", "Thành tiền");

            middlePanel.Controls.Add(dgvChiTietHD);
            middlePanel.Controls.Add(lblGridTitle);

            // Bottom Section - Product Details & Checkout
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // Left: Product Details
            Panel leftBottomPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 400,
                BackColor = Color.White,
                Padding = new Padding(12),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblProductInfo = new Label { Text = "Thông tin sản phẩm", Left = 10, Top = 10, Width = 200, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };

            Label lblDonGia = new Label { Text = "Đơn giá", Left = 10, Top = 40, Width = 60 };
            txtDonGia = new TextBox { Left = 100, Top = 38, Width = 120, Height = 22, ReadOnly = true };

            Label lblSoLuongTon = new Label { Text = "Số lượng tồn", Left = 10, Top = 70, Width = 80 };
            txtSoLuongTon = new TextBox { Left = 100, Top = 68, Width = 120, Height = 22, ReadOnly = true };

            Label lblSoLuong = new Label { Text = "Số lượng", Left = 10, Top = 100, Width = 80 };
            txtSoLuong = new TextBox { Left = 100, Top = 98, Width = 120, Height = 22 };

            Button btnThemVaoHD = new Button { Text = "Thêm vào hóa đơn", Left = 100, Top = 140, Width = 150, Height = 35, BackColor = Color.LightBlue, Font = new Font("Segoe UI", 10F) };

            leftBottomPanel.Controls.AddRange(new Control[] { lblProductInfo, lblDonGia, txtDonGia, lblSoLuongTon, txtSoLuongTon, lblSoLuong, txtSoLuong, btnThemVaoHD });

            // Right: Checkout
            Panel rightBottomPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(12),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblTotal = new Label { Text = "Tổng thanh toán", Left = 10, Top = 80, Width = 200, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            lblTongTien = new Label { Text = "0 đ", Left = 200, Top = 80, Width = 150, Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.Red, TextAlign = ContentAlignment.MiddleRight };

            Button btnThanhToan = new Button { Text = "Thanh toán", Left = 150, Top = 130, Width = 120, Height = 40, BackColor = Color.LightGreen, Font = new Font("Segoe UI", 10F) };

            rightBottomPanel.Controls.AddRange(new Control[] { lblTotal, lblTongTien, btnThanhToan });

            bottomPanel.Controls.Add(rightBottomPanel);
            bottomPanel.Controls.Add(leftBottomPanel);

            mainPanel.Controls.Add(bottomPanel);
            mainPanel.Controls.Add(middlePanel);
            mainPanel.Controls.Add(topPanel);

            Controls.Add(mainPanel);
            Controls.Add(headerPanel);

            // Events
            btnTimSP.Click += (s, e) => TimSanPham();
            btnThemVaoHD.Click += (s, e) => ThemVaoHoaDon();
            btnThanhToan.Click += (s, e) => XuLyThanhToan();

            ResumeLayout();
        }

        private void TimSanPham()
        {
            try
            {
                List<SanPham> danhSach = spBUS.LayDS();
                var sp = danhSach.FirstOrDefault(x => x.TenSP.Contains(txtTimSP.Text) || x.MaSP.Contains(txtTimSP.Text));
                if (sp != null)
                {
                    txtDonGia.Text = sp.GiaBan.ToString("N0");
                    txtSoLuongTon.Text = sp.SoLuongTon.ToString();
                }
            }
            catch { }
        }

        private void ThemVaoHoaDon()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDonGia.Text) || !int.TryParse(txtSoLuong.Text, out int soLuong))
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ", "Thông báo");
                    return;
                }
                List<SanPham> danhSach = spBUS.LayDS();
                var sp = danhSach.FirstOrDefault(x => x.TenSP.Contains(txtTimSP.Text) || x.MaSP.Contains(txtTimSP.Text));
                if (sp != null)
                {
                    dgvChiTietHD.Rows.Add(dgvChiTietHD.Rows.Count + 1, sp.MaSP, sp.TenSP, sp.GiaBan, soLuong, sp.GiaBan * soLuong);
                    TinhTongTien();
                    txtSoLuong.Clear();
                    txtDonGia.Clear();
                    txtSoLuongTon.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void TinhTongTien()
        {
            decimal tongTien = 0;
            foreach (DataGridViewRow row in dgvChiTietHD.Rows)
            {
                if (decimal.TryParse(row.Cells["ThanhTien"].Value?.ToString(), out decimal thanhTien))
                    tongTien += thanhTien;
            }
            lblTongTien.Text = tongTien.ToString("N0") + " đ";
        }

        private void XuLyThanhToan()
        {
            if (dgvChiTietHD.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm sản phẩm vào hóa đơn", "Thông báo");
                return;
            }
            MessageBox.Show("Thanh toán thành công!\nTổng tiền: " + lblTongTien.Text, "Thông báo");
            LamMoiForm();
        }

        private void LamMoiForm()
        {
            txtMaHD.Clear();
            txtNgayBan.Clear();
            txtTenKH.Clear();
            txtTimSP.Clear();
            txtDonGia.Clear();
            txtSoLuongTon.Clear();
            txtSoLuong.Clear();
            dgvChiTietHD.Rows.Clear();
            lblTongTien.Text = "0 đ";
        }
    }
}
