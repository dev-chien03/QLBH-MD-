using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Forms
{
    public class frmNhapHang : Form
    {
        private readonly SanPhamBUS spBUS = new SanPhamBUS();
        private readonly PhieuNhapBUS pnBUS = new PhieuNhapBUS();

        private TextBox txtMaPN, txtNgayNhap, txtGiaNhap, txtSoLuong;
        private ComboBox cboSanPham;
        private Label lblTongTien, lblTonHienTai;
        private DataGridView dgvItems;
        private Button btnAdd, btnXoaItem, btnLuu, btnHuy;

        private readonly List<ChiTietPN> _gioNhap = new List<ChiTietPN>();
        private List<SanPham> _dsSP = new List<SanPham>();
        public bool DaLuu { get; private set; } = false;

        public frmNhapHang()
        {
            BuildLayout();
            this.Load += (s, e) => Init();
        }

        private void BuildLayout()
        {
            this.Text = "Nhập hàng vào kho";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 45, BackColor = Color.FromArgb(200, 240, 180) };
            pnlHeader.Controls.Add(new Label { Dock = DockStyle.Fill, Text = "NHẬP HÀNG VÀO KHO", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 12, FontStyle.Bold) });

            // Info row
            Panel pnlInfo = new Panel { Dock = DockStyle.Top, Height = 60, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(10) };
            pnlInfo.Controls.Add(new Label { Text = "Mã PN:", Location = new Point(10, 18), AutoSize = true });
            txtMaPN = new TextBox { Location = new Point(70, 15), Size = new Size(100, 22), ReadOnly = true };
            pnlInfo.Controls.Add(txtMaPN);
            pnlInfo.Controls.Add(new Label { Text = "Ngày nhập:", Location = new Point(190, 18), AutoSize = true });
            txtNgayNhap = new TextBox { Location = new Point(260, 15), Size = new Size(140, 22), ReadOnly = true };
            pnlInfo.Controls.Add(txtNgayNhap);

            // Form thêm sản phẩm vào phiếu
            Panel pnlInput = new Panel { Dock = DockStyle.Top, Height = 90, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(10), BackColor = Color.WhiteSmoke };
            pnlInput.Controls.Add(new Label { Text = "Sản phẩm:", Location = new Point(10, 12), AutoSize = true });
            cboSanPham = new ComboBox { Location = new Point(80, 10), Size = new Size(320, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            pnlInput.Controls.Add(cboSanPham);
            pnlInput.Controls.Add(new Label { Text = "Tồn hiện tại:", Location = new Point(420, 12), AutoSize = true });
            lblTonHienTai = new Label { Text = "0", Location = new Point(505, 12), AutoSize = true, Font = new Font("Arial", 9, FontStyle.Bold), ForeColor = Color.DarkBlue };
            pnlInput.Controls.Add(lblTonHienTai);

            pnlInput.Controls.Add(new Label { Text = "Số lượng:", Location = new Point(10, 47), AutoSize = true });
            txtSoLuong = new TextBox { Location = new Point(80, 45), Size = new Size(80, 22) };
            pnlInput.Controls.Add(txtSoLuong);
            pnlInput.Controls.Add(new Label { Text = "Giá nhập:", Location = new Point(180, 47), AutoSize = true });
            txtGiaNhap = new TextBox { Location = new Point(245, 45), Size = new Size(120, 22) };
            pnlInput.Controls.Add(txtGiaNhap);
            btnAdd = new Button { Text = "Thêm vào phiếu", Location = new Point(380, 43), Size = new Size(140, 27), BackColor = Color.FromArgb(100, 180, 255), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnAdd.FlatAppearance.BorderSize = 0;
            pnlInput.Controls.Add(btnAdd);

            // Bottom action bar - dùng sub-panel với Dock cho robust theo bề rộng form
            Panel pnlActions = new Panel { Dock = DockStyle.Bottom, Height = 70, BackColor = Color.WhiteSmoke, BorderStyle = BorderStyle.FixedSingle };

            // Right sub-panel (Tổng tiền + Lưu + Hủy) - luôn dính bên phải
            Panel pnlActionsRight = new Panel { Dock = DockStyle.Right, Width = 530 };
            Label lblTongTitle = new Label { Text = "Tổng tiền nhập:", Location = new Point(8, 25), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };
            pnlActionsRight.Controls.Add(lblTongTitle);
            lblTongTien = new Label { Text = "0 đ", Location = new Point(120, 22), Size = new Size(170, 25), Font = new Font("Arial", 12, FontStyle.Bold), ForeColor = Color.Red, TextAlign = ContentAlignment.MiddleRight };
            pnlActionsRight.Controls.Add(lblTongTien);
            btnLuu = new Button { Text = "Lưu phiếu nhập", Location = new Point(300, 19), Size = new Size(150, 32), BackColor = Color.FromArgb(100, 200, 100), ForeColor = Color.White, Font = new Font("Arial", 9, FontStyle.Bold), FlatStyle = FlatStyle.Flat };
            btnLuu.FlatAppearance.BorderSize = 0;
            pnlActionsRight.Controls.Add(btnLuu);
            btnHuy = new Button { Text = "Hủy", Location = new Point(460, 19), Size = new Size(60, 32), BackColor = Color.LightGray };
            pnlActionsRight.Controls.Add(btnHuy);

            // Left sub-panel (Xóa dòng) - luôn dính bên trái
            Panel pnlActionsLeft = new Panel { Dock = DockStyle.Left, Width = 180 };
            btnXoaItem = new Button { Text = "Xóa dòng đã chọn", Location = new Point(10, 19), Size = new Size(160, 32), BackColor = Color.LightPink };
            pnlActionsLeft.Controls.Add(btnXoaItem);

            pnlActions.Controls.Add(pnlActionsRight);
            pnlActions.Controls.Add(pnlActionsLeft);

            // Grid (Dock=Fill của form -> chiếm phần còn lại)
            Panel pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10), BackColor = Color.White };
            pnlGrid.Controls.Add(new Label { Text = "Chi tiết phiếu nhập", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 5), AutoSize = true });
            dgvItems = new DataGridView
            {
                Location = new Point(10, 30),
                Size = new Size(870, 220),
                AllowUserToAddRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            dgvItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 149, 237);
            dgvItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvItems.Columns.Add("STT", "STT");
            dgvItems.Columns.Add("MaSP", "Mã SP");
            dgvItems.Columns.Add("TenSP", "Tên Sản Phẩm");
            dgvItems.Columns.Add("SoLuong", "Số lượng");
            dgvItems.Columns.Add("GiaNhap", "Giá nhập");
            dgvItems.Columns.Add("ThanhTien", "Thành tiền");
            pnlGrid.Controls.Add(dgvItems);

            this.Controls.Add(pnlGrid);
            this.Controls.Add(pnlActions);
            this.Controls.Add(pnlInput);
            this.Controls.Add(pnlInfo);
            this.Controls.Add(pnlHeader);

            cboSanPham.SelectedIndexChanged += (s, e) => OnSelectSP();
            btnAdd.Click += (s, e) => ThemVaoPhieu();
            btnXoaItem.Click += (s, e) => XoaDong();
            btnLuu.Click += (s, e) => LuuPhieu();
            btnHuy.Click += (s, e) => this.Close();
            txtSoLuong.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; ThemVaoPhieu(); } };
        }

        private void Init()
        {
            try { txtMaPN.Text = pnBUS.TaoMaPNMoi(); } catch { txtMaPN.Text = "PN001"; }
            txtNgayNhap.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            _dsSP = spBUS.LayDS();
            cboSanPham.DisplayMember = "TenSP";
            cboSanPham.ValueMember = "MaSP";
            cboSanPham.DataSource = _dsSP;
            OnSelectSP();
        }

        private void OnSelectSP()
        {
            if (cboSanPham.SelectedItem is SanPham sp)
            {
                lblTonHienTai.Text = sp.SoLuongTon.ToString();
                txtGiaNhap.Text = (sp.GiaBan * 0.7m).ToString("0"); // Gợi ý giá nhập = 70% giá bán
            }
        }

        private void ThemVaoPhieu()
        {
            if (!(cboSanPham.SelectedItem is SanPham sp))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            if (!int.TryParse(txtSoLuong.Text, out int sl) || sl <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            if (!decimal.TryParse(txtGiaNhap.Text, out decimal gia) || gia < 0)
            {
                MessageBox.Show("Giá nhập phải là số và không âm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            var existing = _gioNhap.FirstOrDefault(x => x.MaSP == sp.MaSP);
            if (existing != null)
            {
                existing.SoLuong += sl;
                existing.GiaNhap = gia; // Cập nhật giá nhập mới (gộp dòng nhưng giá lấy theo lần thêm cuối)
            }
            else
            {
                _gioNhap.Add(new ChiTietPN { MaSP = sp.MaSP, SoLuong = sl, GiaNhap = gia });
            }
            RebuildGrid();
            UpdateTotal();

            txtSoLuong.Text = "";
            cboSanPham.Focus();
        }

        private void XoaDong()
        {
            if (dgvItems.CurrentRow == null || dgvItems.CurrentRow.Index < 0) return;
            int idx = dgvItems.CurrentRow.Index;
            if (idx < 0 || idx >= _gioNhap.Count) return;
            _gioNhap.RemoveAt(idx);
            RebuildGrid();
            UpdateTotal();
        }

        private void RebuildGrid()
        {
            dgvItems.Rows.Clear();
            int stt = 1;
            foreach (var ct in _gioNhap)
            {
                var sp = _dsSP.FirstOrDefault(x => x.MaSP == ct.MaSP);
                string tenSP = sp != null ? sp.TenSP : "";
                decimal tt = ct.SoLuong * ct.GiaNhap;
                dgvItems.Rows.Add(stt++, ct.MaSP, tenSP, ct.SoLuong, ct.GiaNhap.ToString("N0"), tt.ToString("N0"));
            }
        }

        private void UpdateTotal()
        {
            decimal total = _gioNhap.Sum(x => x.SoLuong * x.GiaNhap);
            lblTongTien.Text = total.ToString("N0") + " đ";
        }

        private void LuuPhieu()
        {
            if (_gioNhap.Count == 0)
            {
                MessageBox.Show("Phiếu nhập rỗng. Vui lòng thêm sản phẩm trước.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            decimal tong = _gioNhap.Sum(x => x.SoLuong * x.GiaNhap);
            if (MessageBox.Show($"Xác nhận lưu phiếu nhập?\n\nTổng tiền: {tong:N0} đ",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                // Không tự gán MaPN; DAL sinh trong transaction và set lại
                var pn = new PhieuNhap { NgayNhap = DateTime.Now, TongTienNhap = tong };
                if (pnBUS.NhapHangVaoKho(pn, _gioNhap))
                {
                    MessageBox.Show($"Lưu phiếu nhập thành công.\nMã PN: {pn.MaPN}\nTổng: {tong:N0} đ\n\nTồn kho đã được cập nhật tự động.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DaLuu = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lưu phiếu nhập thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
