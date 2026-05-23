using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Forms
{
    public class frmLichSuNhap : Form
    {
        private readonly PhieuNhapBUS pnBUS = new PhieuNhapBUS();
        private readonly SanPhamBUS spBUS = new SanPhamBUS();

        private DataGridView dgvPN, dgvChiTiet;
        private List<PhieuNhap> _dsPN = new List<PhieuNhap>();
        private List<ChiTietPN> _dsCT = new List<ChiTietPN>();
        private List<SanPham> _dsSP = new List<SanPham>();

        public frmLichSuNhap()
        {
            BuildLayout();
            this.Load += (s, e) => LoadData();
        }

        private void BuildLayout()
        {
            this.Text = "Lịch sử nhập hàng";
            this.Size = new Size(950, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 45, BackColor = Color.FromArgb(200, 240, 180) };
            pnlHeader.Controls.Add(new Label { Dock = DockStyle.Fill, Text = "LỊCH SỬ NHẬP HÀNG", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 12, FontStyle.Bold) });

            Panel pnlMaster = new Panel { Dock = DockStyle.Top, Height = 240, Padding = new Padding(10), BorderStyle = BorderStyle.FixedSingle };
            pnlMaster.Controls.Add(new Label { Text = "Danh sách phiếu nhập (chọn 1 phiếu để xem chi tiết)", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 5), AutoSize = true });
            dgvPN = new DataGridView
            {
                Location = new Point(10, 30),
                Size = new Size(910, 195),
                AllowUserToAddRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            dgvPN.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 149, 237);
            dgvPN.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPN.Columns.Add("MaPN", "Mã PN");
            dgvPN.Columns.Add("NgayNhap", "Ngày nhập");
            dgvPN.Columns.Add("SoMatHang", "Số mặt hàng");
            dgvPN.Columns.Add("TongTien", "Tổng tiền");
            dgvPN.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            pnlMaster.Controls.Add(dgvPN);

            Panel pnlDetail = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            pnlDetail.Controls.Add(new Label { Text = "Chi tiết phiếu", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 5), AutoSize = true });
            dgvChiTiet = new DataGridView
            {
                Location = new Point(10, 30),
                Size = new Size(910, 200),
                AllowUserToAddRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            dgvChiTiet.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 149, 237);
            dgvChiTiet.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvChiTiet.Columns.Add("STT", "STT");
            dgvChiTiet.Columns.Add("MaSP", "Mã SP");
            dgvChiTiet.Columns.Add("TenSP", "Tên Sản Phẩm");
            dgvChiTiet.Columns.Add("SoLuong", "Số lượng");
            dgvChiTiet.Columns.Add("GiaNhap", "Giá nhập");
            dgvChiTiet.Columns.Add("ThanhTien", "Thành tiền");
            dgvChiTiet.Columns["GiaNhap"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            pnlDetail.Controls.Add(dgvChiTiet);

            this.Controls.Add(pnlDetail);
            this.Controls.Add(pnlMaster);
            this.Controls.Add(pnlHeader);

            dgvPN.SelectionChanged += (s, e) => LoadChiTiet();
        }

        private void LoadData()
        {
            try
            {
                _dsPN = pnBUS.LayDS().OrderByDescending(x => x.NgayNhap).ToList();
                _dsCT = pnBUS.LayTatCaChiTiet();
                _dsSP = spBUS.LayDS();

                dgvPN.Rows.Clear();
                foreach (var pn in _dsPN)
                {
                    int soMatHang = _dsCT.Count(c => c.MaPN == pn.MaPN);
                    dgvPN.Rows.Add(pn.MaPN, pn.NgayNhap.ToString("dd/MM/yyyy HH:mm"), soMatHang, pn.TongTienNhap.ToString("N0"));
                }
                if (dgvPN.Rows.Count > 0)
                {
                    dgvPN.ClearSelection();
                    dgvPN.Rows[0].Selected = true;
                }
                else
                {
                    dgvChiTiet.Rows.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiTiet()
        {
            dgvChiTiet.Rows.Clear();
            if (dgvPN.CurrentRow == null) return;
            string maPN = dgvPN.CurrentRow.Cells["MaPN"].Value?.ToString();
            if (string.IsNullOrEmpty(maPN)) return;

            int stt = 1;
            foreach (var ct in _dsCT.Where(x => x.MaPN == maPN))
            {
                var sp = _dsSP.FirstOrDefault(x => x.MaSP == ct.MaSP);
                string tenSP = sp != null ? sp.TenSP : "";
                decimal tt = ct.SoLuong * ct.GiaNhap;
                dgvChiTiet.Rows.Add(stt++, ct.MaSP, tenSP, ct.SoLuong, ct.GiaNhap.ToString("N0"), tt.ToString("N0"));
            }
        }
    }
}
