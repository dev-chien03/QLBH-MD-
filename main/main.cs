using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void main_Load(object sender, EventArgs e)
        {
            this.Text = "Quản Lý Kho Hàng";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1000, 700);
            this.BackColor = Color.FromArgb(240, 240, 240);
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Create Tab Control
            TabControl tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            tabControl.Margin = new Padding(10);

            // Tab 1: Products
            TabPage tabProducts = new TabPage("Quản Lý Sản Phẩm");
            CreateProductsTab(tabProducts);
            tabControl.TabPages.Add(tabProducts);

            // Tab 2: Customers
            TabPage tabCustomers = new TabPage("Quản Lý Khách Hàng");
            CreateCustomersTab(tabCustomers);
            tabControl.TabPages.Add(tabCustomers);

            // Tab 3: Invoices
            TabPage tabInvoices = new TabPage("Quản Lý Hóa Đơn");
            CreateInvoicesTab(tabInvoices);
            tabControl.TabPages.Add(tabInvoices);

            // Tab 4: Import Receipts
            TabPage tabImports = new TabPage("Quản Lý Phiếu Nhập");
            CreateImportsTab(tabImports);
            tabControl.TabPages.Add(tabImports);

            this.Controls.Add(tabControl);
        }

        private void CreateProductsTab(TabPage tab)
        {
            Panel pnlTop = new Panel() { Dock = DockStyle.Top, Height = 80, BackColor = Color.White, Padding = new Padding(10) };

            Label lblMaSP = new Label() { Text = "Mã SP:", Left = 10, Top = 10, Width = 60 };
            TextBox txtMaSP = new TextBox() { Left = 80, Top = 10, Width = 150 };

            Label lblTenSP = new Label() { Text = "Tên SP:", Left = 10, Top = 40, Width = 60 };
            TextBox txtTenSP = new TextBox() { Left = 80, Top = 40, Width = 150 };

            Label lblGia = new Label() { Text = "Giá Bán:", Left = 250, Top = 10, Width = 70 };
            TextBox txtGia = new TextBox() { Left = 330, Top = 10, Width = 100 };

            Label lblSoLuong = new Label() { Text = "Số Lượng:", Left = 250, Top = 40, Width = 70 };
            TextBox txtSoLuong = new TextBox() { Left = 330, Top = 40, Width = 100 };

            Button btnThem = new Button() { Text = "Thêm", Left = 450, Top = 10, Width = 80, Height = 30, BackColor = Color.LightGreen };
            Button btnSua = new Button() { Text = "Sửa", Left = 540, Top = 10, Width = 80, Height = 30, BackColor = Color.LightBlue };
            Button btnXoa = new Button() { Text = "Xóa", Left = 630, Top = 10, Width = 80, Height = 30, BackColor = Color.LightCoral };
            Button btnLoad = new Button() { Text = "Tải Lại", Left = 720, Top = 10, Width = 80, Height = 30 };

            pnlTop.Controls.AddRange(new Control[] { lblMaSP, txtMaSP, lblTenSP, txtTenSP, lblGia, txtGia, lblSoLuong, txtSoLuong, btnThem, btnSua, btnXoa, btnLoad });

            DataGridView dgvProducts = new DataGridView() { Dock = DockStyle.Fill, AllowUserToAddRows = false };
            dgvProducts.Columns.Add("MaSP", "Mã SP");
            dgvProducts.Columns.Add("TenSP", "Tên SP");
            dgvProducts.Columns.Add("GiaBan", "Giá Bán");
            dgvProducts.Columns.Add("SoLuongTon", "Số Lượng");
            dgvProducts.Columns.Add("DonViTinh", "Đơn Vị");
            dgvProducts.Columns.Add("MaLoai", "Mã Loại");

            tab.Controls.Add(dgvProducts);
            tab.Controls.Add(pnlTop);

            // Load data
            BUS.SanPhamBUS busSP = new BUS.SanPhamBUS();
            LoadProductData(dgvProducts, busSP);

            // Events
            dgvProducts.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    txtMaSP.Text = dgvProducts.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                    txtTenSP.Text = dgvProducts.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";
                    txtGia.Text = dgvProducts.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "";
                    txtSoLuong.Text = dgvProducts.Rows[e.RowIndex].Cells[3].Value?.ToString() ?? "";
                }
            };

            btnThem.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaSP.Text) || string.IsNullOrWhiteSpace(txtTenSP.Text))
                {
                    MessageBox.Show("Vui lòng nhập đủ thông tin", "Thông báo");
                    return;
                }

                var sp = new DTO.SanPham
                {
                    MaSP = txtMaSP.Text,
                    TenSP = txtTenSP.Text,
                    GiaBan = decimal.Parse(txtGia.Text ?? "0"),
                    SoLuongTon = int.Parse(txtSoLuong.Text ?? "0"),
                    DonViTinh = "Cái",
                    MaLoai = "L001"
                };

                if (busSP.LuuSanPham(sp))
                {
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thành công");
                    LoadProductData(dgvProducts, busSP);
                    ClearProductFields(txtMaSP, txtTenSP, txtGia, txtSoLuong);
                }
                else
                {
                    MessageBox.Show("Thêm sản phẩm thất bại!", "Lỗi");
                }
            };

            btnXoa.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để xóa", "Thông báo");
                    return;
                }

                if (MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (busSP.XoaSanPham(txtMaSP.Text))
                    {
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thành công");
                        LoadProductData(dgvProducts, busSP);
                        ClearProductFields(txtMaSP, txtTenSP, txtGia, txtSoLuong);
                    }
                }
            };

            btnLoad.Click += (s, e) => LoadProductData(dgvProducts, busSP);
        }

        private void CreateCustomersTab(TabPage tab)
        {
            Panel pnlTop = new Panel() { Dock = DockStyle.Top, Height = 80, BackColor = Color.White, Padding = new Padding(10) };

            Label lblMaKH = new Label() { Text = "Mã KH:", Left = 10, Top = 10, Width = 60 };
            TextBox txtMaKH = new TextBox() { Left = 80, Top = 10, Width = 150 };

            Label lblTenKH = new Label() { Text = "Tên KH:", Left = 10, Top = 40, Width = 60 };
            TextBox txtTenKH = new TextBox() { Left = 80, Top = 40, Width = 150 };

            Label lblSDT = new Label() { Text = "SĐT:", Left = 250, Top = 10, Width = 50 };
            TextBox txtSDT = new TextBox() { Left = 310, Top = 10, Width = 120 };

            Label lblDiaChi = new Label() { Text = "Địa Chỉ:", Left = 250, Top = 40, Width = 60 };
            TextBox txtDiaChi = new TextBox() { Left = 310, Top = 40, Width = 240 };

            Button btnThem = new Button() { Text = "Thêm", Left = 570, Top = 10, Width = 80, Height = 30, BackColor = Color.LightGreen };
            Button btnSua = new Button() { Text = "Sửa", Left = 660, Top = 10, Width = 80, Height = 30, BackColor = Color.LightBlue };
            Button btnXoa = new Button() { Text = "Xóa", Left = 750, Top = 10, Width = 80, Height = 30, BackColor = Color.LightCoral };
            Button btnLoad = new Button() { Text = "Tải Lại", Left = 840, Top = 10, Width = 80, Height = 30 };

            pnlTop.Controls.AddRange(new Control[] { lblMaKH, txtMaKH, lblTenKH, txtTenKH, lblSDT, txtSDT, lblDiaChi, txtDiaChi, btnThem, btnSua, btnXoa, btnLoad });

            DataGridView dgvCustomers = new DataGridView() { Dock = DockStyle.Fill, AllowUserToAddRows = false };
            dgvCustomers.Columns.Add("MaKH", "Mã KH");
            dgvCustomers.Columns.Add("TenKH", "Tên KH");
            dgvCustomers.Columns.Add("SoDienThoai", "SĐT");
            dgvCustomers.Columns.Add("DiaChi", "Địa Chỉ");

            tab.Controls.Add(dgvCustomers);
            tab.Controls.Add(pnlTop);

            BUS.KhachHangBUS busKH = new BUS.KhachHangBUS();
            LoadCustomerData(dgvCustomers, busKH);

            dgvCustomers.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    txtMaKH.Text = dgvCustomers.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                    txtTenKH.Text = dgvCustomers.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";
                    txtSDT.Text = dgvCustomers.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "";
                    txtDiaChi.Text = dgvCustomers.Rows[e.RowIndex].Cells[3].Value?.ToString() ?? "";
                }
            };

            btnThem.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaKH.Text) || string.IsNullOrWhiteSpace(txtTenKH.Text) || string.IsNullOrWhiteSpace(txtSDT.Text))
                {
                    MessageBox.Show("Vui lòng nhập đủ thông tin", "Thông báo");
                    return;
                }

                var kh = new DTO.KhachHang
                {
                    MaKH = txtMaKH.Text,
                    TenKH = txtTenKH.Text,
                    SoDienThoai = txtSDT.Text,
                    DiaChi = txtDiaChi.Text
                };

                if (busKH.ThemKhachHang(kh))
                {
                    MessageBox.Show("Thêm khách hàng thành công!", "Thành công");
                    LoadCustomerData(dgvCustomers, busKH);
                    ClearCustomerFields(txtMaKH, txtTenKH, txtSDT, txtDiaChi);
                }
                else
                {
                    MessageBox.Show("Thêm khách hàng thất bại!", "Lỗi");
                }
            };

            btnSua.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaKH.Text))
                {
                    MessageBox.Show("Vui lòng chọn khách hàng để sửa", "Thông báo");
                    return;
                }

                var kh = new DTO.KhachHang
                {
                    MaKH = txtMaKH.Text,
                    TenKH = txtTenKH.Text,
                    SoDienThoai = txtSDT.Text,
                    DiaChi = txtDiaChi.Text
                };

                if (busKH.SuaKhachHang(kh))
                {
                    MessageBox.Show("Cập nhật khách hàng thành công!", "Thành công");
                    LoadCustomerData(dgvCustomers, busKH);
                    ClearCustomerFields(txtMaKH, txtTenKH, txtSDT, txtDiaChi);
                }
            };

            btnXoa.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaKH.Text))
                {
                    MessageBox.Show("Vui lòng chọn khách hàng để xóa", "Thông báo");
                    return;
                }

                if (MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MessageBox.Show("Xóa khách hàng thành công!", "Thành công");
                    LoadCustomerData(dgvCustomers, busKH);
                    ClearCustomerFields(txtMaKH, txtTenKH, txtSDT, txtDiaChi);
                }
            };

            btnLoad.Click += (s, e) => LoadCustomerData(dgvCustomers, busKH);
        }

        private void CreateInvoicesTab(TabPage tab)
        {
            Panel pnlTop = new Panel() { Dock = DockStyle.Top, Height = 80, BackColor = Color.White, Padding = new Padding(10) };

            Label lblMaHD = new Label() { Text = "Mã HĐ:", Left = 10, Top = 10, Width = 60 };
            TextBox txtMaHD = new TextBox() { Left = 80, Top = 10, Width = 150 };

            Label lblMaKH = new Label() { Text = "Mã KH:", Left = 250, Top = 10, Width = 60 };
            TextBox txtMaKH = new TextBox() { Left = 320, Top = 10, Width = 100 };

            Label lblNgayLap = new Label() { Text = "Ngày Lập:", Left = 430, Top = 10, Width = 70 };
            TextBox txtNgayLap = new TextBox() { Left = 510, Top = 10, Width = 120 };

            Label lblTongTien = new Label() { Text = "Tổng Tiền:", Left = 250, Top = 40, Width = 70 };
            TextBox txtTongTien = new TextBox() { Left = 320, Top = 40, Width = 100 };

            Button btnThem = new Button() { Text = "Thêm", Left = 450, Top = 40, Width = 80, Height = 30, BackColor = Color.LightGreen };
            Button btnLoad = new Button() { Text = "Tải Lại", Left = 540, Top = 40, Width = 80, Height = 30 };

            pnlTop.Controls.AddRange(new Control[] { lblMaHD, txtMaHD, lblMaKH, txtMaKH, lblNgayLap, txtNgayLap, lblTongTien, txtTongTien, btnThem, btnLoad });

            DataGridView dgvInvoices = new DataGridView() { Dock = DockStyle.Fill, AllowUserToAddRows = false };
            dgvInvoices.Columns.Add("MaHD", "Mã HĐ");
            dgvInvoices.Columns.Add("NgayLap", "Ngày Lập");
            dgvInvoices.Columns.Add("MaKH", "Mã KH");
            dgvInvoices.Columns.Add("TongTien", "Tổng Tiền");

            tab.Controls.Add(dgvInvoices);
            tab.Controls.Add(pnlTop);

            BUS.HoaDonBUS busHD = new BUS.HoaDonBUS();
            LoadInvoiceData(dgvInvoices, busHD);

            dgvInvoices.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    txtMaHD.Text = dgvInvoices.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                    txtNgayLap.Text = dgvInvoices.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";
                    txtMaKH.Text = dgvInvoices.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "";
                    txtTongTien.Text = dgvInvoices.Rows[e.RowIndex].Cells[3].Value?.ToString() ?? "";
                }
            };

            btnThem.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaHD.Text) || string.IsNullOrWhiteSpace(txtMaKH.Text))
                {
                    MessageBox.Show("Vui lòng nhập đủ thông tin", "Thông báo");
                    return;
                }

                var hd = new DTO.HoaDon
                {
                    MaHD = txtMaHD.Text,
                    NgayLap = DateTime.Parse(txtNgayLap.Text ?? DateTime.Now.ToString("yyyy-MM-dd")),
                    MaKH = txtMaKH.Text,
                    TongTien = decimal.Parse(txtTongTien.Text ?? "0")
                };

                if (busHD.LuuHoaDon(hd))
                {
                    MessageBox.Show("Thêm hóa đơn thành công!", "Thành công");
                    LoadInvoiceData(dgvInvoices, busHD);
                    ClearInvoiceFields(txtMaHD, txtMaKH, txtNgayLap, txtTongTien);
                }
            };

            btnLoad.Click += (s, e) => LoadInvoiceData(dgvInvoices, busHD);
        }

        private void CreateImportsTab(TabPage tab)
        {
            Panel pnlTop = new Panel() { Dock = DockStyle.Top, Height = 80, BackColor = Color.White, Padding = new Padding(10) };

            Label lblMaPN = new Label() { Text = "Mã PN:", Left = 10, Top = 10, Width = 60 };
            TextBox txtMaPN = new TextBox() { Left = 80, Top = 10, Width = 150 };

            Label lblNgayNhap = new Label() { Text = "Ngày Nhập:", Left = 250, Top = 10, Width = 70 };
            TextBox txtNgayNhap = new TextBox() { Left = 330, Top = 10, Width = 150 };

            Label lblTongTien = new Label() { Text = "Tổng Tiền:", Left = 10, Top = 40, Width = 70 };
            TextBox txtTongTien = new TextBox() { Left = 80, Top = 40, Width = 150 };

            Button btnThem = new Button() { Text = "Thêm", Left = 250, Top = 40, Width = 80, Height = 30, BackColor = Color.LightGreen };
            Button btnLoad = new Button() { Text = "Tải Lại", Left = 340, Top = 40, Width = 80, Height = 30 };

            pnlTop.Controls.AddRange(new Control[] { lblMaPN, txtMaPN, lblNgayNhap, txtNgayNhap, lblTongTien, txtTongTien, btnThem, btnLoad });

            DataGridView dgvImports = new DataGridView() { Dock = DockStyle.Fill, AllowUserToAddRows = false };
            dgvImports.Columns.Add("MaPN", "Mã PN");
            dgvImports.Columns.Add("NgayNhap", "Ngày Nhập");
            dgvImports.Columns.Add("TongTienNhap", "Tổng Tiền");

            tab.Controls.Add(dgvImports);
            tab.Controls.Add(pnlTop);

            BUS.PhieuNhapBUS busPN = new BUS.PhieuNhapBUS();
            LoadImportData(dgvImports, busPN);

            dgvImports.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    txtMaPN.Text = dgvImports.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                    txtNgayNhap.Text = dgvImports.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";
                    txtTongTien.Text = dgvImports.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "";
                }
            };

            btnThem.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaPN.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã phiếu nhập", "Thông báo");
                    return;
                }

                var pn = new DTO.PhieuNhap
                {
                    MaPN = txtMaPN.Text,
                    NgayNhap = DateTime.Parse(txtNgayNhap.Text ?? DateTime.Now.ToString("yyyy-MM-dd")),
                    TongTienNhap = decimal.Parse(txtTongTien.Text ?? "0")
                };

                if (busPN.LuuPhieuNhap(pn))
                {
                    MessageBox.Show("Thêm phiếu nhập thành công!", "Thành công");
                    LoadImportData(dgvImports, busPN);
                    ClearImportFields(txtMaPN, txtNgayNhap, txtTongTien);
                }
            };

            btnLoad.Click += (s, e) => LoadImportData(dgvImports, busPN);
        }

        private void LoadProductData(DataGridView dgv, BUS.SanPhamBUS bus)
        {
            dgv.Rows.Clear();
            var data = bus.LayDS();
            foreach (var item in data)
            {
                dgv.Rows.Add(item.MaSP, item.TenSP, item.GiaBan, item.SoLuongTon, item.DonViTinh, item.MaLoai);
            }
        }

        private void LoadCustomerData(DataGridView dgv, BUS.KhachHangBUS bus)
        {
            dgv.Rows.Clear();
            var data = bus.LayDS();
            foreach (var item in data)
            {
                dgv.Rows.Add(item.MaKH, item.TenKH, item.SoDienThoai, item.DiaChi);
            }
        }

        private void LoadInvoiceData(DataGridView dgv, BUS.HoaDonBUS bus)
        {
            dgv.Rows.Clear();
            var data = bus.LayDS();
            foreach (var item in data)
            {
                dgv.Rows.Add(item.MaHD, item.NgayLap.ToString("yyyy-MM-dd"), item.MaKH, item.TongTien);
            }
        }

        private void LoadImportData(DataGridView dgv, BUS.PhieuNhapBUS bus)
        {
            dgv.Rows.Clear();
            var data = bus.LayDS();
            foreach (var item in data)
            {
                dgv.Rows.Add(item.MaPN, item.NgayNhap.ToString("yyyy-MM-dd"), item.TongTienNhap);
            }
        }

        private void ClearProductFields(TextBox txtMaSP, TextBox txtTenSP, TextBox txtGia, TextBox txtSoLuong)
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtGia.Clear();
            txtSoLuong.Clear();
        }

        private void ClearCustomerFields(TextBox txtMaKH, TextBox txtTenKH, TextBox txtSDT, TextBox txtDiaChi)
        {
            txtMaKH.Clear();
            txtTenKH.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
        }

        private void ClearInvoiceFields(TextBox txtMaHD, TextBox txtMaKH, TextBox txtNgayLap, TextBox txtTongTien)
        {
            txtMaHD.Clear();
            txtMaKH.Clear();
            txtNgayLap.Clear();
            txtTongTien.Clear();
        }

        private void ClearImportFields(TextBox txtMaPN, TextBox txtNgayNhap, TextBox txtTongTien)
        {
            txtMaPN.Clear();
            txtNgayNhap.Clear();
            txtTongTien.Clear();
        }
    }
}
