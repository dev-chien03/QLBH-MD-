using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class frmKhoHang : Form
    {
        private const int NGUONG_CANH_BAO = 20; // Tồn <= ngưỡng → cảnh báo "Sắp hết"

        private readonly SanPhamBUS spBUS = new SanPhamBUS();
        private List<SanPham> _dsSP = new List<SanPham>();

        private TextBox txtTimKiem;
        private Button btnTim, btnNhap, btnLichSu, btnTaiLai;
        private DataGridView dgvKho;
        private Label lblTongSP, lblTongTon, lblSoSPSapHet;

        public frmKhoHang()
        {
            InitializeComponent();
            this.Load += (s, e) => { BuildLayout(); LoadData(); };
        }

        private void BuildLayout()
        {
            this.SuspendLayout();
            this.Controls.Clear();
            this.Text = "Quản Lý Kho Hàng";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Header
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(200, 240, 180), BorderStyle = BorderStyle.FixedSingle };
            pnlHeader.Controls.Add(new Label { Dock = DockStyle.Fill, Text = "QUẢN LÝ KHO HÀNG", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 13, FontStyle.Bold) });

            // Search + actions
            Panel pnlSearch = new Panel { Dock = DockStyle.Top, Height = 60, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(10), BackColor = Color.White };
            pnlSearch.Controls.Add(new Label { Text = "Tìm kiếm:", Location = new Point(10, 15), AutoSize = true });
            txtTimKiem = new TextBox { Location = new Point(80, 12), Size = new Size(280, 25) };
            pnlSearch.Controls.Add(txtTimKiem);
            btnTim = new Button { Text = "Tìm", Location = new Point(370, 12), Size = new Size(60, 25), BackColor = Color.LightBlue };
            pnlSearch.Controls.Add(btnTim);
            btnTaiLai = new Button { Text = "Tải lại", Location = new Point(440, 12), Size = new Size(70, 25), BackColor = Color.LightGray };
            pnlSearch.Controls.Add(btnTaiLai);
            btnNhap = new Button { Text = "Nhập hàng", Location = new Point(520, 12), Size = new Size(100, 25), BackColor = Color.FromArgb(100, 200, 100), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnNhap.FlatAppearance.BorderSize = 0;
            pnlSearch.Controls.Add(btnNhap);
            btnLichSu = new Button { Text = "Lịch sử nhập", Location = new Point(630, 12), Size = new Size(110, 25), BackColor = Color.FromArgb(220, 150, 200), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnLichSu.FlatAppearance.BorderSize = 0;
            pnlSearch.Controls.Add(btnLichSu);

            // Footer (thống kê tóm tắt)
            Panel pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 40, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.WhiteSmoke, Padding = new Padding(10) };
            lblTongSP = new Label { Text = "Tổng SP: 0", Location = new Point(10, 10), AutoSize = true, Font = new Font("Arial", 9, FontStyle.Bold) };
            lblTongTon = new Label { Text = "Tổng tồn: 0", Location = new Point(150, 10), AutoSize = true, Font = new Font("Arial", 9, FontStyle.Bold) };
            lblSoSPSapHet = new Label { Text = $"SP sắp hết (≤ {NGUONG_CANH_BAO}): 0", Location = new Point(310, 10), AutoSize = true, Font = new Font("Arial", 9, FontStyle.Bold), ForeColor = Color.DarkRed };
            pnlFooter.Controls.AddRange(new Control[] { lblTongSP, lblTongTon, lblSoSPSapHet });

            // Grid
            dgvKho = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            dgvKho.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 149, 237);
            dgvKho.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvKho.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvKho.Columns.Add("STT", "STT");
            dgvKho.Columns.Add("MaSP", "Mã SP");
            dgvKho.Columns.Add("TenSP", "Tên Sản Phẩm");
            dgvKho.Columns.Add("DonVi", "Đơn vị");
            dgvKho.Columns.Add("GiaBan", "Giá bán");
            dgvKho.Columns.Add("SoLuongTon", "Số lượng tồn");
            dgvKho.Columns.Add("TrangThai", "Trạng thái");
            dgvKho.Columns["GiaBan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvKho.Columns["SoLuongTon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Body container so footer + grid + search stack correctly
            Panel pnlBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10, 0, 10, 0) };
            pnlBody.Controls.Add(dgvKho);

            this.Controls.Add(pnlBody);
            this.Controls.Add(pnlFooter);
            this.Controls.Add(pnlSearch);
            this.Controls.Add(pnlHeader);

            // Events
            btnTim.Click += (s, e) => ApplyFilter();
            btnTaiLai.Click += (s, e) => { txtTimKiem.Clear(); LoadData(); };
            txtTimKiem.TextChanged += (s, e) => ApplyFilter();
            txtTimKiem.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; ApplyFilter(); } };
            btnNhap.Click += (s, e) => MoNhapHang();
            btnLichSu.Click += (s, e) => MoLichSu();

            this.ResumeLayout();
        }

        private void LoadData()
        {
            try
            {
                _dsSP = spBUS.LayDS() ?? new List<SanPham>();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter()
        {
            string kw = (txtTimKiem.Text ?? "").Trim().ToLower();
            IEnumerable<SanPham> filtered = _dsSP;
            if (!string.IsNullOrEmpty(kw))
            {
                filtered = _dsSP.Where(x =>
                    (!string.IsNullOrEmpty(x.MaSP) && x.MaSP.ToLower().Contains(kw)) ||
                    (!string.IsNullOrEmpty(x.TenSP) && x.TenSP.ToLower().Contains(kw)));
            }
            BindData(filtered.ToList());
        }

        private void BindData(List<SanPham> data)
        {
            dgvKho.Rows.Clear();
            int stt = 1;
            foreach (var item in data)
            {
                string trangThai;
                Color foreColor;
                if (item.SoLuongTon <= 0) { trangThai = "Hết hàng"; foreColor = Color.Red; }
                else if (item.SoLuongTon <= NGUONG_CANH_BAO) { trangThai = "Sắp hết"; foreColor = Color.DarkOrange; }
                else { trangThai = "Bình thường"; foreColor = Color.DarkGreen; }

                int rowIdx = dgvKho.Rows.Add(stt++, item.MaSP, item.TenSP, item.DonViTinh, item.GiaBan.ToString("N0"), item.SoLuongTon, trangThai);
                dgvKho.Rows[rowIdx].Cells["TrangThai"].Style.ForeColor = foreColor;
                dgvKho.Rows[rowIdx].Cells["TrangThai"].Style.Font = new Font("Arial", 9, FontStyle.Bold);
            }

            // Cập nhật footer dùng toàn bộ _dsSP (không phụ thuộc filter)
            lblTongSP.Text = "Tổng SP: " + _dsSP.Count;
            lblTongTon.Text = "Tổng tồn: " + _dsSP.Sum(x => x.SoLuongTon).ToString("N0");
            lblSoSPSapHet.Text = $"SP sắp hết (≤ {NGUONG_CANH_BAO}): " + _dsSP.Count(x => x.SoLuongTon <= NGUONG_CANH_BAO);
        }

        private void MoNhapHang()
        {
            using (var f = new frmNhapHang())
            {
                f.ShowDialog(this);
                if (f.DaLuu) LoadData(); // Refresh tồn kho sau khi nhập
            }
        }

        private void MoLichSu()
        {
            using (var f = new frmLichSuNhap())
            {
                f.ShowDialog(this);
            }
        }
    }
}
