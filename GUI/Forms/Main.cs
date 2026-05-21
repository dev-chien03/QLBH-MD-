using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class Main : Form
    {
        private readonly SanPhamBUS spBUS = new SanPhamBUS();
        private readonly LoaiSPBUS loaiBUS = new LoaiSPBUS();
        private readonly KhachHangBUS khBUS = new KhachHangBUS();
        private readonly HoaDonBUS hdBUS = new HoaDonBUS();
        private readonly PhieuNhapBUS pnBUS = new PhieuNhapBUS();

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BuildDashboard();
        }

        private void btnQuanLySanPham_Click(object sender, EventArgs e)
        {
            BuildDashboard();
        }

        private void btnBanHang_Click(object sender, EventArgs e)
        {
            BuildDashboard();
        }

        private void BuildDashboard()
        {
            SuspendLayout();
            Controls.Clear();

            Text = "Quản Lý Bán Hàng";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            BackColor = Color.FromArgb(240, 243, 248);

            TabControl tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                Padding = new Point(16, 6)
            };

            tabControl.TabPages.Add(CreateProductTab());
            tabControl.TabPages.Add(CreateCustomerTab());
            tabControl.TabPages.Add(CreateInvoiceTab());
            tabControl.TabPages.Add(CreateImportTab());

            Controls.Add(tabControl);
            ResumeLayout(true);
        }

        private TabPage CreateProductTab()
        {
            TabPage tab = new TabPage("Quản Lý Sản Phẩm") { BackColor = Color.White };
            Panel top = new Panel { Dock = DockStyle.Top, Height = 160, Padding = new Padding(12), BackColor = Color.WhiteSmoke };

            TextBox txtSearch = new TextBox { Left = 90, Top = 12, Width = 260 };
            Label lblSearch = new Label { Text = "Tìm kiếm:", Left = 12, Top = 16, Width = 72 };

            Label lblMaSP = new Label { Text = "Mã SP:", Left = 12, Top = 52, Width = 72 };
            TextBox txtMaSP = new TextBox { Left = 90, Top = 48, Width = 150 };
            Label lblTenSP = new Label { Text = "Tên SP:", Left = 260, Top = 52, Width = 72 };
            TextBox txtTenSP = new TextBox { Left = 338, Top = 48, Width = 220 };
            Label lblGia = new Label { Text = "Giá bán:", Left = 580, Top = 52, Width = 72 };
            TextBox txtGia = new TextBox { Left = 658, Top = 48, Width = 130 };
            Label lblTon = new Label { Text = "Tồn kho:", Left = 810, Top = 52, Width = 72 };
            TextBox txtTon = new TextBox { Left = 888, Top = 48, Width = 110 };

            Label lblDVT = new Label { Text = "ĐVT:", Left = 12, Top = 92, Width = 72 };
            TextBox txtDVT = new TextBox { Left = 90, Top = 88, Width = 150 };
            Label lblLoai = new Label { Text = "Loại:", Left = 260, Top = 92, Width = 72 };
            ComboBox cboLoai = new ComboBox { Left = 338, Top = 88, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };

            Button btnThem = new Button { Text = "Thêm", Left = 580, Top = 86, Width = 100, Height = 30 };
            Button btnSua = new Button { Text = "Sửa", Left = 690, Top = 86, Width = 100, Height = 30 };
            Button btnXoa = new Button { Text = "Xóa", Left = 800, Top = 86, Width = 100, Height = 30 };
            Button btnTai = new Button { Text = "Tải lại", Left = 910, Top = 86, Width = 100, Height = 30 };

            DataGridView dgv = CreateGrid();
            dgv.Dock = DockStyle.Fill;

            top.Controls.AddRange(new Control[] { lblSearch, txtSearch, lblMaSP, txtMaSP, lblTenSP, txtTenSP, lblGia, txtGia, lblTon, txtTon, lblDVT, txtDVT, lblLoai, cboLoai, btnThem, btnSua, btnXoa, btnTai });
            tab.Controls.Add(dgv);
            tab.Controls.Add(top);

            Action loadLoai = () =>
            {
                try
                {
                    cboLoai.DataSource = loaiBUS.LayDS();
                    cboLoai.DisplayMember = "TenLoai";
                    cboLoai.ValueMember = "MaLoai";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không tải được danh mục loại sản phẩm: {ex.Message}", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboLoai.DataSource = null;
                }
            };

            Action loadData = () =>
            {
                try
                {
                    var data = spBUS.LayDS();
                    if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                    {
                        string keyword = txtSearch.Text.Trim().ToLowerInvariant();
                        data = data.Where(x => (x.TenSP ?? string.Empty).ToLowerInvariant().Contains(keyword) || (x.MaSP ?? string.Empty).ToLowerInvariant().Contains(keyword)).ToList();
                    }

                    dgv.DataSource = data;
                    if (dgv.Columns["MaSP"] != null) dgv.Columns["MaSP"].HeaderText = "Mã Sản Phẩm";
                    if (dgv.Columns["TenSP"] != null) dgv.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                    if (dgv.Columns["GiaBan"] != null) dgv.Columns["GiaBan"].HeaderText = "Giá Bán";
                    if (dgv.Columns["SoLuongTon"] != null) dgv.Columns["SoLuongTon"].HeaderText = "Tồn Kho";
                    if (dgv.Columns["DonViTinh"] != null) dgv.Columns["DonViTinh"].HeaderText = "ĐVT";
                    if (dgv.Columns["MaLoai"] != null) dgv.Columns["MaLoai"].HeaderText = "Mã Loại";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không tải được danh sách sản phẩm: {ex.Message}", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dgv.DataSource = new List<SanPham>();
                }
            };

            Action clearInputs = () =>
            {
                txtMaSP.Clear();
                txtTenSP.Clear();
                txtGia.Clear();
                txtTon.Clear();
                txtDVT.Clear();
                txtMaSP.Enabled = true;
                if (cboLoai.Items.Count > 0) cboLoai.SelectedIndex = 0;
            };

            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                var row = dgv.Rows[e.RowIndex];
                txtMaSP.Text = row.Cells["MaSP"].Value?.ToString();
                txtTenSP.Text = row.Cells["TenSP"].Value?.ToString();
                txtGia.Text = row.Cells["GiaBan"].Value?.ToString();
                txtTon.Text = row.Cells["SoLuongTon"].Value?.ToString();
                txtDVT.Text = row.Cells["DonViTinh"].Value?.ToString();

                if (row.Cells["MaLoai"].Value != null)
                    cboLoai.SelectedValue = row.Cells["MaLoai"].Value.ToString();

                txtMaSP.Enabled = false;
            };

            btnThem.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaSP.Text) || string.IsNullOrWhiteSpace(txtTenSP.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã và tên sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtGia.Text, out decimal gia)) gia = 0;
                if (!int.TryParse(txtTon.Text, out int ton)) ton = 0;

                var sp = new SanPham
                {
                    MaSP = txtMaSP.Text.Trim(),
                    TenSP = txtTenSP.Text.Trim(),
                    GiaBan = gia,
                    SoLuongTon = ton,
                    DonViTinh = txtDVT.Text.Trim(),
                    MaLoai = cboLoai.SelectedValue?.ToString() ?? string.Empty
                };

                if (spBUS.LuuSanPham(sp))
                {
                    MessageBox.Show("Thêm sản phẩm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData();
                    clearInputs();
                }
                else
                {
                    MessageBox.Show("Thêm sản phẩm thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnSua.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtGia.Text, out decimal gia)) gia = 0;
                if (!int.TryParse(txtTon.Text, out int ton)) ton = 0;

                var sp = new SanPham
                {
                    MaSP = txtMaSP.Text.Trim(),
                    TenSP = txtTenSP.Text.Trim(),
                    GiaBan = gia,
                    SoLuongTon = ton,
                    DonViTinh = txtDVT.Text.Trim(),
                    MaLoai = cboLoai.SelectedValue?.ToString() ?? string.Empty
                };

                if (spBUS.SuaSanPham(sp))
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData();
                    clearInputs();
                }
                else
                {
                    MessageBox.Show("Cập nhật sản phẩm thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnXoa.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Xóa sản phẩm {txtMaSP.Text}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                if (spBUS.XoaSanPham(txtMaSP.Text.Trim()))
                {
                    MessageBox.Show("Xóa sản phẩm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData();
                    clearInputs();
                }
                else
                {
                    MessageBox.Show("Xóa sản phẩm thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnTai.Click += (s, e) => loadData();
            txtSearch.TextChanged += (s, e) => loadData();

            loadLoai();
            loadData();
            return tab;
        }

        private TabPage CreateCustomerTab()
        {
            TabPage tab = new TabPage("Quản Lý Khách Hàng") { BackColor = Color.White };
            Panel top = new Panel { Dock = DockStyle.Top, Height = 145, Padding = new Padding(12), BackColor = Color.WhiteSmoke };

            Label lblMa = new Label { Text = "Mã KH:", Left = 12, Top = 16, Width = 72 };
            TextBox txtMa = new TextBox { Left = 90, Top = 12, Width = 150 };
            Label lblTen = new Label { Text = "Tên KH:", Left = 260, Top = 16, Width = 72 };
            TextBox txtTen = new TextBox { Left = 338, Top = 12, Width = 220 };
            Label lblSdt = new Label { Text = "SĐT:", Left = 580, Top = 16, Width = 72 };
            TextBox txtSdt = new TextBox { Left = 658, Top = 12, Width = 220 };
            Label lblDiaChi = new Label { Text = "Địa chỉ:", Left = 12, Top = 58, Width = 72 };
            TextBox txtDiaChi = new TextBox { Left = 90, Top = 54, Width = 788 };

            Button btnThem = new Button { Text = "Thêm", Left = 90, Top = 95, Width = 100, Height = 30 };
            Button btnSua = new Button { Text = "Sửa", Left = 200, Top = 95, Width = 100, Height = 30 };
            Button btnXoa = new Button { Text = "Xóa", Left = 310, Top = 95, Width = 100, Height = 30 };
            Button btnTai = new Button { Text = "Tải lại", Left = 420, Top = 95, Width = 100, Height = 30 };

            DataGridView dgv = CreateGrid();
            dgv.Dock = DockStyle.Fill;

            top.Controls.AddRange(new Control[] { lblMa, txtMa, lblTen, txtTen, lblSdt, txtSdt, lblDiaChi, txtDiaChi, btnThem, btnSua, btnXoa, btnTai });
            tab.Controls.Add(dgv);
            tab.Controls.Add(top);

            Action loadData = () =>
            {
                try
                {
                    dgv.DataSource = khBUS.LayDS();
                    if (dgv.Columns["MaKH"] != null) dgv.Columns["MaKH"].HeaderText = "Mã Khách Hàng";
                    if (dgv.Columns["TenKH"] != null) dgv.Columns["TenKH"].HeaderText = "Tên Khách Hàng";
                    if (dgv.Columns["SoDienThoai"] != null) dgv.Columns["SoDienThoai"].HeaderText = "Số Điện Thoại";
                    if (dgv.Columns["DiaChi"] != null) dgv.Columns["DiaChi"].HeaderText = "Địa Chỉ";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không tải được danh sách khách hàng: {ex.Message}", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dgv.DataSource = new List<KhachHang>();
                }
            };

            Action clearInputs = () =>
            {
                txtMa.Clear();
                txtTen.Clear();
                txtSdt.Clear();
                txtDiaChi.Clear();
                txtMa.Enabled = true;
            };

            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                var row = dgv.Rows[e.RowIndex];
                txtMa.Text = row.Cells["MaKH"].Value?.ToString();
                txtTen.Text = row.Cells["TenKH"].Value?.ToString();
                txtSdt.Text = row.Cells["SoDienThoai"].Value?.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
                txtMa.Enabled = false;
            };

            txtSdt.TextChanged += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSdt.Text)) return;

                var found = khBUS.TimKiemNhanh(txtSdt.Text.Trim());
                if (found != null)
                {
                    txtMa.Text = found.MaKH;
                    txtTen.Text = found.TenKH;
                    txtDiaChi.Text = found.DiaChi;
                }
            };

            btnThem.Click += (s, e) =>
            {
                var kh = new KhachHang
                {
                    MaKH = txtMa.Text.Trim(),
                    TenKH = txtTen.Text.Trim(),
                    SoDienThoai = txtSdt.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim()
                };

                if (khBUS.ThemKhachHang(kh))
                {
                    MessageBox.Show("Thêm khách hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData();
                    clearInputs();
                }
                else
                {
                    MessageBox.Show("Thêm khách hàng thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnSua.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text))
                {
                    MessageBox.Show("Vui lòng chọn khách hàng cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var kh = new KhachHang
                {
                    MaKH = txtMa.Text.Trim(),
                    TenKH = txtTen.Text.Trim(),
                    SoDienThoai = txtSdt.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim()
                };

                if (khBUS.SuaKhachHang(kh))
                {
                    MessageBox.Show("Cập nhật khách hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData();
                    clearInputs();
                }
                else
                {
                    MessageBox.Show("Cập nhật khách hàng thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnXoa.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text))
                {
                    MessageBox.Show("Vui lòng chọn khách hàng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Xóa khách hàng {txtMa.Text}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                if (khBUS.XoaKhachHang(txtMa.Text.Trim()))
                {
                    MessageBox.Show("Xóa khách hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData();
                    clearInputs();
                }
                else
                {
                    MessageBox.Show("Xóa khách hàng thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnTai.Click += (s, e) => loadData();

            loadData();
            return tab;
        }

        private TabPage CreateInvoiceTab()
        {
            TabPage tab = new TabPage("Quản Lý Hóa Đơn") { BackColor = Color.White };
            Panel top = new Panel { Dock = DockStyle.Top, Height = 130, Padding = new Padding(12), BackColor = Color.WhiteSmoke };

            Label lblMaHD = new Label { Text = "Mã HĐ:", Left = 12, Top = 16, Width = 72 };
            TextBox txtMaHD = new TextBox { Left = 90, Top = 12, Width = 160 };
            Label lblMaKH = new Label { Text = "Mã KH:", Left = 270, Top = 16, Width = 72 };
            TextBox txtMaKH = new TextBox { Left = 348, Top = 12, Width = 160 };
            Label lblNgay = new Label { Text = "Ngày lập:", Left = 528, Top = 16, Width = 72 };
            TextBox txtNgay = new TextBox { Left = 606, Top = 12, Width = 180 };
            Label lblTong = new Label { Text = "Tổng tiền:", Left = 12, Top = 58, Width = 72 };
            TextBox txtTong = new TextBox { Left = 90, Top = 54, Width = 160 };

            Button btnMaMoi = new Button { Text = "Tạo mã", Left = 270, Top = 52, Width = 100, Height = 30 };
            Button btnThem = new Button { Text = "Lưu hóa đơn", Left = 380, Top = 52, Width = 120, Height = 30 };
            Button btnTai = new Button { Text = "Tải lại", Left = 510, Top = 52, Width = 100, Height = 30 };

            DataGridView dgv = CreateGrid();
            dgv.Dock = DockStyle.Fill;

            top.Controls.AddRange(new Control[] { lblMaHD, txtMaHD, lblMaKH, txtMaKH, lblNgay, txtNgay, lblTong, txtTong, btnMaMoi, btnThem, btnTai });
            tab.Controls.Add(dgv);
            tab.Controls.Add(top);

            Action loadData = () =>
            {
                try
                {
                    dgv.DataSource = hdBUS.LayDS();
                    if (dgv.Columns["MaHD"] != null) dgv.Columns["MaHD"].HeaderText = "Mã Hóa Đơn";
                    if (dgv.Columns["NgayLap"] != null) dgv.Columns["NgayLap"].HeaderText = "Ngày Lập";
                    if (dgv.Columns["MaKH"] != null) dgv.Columns["MaKH"].HeaderText = "Mã Khách Hàng";
                    if (dgv.Columns["TongTien"] != null) dgv.Columns["TongTien"].HeaderText = "Tổng Tiền";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không tải được danh sách hóa đơn: {ex.Message}", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dgv.DataSource = new List<HoaDon>();
                }
            };

            Action clearInputs = () =>
            {
                txtMaHD.Clear();
                txtMaKH.Clear();
                txtNgay.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtTong.Clear();
            };

            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                var row = dgv.Rows[e.RowIndex];
                txtMaHD.Text = row.Cells["MaHD"].Value?.ToString();
                txtNgay.Text = Convert.ToDateTime(row.Cells["NgayLap"].Value).ToString("yyyy-MM-dd");
                txtMaKH.Text = row.Cells["MaKH"].Value?.ToString();
                txtTong.Text = row.Cells["TongTien"].Value?.ToString();
            };

            btnMaMoi.Click += (s, e) => txtMaHD.Text = hdBUS.TaoMaHDMoi();

            btnThem.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaHD.Text) || string.IsNullOrWhiteSpace(txtMaKH.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã hóa đơn và mã khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!DateTime.TryParse(txtNgay.Text, out DateTime ngayLap)) ngayLap = DateTime.Now;
                if (!decimal.TryParse(txtTong.Text, out decimal tongTien)) tongTien = 0;

                var hd = new HoaDon
                {
                    MaHD = txtMaHD.Text.Trim(),
                    MaKH = txtMaKH.Text.Trim(),
                    NgayLap = ngayLap,
                    TongTien = tongTien
                };

                if (hdBUS.LuuHoaDon(hd))
                {
                    MessageBox.Show("Lưu hóa đơn thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData();
                    clearInputs();
                }
                else
                {
                    MessageBox.Show("Lưu hóa đơn thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnTai.Click += (s, e) => loadData();

            txtNgay.Text = DateTime.Now.ToString("yyyy-MM-dd");
            loadData();
            return tab;
        }

        private TabPage CreateImportTab()
        {
            TabPage tab = new TabPage("Quản Lý Phiếu Nhập") { BackColor = Color.White };
            Panel top = new Panel { Dock = DockStyle.Top, Height = 130, Padding = new Padding(12), BackColor = Color.WhiteSmoke };

            Label lblMaPN = new Label { Text = "Mã PN:", Left = 12, Top = 16, Width = 72 };
            TextBox txtMaPN = new TextBox { Left = 90, Top = 12, Width = 160 };
            Label lblNgay = new Label { Text = "Ngày nhập:", Left = 270, Top = 16, Width = 72 };
            TextBox txtNgay = new TextBox { Left = 348, Top = 12, Width = 160 };
            Label lblTong = new Label { Text = "Tổng tiền:", Left = 528, Top = 16, Width = 72 };
            TextBox txtTong = new TextBox { Left = 606, Top = 12, Width = 180 };

            Button btnMaMoi = new Button { Text = "Tạo mã", Left = 90, Top = 52, Width = 100, Height = 30 };
            Button btnThem = new Button { Text = "Lưu phiếu nhập", Left = 200, Top = 52, Width = 130, Height = 30 };
            Button btnTai = new Button { Text = "Tải lại", Left = 340, Top = 52, Width = 100, Height = 30 };

            DataGridView dgv = CreateGrid();
            dgv.Dock = DockStyle.Fill;

            top.Controls.AddRange(new Control[] { lblMaPN, txtMaPN, lblNgay, txtNgay, lblTong, txtTong, btnMaMoi, btnThem, btnTai });
            tab.Controls.Add(dgv);
            tab.Controls.Add(top);

            Action loadData = () =>
            {
                try
                {
                    dgv.DataSource = pnBUS.LayDS();
                    if (dgv.Columns["MaPN"] != null) dgv.Columns["MaPN"].HeaderText = "Mã Phiếu Nhập";
                    if (dgv.Columns["NgayNhap"] != null) dgv.Columns["NgayNhap"].HeaderText = "Ngày Nhập";
                    if (dgv.Columns["TongTienNhap"] != null) dgv.Columns["TongTienNhap"].HeaderText = "Tổng Tiền Nhập";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không tải được danh sách phiếu nhập: {ex.Message}", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dgv.DataSource = new List<PhieuNhap>();
                }
            };

            Action clearInputs = () =>
            {
                txtMaPN.Clear();
                txtNgay.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtTong.Clear();
            };

            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                var row = dgv.Rows[e.RowIndex];
                txtMaPN.Text = row.Cells["MaPN"].Value?.ToString();
                txtNgay.Text = Convert.ToDateTime(row.Cells["NgayNhap"].Value).ToString("yyyy-MM-dd");
                txtTong.Text = row.Cells["TongTienNhap"].Value?.ToString();
            };

            btnMaMoi.Click += (s, e) => txtMaPN.Text = "PN" + DateTime.Now.ToString("yyMMddHHmmss");

            btnThem.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaPN.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã phiếu nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!DateTime.TryParse(txtNgay.Text, out DateTime ngayNhap)) ngayNhap = DateTime.Now;
                if (!decimal.TryParse(txtTong.Text, out decimal tongTien)) tongTien = 0;

                var pn = new PhieuNhap
                {
                    MaPN = txtMaPN.Text.Trim(),
                    NgayNhap = ngayNhap,
                    TongTienNhap = tongTien
                };

                if (pnBUS.LuuPhieuNhap(pn))
                {
                    MessageBox.Show("Lưu phiếu nhập thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData();
                    clearInputs();
                }
                else
                {
                    MessageBox.Show("Lưu phiếu nhập thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnTai.Click += (s, e) => loadData();

            txtNgay.Text = DateTime.Now.ToString("yyyy-MM-dd");
            loadData();
            return tab;
        }

        private DataGridView CreateGrid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false
            };
        }
    }
}
