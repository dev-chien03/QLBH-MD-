using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace GUI.Forms
{
    public partial class frmKhachHang : Form
    {
        private KhachHangBUS khBUS = new KhachHangBUS();
        private TextBox txtTimKiem, txtMaKH, txtTenKH, txtSDT, txtDiaChi;
        private DataGridView dgvKhachHang;
        private Button btnThem, btnSua, btnXoa, btnLuu;

        public frmKhachHang()
        {
            InitializeComponent();
            this.Load += frmKhachHang_Load;
        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {
            BuildLayout();
            LoadData();
        }

        private void BuildLayout()
        {
            this.SuspendLayout();
            this.Controls.Clear();
            this.Text = "Quản Lý Khách Hàng";
            this.Size = new Size(1200, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Header
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(200, 240, 180), BorderStyle = BorderStyle.FixedSingle };
            Label lblTitle = new Label { Dock = DockStyle.Fill, Text = "QUẢN LÝ KHÁCH HÀNG", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 14, FontStyle.Bold) };
            pnlHeader.Controls.Add(lblTitle);

            // Top Panel - Search & Buttons
            Panel pnlTop = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(10) };
            
            Label lblSearch = new Label { Text = "Tìm kiếm", Location = new Point(10, 12), AutoSize = true, Font = new Font("Arial", 9) };
            txtTimKiem = new TextBox { Location = new Point(60, 10), Size = new Size(150, 22), Font = new Font("Arial", 10) };
            Button btnTim = new Button { Text = "Tìm", Location = new Point(218, 10), Size = new Size(50, 22), BackColor = Color.FromArgb(100, 180, 255), Font = new Font("Arial", 9) };
            
            btnThem = new Button { Text = "Thêm", Location = new Point(280, 10), Size = new Size(50, 22), BackColor = Color.LightPink, Font = new Font("Arial", 9) };
            btnSua = new Button { Text = "Sửa", Location = new Point(340, 10), Size = new Size(50, 22), BackColor = Color.FromArgb(255, 200, 100), Font = new Font("Arial", 9) };
            btnXoa = new Button { Text = "Xóa", Location = new Point(400, 10), Size = new Size(50, 22), BackColor = Color.Plum, Font = new Font("Arial", 9) };
            
            pnlTop.Controls.AddRange(new Control[] { lblSearch, txtTimKiem, btnTim, btnThem, btnSua, btnXoa });

            // Content Panel - Left (Grid) & Right (Info)
            Panel pnlContent = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };

            // Left Panel - Grid
            Panel pnlLeft = new Panel { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(10) };
            Label lblGridTitle = new Label { Text = "Danh sách khách hàng", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 10), AutoSize = true };
            
            dgvKhachHang = new DataGridView 
            { 
                Location = new Point(10, 35), 
                Size = new Size(580, 450),
                AllowUserToAddRows = false, 
                ReadOnly = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            dgvKhachHang.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 149, 237);
            dgvKhachHang.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvKhachHang.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            pnlLeft.Controls.Add(dgvKhachHang);
            pnlLeft.Controls.Add(lblGridTitle);

            // Right Panel - Info
            Panel pnlRight = new Panel { Dock = DockStyle.Right, Width = 350, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(15) };
            Label lblInfo = new Label { Text = "Thông tin khách hàng", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(15, 10), AutoSize = true };
            
            Label lbl1 = new Label { Text = "Mã KH:", Location = new Point(15, 45), AutoSize = true };
            txtMaKH = new TextBox { Location = new Point(100, 42), Size = new Size(180, 22), ReadOnly = true };
            
            Label lbl2 = new Label { Text = "Họ Tên:", Location = new Point(15, 75), AutoSize = true };
            txtTenKH = new TextBox { Location = new Point(100, 72), Size = new Size(180, 22) };
            
            Label lbl3 = new Label { Text = "SĐT:", Location = new Point(15, 105), AutoSize = true };
            txtSDT = new TextBox { Location = new Point(100, 102), Size = new Size(180, 22) };
            
            Label lbl4 = new Label { Text = "Địa chỉ:", Location = new Point(15, 135), AutoSize = true };
            txtDiaChi = new TextBox { Location = new Point(100, 132), Size = new Size(180, 80), Multiline = true, ScrollBars = ScrollBars.Vertical };
            
            Label lbl5 = new Label { Text = "Ngày tham gia:", Location = new Point(15, 220), AutoSize = true };
            TextBox txtNgayThamGia = new TextBox { Location = new Point(100, 217), Size = new Size(180, 22), ReadOnly = true };
            
            btnLuu = new Button { Text = "Lưu", Location = new Point(100, 260), Size = new Size(180, 35), BackColor = Color.FromArgb(200, 230, 200), Font = new Font("Arial", 10, FontStyle.Bold) };
            
            pnlRight.Controls.AddRange(new Control[] { 
                lblInfo, lbl1, txtMaKH, lbl2, txtTenKH, lbl3, txtSDT, lbl4, txtDiaChi, lbl5, txtNgayThamGia, btnLuu 
            });

            pnlContent.Controls.Add(pnlRight);
            pnlContent.Controls.Add(pnlLeft);

            this.Controls.Add(pnlContent);
            this.Controls.Add(pnlTop);
            this.Controls.Add(pnlHeader);

            // Events
            btnTim.Click += (s, e) => LoadData();
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            dgvKhachHang.CellClick += DgvKhachHang_CellClick;
            txtTimKiem.TextChanged += (s, e) => SearchData();

            this.ResumeLayout();
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

        private void LoadData()
        {
            try
            {
                List<KhachHang> list = khBUS.LayDS();
                dgvKhachHang.DataSource = list;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void SearchData()
        {
            try
            {
                List<KhachHang> list = khBUS.LayDS();
                if (!string.IsNullOrEmpty(txtTimKiem.Text))
                    list = list.Where(x => x.TenKH.Contains(txtTimKiem.Text) || x.SoDienThoai.Contains(txtTimKiem.Text)).ToList();
                dgvKhachHang.DataSource = list;
            }
            catch { }
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                MessageBox.Show("Vui lòng nhập Họ Tên khách hàng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập Số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // txtMaKH ReadOnly nên người dùng không nhập được - tự sinh mã mới nếu rỗng
                string maKH = string.IsNullOrWhiteSpace(txtMaKH.Text) ? GenerateMaKH() : txtMaKH.Text.Trim();

                KhachHang kh = new KhachHang
                {
                    MaKH = maKH,
                    TenKH = txtTenKH.Text.Trim(),
                    SoDienThoai = txtSDT.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim()
                };

                if (khBUS.ThemKhachHang(kh))
                {
                    MessageBox.Show($"Thêm khách hàng thành công (Mã: {maKH})", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Thêm thất bại. Vui lòng kiểm tra dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKH.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa từ danh sách", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                MessageBox.Show("Vui lòng nhập Họ Tên khách hàng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập Số điện thoại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                KhachHang kh = new KhachHang
                {
                    MaKH = txtMaKH.Text.Trim(),
                    TenKH = txtTenKH.Text.Trim(),
                    SoDienThoai = txtSDT.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim()
                };

                if (khBUS.SuaKhachHang(kh))
                {
                    MessageBox.Show("Cập nhật khách hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại. Vui lòng kiểm tra dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKH.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa từ danh sách", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"Xóa khách hàng '{txtMaKH.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                if (khBUS.XoaKhachHang(txtMaKH.Text.Trim()))
                {
                    MessageBox.Show("Xóa khách hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại. Khách hàng có thể không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private string GenerateMaKH()
        {
            var list = khBUS.LayDS();
            int max = 0;
            foreach (var kh in list)
            {
                if (kh.MaKH != null && kh.MaKH.StartsWith("KH") &&
                    int.TryParse(kh.MaKH.Substring(2), out int n) && n > max)
                {
                    max = n;
                }
            }
            return "KH" + (max + 1).ToString("D3");
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKH.Text))
                BtnThem_Click(sender, e);
            else
                BtnSua_Click(sender, e);
        }

        private void ClearForm()
        {
            txtMaKH.Clear();
            txtTenKH.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();
        }
    }
}
