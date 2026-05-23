using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class frmBanHang : Form
    {
        private SanPhamBUS spBUS = new SanPhamBUS();
        private KhachHangBUS khBUS = new KhachHangBUS();
        private HoaDonBUS hdBUS = new HoaDonBUS();

        private TextBox txtMaHD, txtNgayBan, txtKhachHang, txtTimSP, txtProdInfo, txtSoLuong;
        private Label lblDonGiaVal, lblSoLuongTonVal, lblTongTien, lblMaKH;
        private DataGridView dgvItems;
        private Button btnTim, btnAdd, btnThanhToan, btnXoaItem, btnHoaDonMoi;

        private SanPham _spDangChon = null;
        private string _maKH = null;
        private readonly List<ChiTietHD> _gioHang = new List<ChiTietHD>();

        public frmBanHang()
        {
            InitializeComponent();
            this.Load += (s, e) => { BuildLayout(); InitInvoice(); };
        }

        private void BuildLayout()
        {
            this.SuspendLayout();
            this.Controls.Clear();
            this.Text = "Bán Hàng";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Header
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(200, 240, 180), BorderStyle = BorderStyle.FixedSingle };
            Label lblTitle = new Label { Dock = DockStyle.Fill, Text = "BÁN HÀNG", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 14, FontStyle.Bold) };
            pnlHeader.Controls.Add(lblTitle);

            // Main Content Split: Left & Right
            Panel pnlContent = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };

            // ========== LEFT PANEL (Invoice & Grid) ==========
            Panel pnlLeft = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };

            // Invoice Info Section
            Panel pnlInvoiceInfo = new Panel { Dock = DockStyle.Top, Height = 100, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(10), BackColor = Color.White };
            Label lblInvoiceTitle = new Label { Text = "Thông tin hóa đơn", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 5), AutoSize = true };

            Label lbl1 = new Label { Text = "Mã hóa đơn:", Location = new Point(10, 30), AutoSize = true };
            txtMaHD = new TextBox { Location = new Point(100, 27), Size = new Size(130, 22), ReadOnly = true };

            Label lbl2 = new Label { Text = "Ngày bán:", Location = new Point(10, 58), AutoSize = true };
            txtNgayBan = new TextBox { Location = new Point(100, 55), Size = new Size(130, 22), ReadOnly = true };

            Label lbl3 = new Label { Text = "SĐT KH:", Location = new Point(250, 30), AutoSize = true };
            txtKhachHang = new TextBox { Location = new Point(320, 27), Size = new Size(120, 22) };
            lblMaKH = new Label { Location = new Point(450, 30), Size = new Size(260, 22), Font = new Font("Arial", 9, FontStyle.Italic), ForeColor = Color.DarkBlue };

            btnHoaDonMoi = new Button { Text = "Hóa đơn mới", Location = new Point(250, 55), Size = new Size(120, 25), BackColor = Color.LightGray };

            pnlInvoiceInfo.Controls.AddRange(new Control[] { lblInvoiceTitle, lbl1, txtMaHD, lbl2, txtNgayBan, lbl3, txtKhachHang, lblMaKH, btnHoaDonMoi });

            // Grid Section
            Panel pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };
            Label lblGridTitle = new Label { Text = "Danh sách sản phẩm trong hóa đơn", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 10), AutoSize = true };

            dgvItems = new DataGridView
            {
                Location = new Point(10, 35),
                Size = new Size(640, 220),
                AllowUserToAddRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            dgvItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 149, 237);
            dgvItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvItems.Columns.Add("STT", "STT");
            dgvItems.Columns.Add("MaSP", "Mã SP");
            dgvItems.Columns.Add("TenSP", "Tên Sản Phẩm");
            dgvItems.Columns.Add("DonGia", "Đơn giá");
            dgvItems.Columns.Add("SoLuong", "Số lượng");
            dgvItems.Columns.Add("ThanhTien", "Thành tiền");
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            btnXoaItem = new Button
            {
                Text = "Xóa dòng đã chọn",
                Location = new Point(10, 265),
                Size = new Size(150, 28),
                BackColor = Color.LightPink,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom
            };

            pnlGrid.Controls.Add(dgvItems);
            pnlGrid.Controls.Add(lblGridTitle);
            pnlGrid.Controls.Add(btnXoaItem);

            pnlLeft.Controls.Add(pnlGrid);
            pnlLeft.Controls.Add(pnlInvoiceInfo);

            // ========== RIGHT PANEL (Search & Product Info) ==========
            Panel pnlRight = new Panel { Dock = DockStyle.Right, Width = 300, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(10) };

            // Search Section
            Label lblSearchTitle = new Label { Text = "Tìm sản phẩm (Mã hoặc Tên)", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 10), AutoSize = true };
            txtTimSP = new TextBox { Location = new Point(10, 35), Size = new Size(200, 22) };
            btnTim = new Button { Text = "Tìm", Location = new Point(215, 35), Size = new Size(60, 22), BackColor = Color.FromArgb(100, 180, 255) };

            // Product Info Section
            Label lblProdInfoTitle = new Label { Text = "Thông tin sản phẩm", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 70), AutoSize = true };
            txtProdInfo = new TextBox
            {
                Location = new Point(10, 95),
                Size = new Size(265, 60),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };

            // Product Details Section
            Label lblDonGia = new Label { Text = "Đơn giá", Location = new Point(10, 165), AutoSize = true };
            lblDonGiaVal = new Label { Text = "0 đ", Location = new Point(90, 165), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };

            Label lblSoLuongTon = new Label { Text = "Số lượng tồn", Location = new Point(10, 190), AutoSize = true };
            lblSoLuongTonVal = new Label { Text = "0", Location = new Point(90, 190), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };

            Label lblSoLuong = new Label { Text = "Số lượng", Location = new Point(10, 215), AutoSize = true };
            txtSoLuong = new TextBox { Location = new Point(90, 212), Size = new Size(70, 22) };

            btnAdd = new Button { Text = "Thêm vào hóa đơn", Location = new Point(10, 245), Size = new Size(265, 35), BackColor = Color.FromArgb(100, 180, 255), Font = new Font("Arial", 9, FontStyle.Bold) };

            // Total Section
            Label lblTotalTitle = new Label { Text = "Tổng thanh toán", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 290), AutoSize = true };
            lblTongTien = new Label { Text = "0 đ", Font = new Font("Arial", 16, FontStyle.Bold), ForeColor = Color.Red, Location = new Point(10, 315), Size = new Size(265, 35), TextAlign = ContentAlignment.MiddleCenter, BorderStyle = BorderStyle.FixedSingle };

            btnThanhToan = new Button { Text = "Thanh toán", Location = new Point(10, 360), Size = new Size(265, 40), BackColor = Color.FromArgb(100, 200, 100), Font = new Font("Arial", 10, FontStyle.Bold) };

            pnlRight.Controls.AddRange(new Control[] {
                lblSearchTitle, txtTimSP, btnTim,
                lblProdInfoTitle, txtProdInfo,
                lblDonGia, lblDonGiaVal, lblSoLuongTon, lblSoLuongTonVal, lblSoLuong, txtSoLuong, btnAdd,
                lblTotalTitle, lblTongTien, btnThanhToan
            });

            // Add panels to content
            pnlContent.Controls.Add(pnlRight);
            pnlContent.Controls.Add(pnlLeft);

            // Add to form
            this.Controls.Add(pnlContent);
            this.Controls.Add(pnlHeader);

            // Events
            btnTim.Click += (s, e) => TimSanPham();
            txtTimSP.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TimSanPham(); } };
            btnAdd.Click += (s, e) => ThemVaoHoaDon();
            btnXoaItem.Click += (s, e) => XoaDongDaChon();
            btnThanhToan.Click += (s, e) => ThanhToan();
            btnHoaDonMoi.Click += (s, e) => InitInvoice();
            txtKhachHang.Leave += (s, e) => TimKhachHang();
            txtKhachHang.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TimKhachHang(); } };

            this.ResumeLayout();
        }

        private void InitInvoice()
        {
            try
            {
                txtMaHD.Text = hdBUS.TaoMaHDMoi();
            }
            catch (Exception ex)
            {
                txtMaHD.Text = "HD001";
                System.Diagnostics.Debug.WriteLine("TaoMaHDMoi error: " + ex.Message);
            }
            txtNgayBan.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            txtKhachHang.Text = "";
            lblMaKH.Text = "";
            _maKH = null;
            _gioHang.Clear();
            if (dgvItems != null) dgvItems.Rows.Clear();
            ClearProductPanel();
            UpdateTotal();
        }

        private void ClearProductPanel()
        {
            _spDangChon = null;
            txtProdInfo.Text = "";
            lblDonGiaVal.Text = "0 đ";
            lblSoLuongTonVal.Text = "0";
            txtSoLuong.Text = "";
            txtTimSP.Text = "";
        }

        private void TimSanPham()
        {
            string keyword = txtTimSP.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập Mã SP hoặc Tên SP cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var list = spBUS.LayDS();
            // Ưu tiên khớp chính xác Mã SP
            var match = list.FirstOrDefault(x => string.Equals(x.MaSP, keyword, StringComparison.OrdinalIgnoreCase));
            if (match == null)
            {
                var matches = list.Where(x => !string.IsNullOrEmpty(x.TenSP) && x.TenSP.ToLower().Contains(keyword.ToLower())).ToList();
                if (matches.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm khớp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (matches.Count > 1)
                {
                    MessageBox.Show($"Tìm thấy {matches.Count} sản phẩm khớp tên. Vui lòng nhập chính xác hơn hoặc dùng Mã SP.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                match = matches[0];
            }

            _spDangChon = match;
            txtProdInfo.Text = $"Mã: {match.MaSP}\r\nTên: {match.TenSP}\r\nĐVT: {match.DonViTinh}";
            lblDonGiaVal.Text = match.GiaBan.ToString("N0") + " đ";
            lblSoLuongTonVal.Text = match.SoLuongTon.ToString();
            txtSoLuong.Text = "1";
            txtSoLuong.Focus();
            txtSoLuong.SelectAll();
        }

        private void ThemVaoHoaDon()
        {
            if (_spDangChon == null)
            {
                MessageBox.Show("Vui lòng tìm sản phẩm trước khi thêm vào hóa đơn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtSoLuong.Text, out int sl) || sl <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existing = _gioHang.FirstOrDefault(x => x.MaSP == _spDangChon.MaSP);
            int slDaCo = existing != null ? existing.SoLuong : 0;

            if (slDaCo + sl > _spDangChon.SoLuongTon)
            {
                MessageBox.Show($"Số lượng vượt quá tồn kho.\nTồn: {_spDangChon.SoLuongTon}, đã có trong giỏ: {slDaCo}, thêm: {sl}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (existing != null)
            {
                existing.SoLuong += sl;
            }
            else
            {
                _gioHang.Add(new ChiTietHD
                {
                    MaSP = _spDangChon.MaSP,
                    SoLuong = sl,
                    DonGia = _spDangChon.GiaBan
                });
            }

            RebuildGrid();
            UpdateTotal();
            ClearProductPanel();
        }

        private void RebuildGrid()
        {
            dgvItems.Rows.Clear();
            int stt = 1;
            var dsSP = spBUS.LayDS();
            foreach (var ct in _gioHang)
            {
                var sp = dsSP.FirstOrDefault(x => x.MaSP == ct.MaSP);
                string tenSP = sp != null ? sp.TenSP : "";
                decimal thanhTien = ct.SoLuong * ct.DonGia;
                dgvItems.Rows.Add(stt++, ct.MaSP, tenSP, ct.DonGia.ToString("N0"), ct.SoLuong, thanhTien.ToString("N0"));
            }
        }

        private void UpdateTotal()
        {
            decimal total = _gioHang.Sum(x => x.SoLuong * x.DonGia);
            lblTongTien.Text = total.ToString("N0") + " đ";
        }

        private void XoaDongDaChon()
        {
            if (dgvItems.CurrentRow == null || dgvItems.CurrentRow.Index < 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int idx = dgvItems.CurrentRow.Index;
            if (idx < 0 || idx >= _gioHang.Count) return;
            _gioHang.RemoveAt(idx);
            RebuildGrid();
            UpdateTotal();
        }

        private void TimKhachHang()
        {
            string sdt = txtKhachHang.Text.Trim();
            if (string.IsNullOrEmpty(sdt))
            {
                _maKH = null;
                lblMaKH.Text = "";
                return;
            }
            try
            {
                var kh = khBUS.TimKiemNhanh(sdt);
                if (kh != null)
                {
                    _maKH = kh.MaKH;
                    lblMaKH.Text = $"({kh.MaKH} - {kh.TenKH})";
                    lblMaKH.ForeColor = Color.DarkBlue;
                }
                else
                {
                    _maKH = null;
                    lblMaKH.Text = "(Khách lẻ - SĐT không có trong hệ thống)";
                    lblMaKH.ForeColor = Color.DarkOrange;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("TimKhachHang error: " + ex.Message);
            }
        }

        private void ThanhToan()
        {
            if (_gioHang.Count == 0)
            {
                MessageBox.Show("Hóa đơn rỗng. Vui lòng thêm sản phẩm trước khi thanh toán.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtMaHD.Text))
            {
                MessageBox.Show("Mã hóa đơn không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal tongTien = _gioHang.Sum(x => x.SoLuong * x.DonGia);
            if (MessageBox.Show($"Xác nhận thanh toán?\n\nKhách: {(string.IsNullOrEmpty(_maKH) ? "Khách lẻ" : _maKH)}\nTổng tiền: {tongTien:N0} đ",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                // Không tự gán MaHD; DAL sẽ sinh trong transaction và set lại vào hd.MaHD
                var hd = new HoaDon
                {
                    NgayLap = DateTime.Now,
                    MaKH = _maKH ?? "",
                    TongTien = tongTien
                };

                if (hdBUS.XuatHoaDon(hd, _gioHang))
                {
                    MessageBox.Show($"Thanh toán thành công!\nMã HĐ: {hd.MaHD}\nTổng: {hd.TongTien:N0} đ",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    InitInvoice();
                }
                else
                {
                    MessageBox.Show("Thanh toán thất bại. Kiểm tra lại dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
