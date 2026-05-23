using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class frmThongKe : Form
    {
        private readonly HoaDonBUS hdBUS = new HoaDonBUS();
        private readonly SanPhamBUS spBUS = new SanPhamBUS();
        private readonly KhachHangBUS khBUS = new KhachHangBUS();

        private ComboBox cboPeriod, cboMonth, cboYear;
        private Label lblMonth;
        private Button btnThongKe;
        private PictureBox picBarChart, picPieChart;
        private Label lblPieLegend;
        private DataGridView dgvHoaDon;

        private static readonly Color[] PieColors = {
            Color.FromArgb(100, 149, 237), Color.FromArgb(220, 110, 110),
            Color.FromArgb(120, 200, 130), Color.FromArgb(230, 180, 80),
            Color.FromArgb(170, 130, 200)
        };

        public frmThongKe()
        {
            InitializeComponent();
            this.Load += (s, e) => { BuildLayout(); ChayThongKe(); };
        }

        private void BuildLayout()
        {
            this.SuspendLayout();
            this.Controls.Clear();
            this.Text = "Thống kê - Báo cáo";
            this.Size = new Size(1200, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Header
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(200, 240, 180), BorderStyle = BorderStyle.FixedSingle };
            Label lblTitle = new Label { Dock = DockStyle.Fill, Text = "THỐNG KÊ - BÁO CÁO", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 14, FontStyle.Bold) };
            pnlHeader.Controls.Add(lblTitle);

            // Filter
            Panel pnlFilter = new Panel { Dock = DockStyle.Top, Height = 55, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(10), BackColor = Color.White };

            Label lblPeriodLabel = new Label { Text = "Thống kê theo", Location = new Point(10, 17), AutoSize = true, Font = new Font("Arial", 9) };
            cboPeriod = new ComboBox { Location = new Point(110, 14), Size = new Size(80, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboPeriod.Items.AddRange(new object[] { "Tháng", "Năm" });
            cboPeriod.SelectedIndex = 0;

            lblMonth = new Label { Text = "Tháng", Location = new Point(210, 17), AutoSize = true, Font = new Font("Arial", 9) };
            cboMonth = new ComboBox { Location = new Point(260, 14), Size = new Size(60, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            for (int i = 1; i <= 12; i++) cboMonth.Items.Add(i);
            cboMonth.SelectedIndex = DateTime.Now.Month - 1;

            Label lblYearLabel = new Label { Text = "Năm", Location = new Point(330, 17), AutoSize = true, Font = new Font("Arial", 9) };
            cboYear = new ComboBox { Location = new Point(370, 14), Size = new Size(70, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            for (int y = DateTime.Now.Year - 4; y <= DateTime.Now.Year + 1; y++) cboYear.Items.Add(y);
            cboYear.SelectedItem = DateTime.Now.Year;
            if (cboYear.SelectedIndex < 0) cboYear.SelectedIndex = cboYear.Items.Count - 1;

            btnThongKe = new Button { Text = "Thống kê", Location = new Point(460, 13), Size = new Size(100, 28), BackColor = Color.FromArgb(100, 180, 255), ForeColor = Color.White, Font = new Font("Arial", 9, FontStyle.Bold), FlatStyle = FlatStyle.Flat };
            btnThongKe.FlatAppearance.BorderSize = 0;

            pnlFilter.Controls.AddRange(new Control[] { lblPeriodLabel, cboPeriod, lblMonth, cboMonth, lblYearLabel, cboYear, btnThongKe });

            // Content (3 columns: BarChart | PieChart | Invoice list)
            Panel pnlContent = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8), BackColor = Color.White };

            // Right - Invoice list
            Panel pnlRight = new Panel { Dock = DockStyle.Right, Width = 440, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(8) };
            Label lblGridTitle = new Label { Text = "Danh sách hóa đơn", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 8), AutoSize = true };
            dgvHoaDon = new DataGridView
            {
                Location = new Point(10, 32),
                Size = new Size(410, 510),
                AllowUserToAddRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                BackgroundColor = Color.White
            };
            dgvHoaDon.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 149, 237);
            dgvHoaDon.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHoaDon.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvHoaDon.Columns.Add("MaHD", "Mã HD");
            dgvHoaDon.Columns.Add("NgayBan", "Ngày bán");
            dgvHoaDon.Columns.Add("KhachHang", "Khách hàng");
            dgvHoaDon.Columns.Add("TongTien", "Tổng tiền");
            dgvHoaDon.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            pnlRight.Controls.Add(dgvHoaDon);
            pnlRight.Controls.Add(lblGridTitle);

            // Middle - Pie chart
            Panel pnlPie = new Panel { Dock = DockStyle.Right, Width = 340, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(8) };
            Label lblPieTitle = new Label { Text = "Sản phẩm bán chạy", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 8), AutoSize = true };
            picPieChart = new PictureBox
            {
                Location = new Point(10, 32),
                Size = new Size(310, 250),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            lblPieLegend = new Label
            {
                Location = new Point(10, 290),
                Size = new Size(310, 220),
                Font = new Font("Arial", 9),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            pnlPie.Controls.Add(lblPieLegend);
            pnlPie.Controls.Add(picPieChart);
            pnlPie.Controls.Add(lblPieTitle);

            // Left - Bar chart (fills remaining)
            Panel pnlBar = new Panel { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(8) };
            Label lblBarTitle = new Label { Text = "Doanh thu theo ngày", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 8), AutoSize = true };
            picBarChart = new PictureBox
            {
                Location = new Point(10, 32),
                Size = new Size(330, 510),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                SizeMode = PictureBoxSizeMode.Normal
            };
            pnlBar.Controls.Add(picBarChart);
            pnlBar.Controls.Add(lblBarTitle);

            pnlContent.Controls.Add(pnlBar);
            pnlContent.Controls.Add(pnlPie);
            pnlContent.Controls.Add(pnlRight);

            this.Controls.Add(pnlContent);
            this.Controls.Add(pnlFilter);
            this.Controls.Add(pnlHeader);

            // Events
            btnThongKe.Click += (s, e) => ChayThongKe();
            cboPeriod.SelectedIndexChanged += (s, e) =>
            {
                bool theoThang = cboPeriod.SelectedIndex == 0;
                lblMonth.Visible = theoThang;
                cboMonth.Visible = theoThang;
            };
            picBarChart.Resize += (s, e) => DrawBarChartCached();
            picPieChart.Resize += (s, e) => DrawPieChartCached();

            this.ResumeLayout();
        }

        // Dữ liệu kết quả thống kê gần nhất, để vẽ lại khi resize PictureBox
        private List<HoaDon> _hdInRange = new List<HoaDon>();
        private List<ChiTietHD> _ctInRange = new List<ChiTietHD>();
        private List<SanPham> _dsSP = new List<SanPham>();

        private void ChayThongKe()
        {
            try
            {
                DateTime from, to;
                GetDateRange(out from, out to);

                var allHD = hdBUS.LayDS();
                _hdInRange = allHD.Where(h => h.NgayLap >= from && h.NgayLap < to).ToList();

                var maHDSet = new HashSet<string>(_hdInRange.Select(h => h.MaHD));
                var allCT = hdBUS.LayTatCaChiTiet();
                _ctInRange = allCT.Where(c => maHDSet.Contains(c.MaHD)).ToList();

                _dsSP = spBUS.LayDS();
                var dsKH = khBUS.LayDS();

                UpdateGrid(_hdInRange, dsKH);
                DrawBarChartCached();
                DrawPieChartCached();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetDateRange(out DateTime from, out DateTime to)
        {
            int year = cboYear.SelectedItem != null ? Convert.ToInt32(cboYear.SelectedItem) : DateTime.Now.Year;
            if (cboPeriod.SelectedIndex == 0)
            {
                int month = cboMonth.SelectedItem != null ? Convert.ToInt32(cboMonth.SelectedItem) : DateTime.Now.Month;
                from = new DateTime(year, month, 1);
                to = from.AddMonths(1);
            }
            else
            {
                from = new DateTime(year, 1, 1);
                to = from.AddYears(1);
            }
        }

        private void UpdateGrid(List<HoaDon> hds, List<KhachHang> dsKH)
        {
            dgvHoaDon.Rows.Clear();
            foreach (var hd in hds.OrderByDescending(h => h.NgayLap))
            {
                var kh = string.IsNullOrEmpty(hd.MaKH) ? null : dsKH.FirstOrDefault(k => k.MaKH == hd.MaKH);
                string tenKH = kh != null ? kh.TenKH : "Khách lẻ";
                dgvHoaDon.Rows.Add(hd.MaHD, hd.NgayLap.ToString("dd/MM/yyyy"), tenKH, hd.TongTien.ToString("N0"));
            }
        }

        private void DrawBarChartCached() => DrawBarChart(_hdInRange);
        private void DrawPieChartCached() => DrawPieChart(_ctInRange, _dsSP);

        private void DrawBarChart(List<HoaDon> hds)
        {
            if (picBarChart.Width <= 10 || picBarChart.Height <= 10) return;

            var byDay = hds.GroupBy(h => h.NgayLap.Date)
                           .Select(g => new { Day = g.Key, Total = g.Sum(x => x.TongTien) })
                           .OrderBy(x => x.Day)
                           .ToList();

            var bmp = new Bitmap(picBarChart.Width, picBarChart.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                if (byDay.Count == 0)
                {
                    using (var f = new Font("Arial", 10, FontStyle.Italic))
                        g.DrawString("Không có dữ liệu trong kỳ", f, Brushes.Gray, 20, bmp.Height / 2 - 10);
                    SetPictureImage(picBarChart, bmp);
                    return;
                }

                decimal maxTotal = byDay.Max(x => x.Total);
                if (maxTotal == 0) maxTotal = 1;

                const int marginLeft = 55, marginRight = 12, marginTop = 12, marginBottom = 38;
                int chartW = bmp.Width - marginLeft - marginRight;
                int chartH = bmp.Height - marginTop - marginBottom;
                int gap = 12;
                int barCount = byDay.Count;
                int barW = Math.Max(10, (chartW - gap * (barCount + 1)) / barCount);

                // Y grid + labels
                using (var fontY = new Font("Arial", 7))
                using (var penGrid = new Pen(Color.FromArgb(230, 230, 230)))
                {
                    for (int i = 0; i <= 4; i++)
                    {
                        int y = marginTop + chartH - (chartH * i / 4);
                        decimal val = maxTotal * i / 4;
                        string lbl = val >= 1000000 ? (val / 1000000m).ToString("0.#") + "M" : (val / 1000m).ToString("0") + "k";
                        g.DrawString(lbl, fontY, Brushes.Gray, 5, y - 7);
                        g.DrawLine(penGrid, marginLeft, y, marginLeft + chartW, y);
                    }
                }

                // Axes
                using (var pen = new Pen(Color.Gray, 1))
                {
                    g.DrawLine(pen, marginLeft, marginTop, marginLeft, marginTop + chartH);
                    g.DrawLine(pen, marginLeft, marginTop + chartH, marginLeft + chartW, marginTop + chartH);
                }

                // Bars
                using (var brushBar = new SolidBrush(Color.FromArgb(100, 149, 237)))
                using (var fontX = new Font("Arial", 7))
                {
                    for (int i = 0; i < barCount; i++)
                    {
                        int barH = (int)Math.Round((double)(byDay[i].Total / maxTotal * chartH));
                        if (barH < 1) barH = 1;
                        int x = marginLeft + gap + i * (barW + gap);
                        int y = marginTop + chartH - barH;
                        g.FillRectangle(brushBar, x, y, barW, barH);

                        string lblDay = byDay[i].Day.ToString("dd/MM");
                        SizeF sz = g.MeasureString(lblDay, fontX);
                        g.DrawString(lblDay, fontX, Brushes.Black, x + (barW - sz.Width) / 2, marginTop + chartH + 5);

                        string lblVal = (byDay[i].Total / 1000m).ToString("0") + "k";
                        SizeF szv = g.MeasureString(lblVal, fontX);
                        g.DrawString(lblVal, fontX, Brushes.Black, x + (barW - szv.Width) / 2, y - 12);
                    }
                }
            }
            SetPictureImage(picBarChart, bmp);
        }

        private void DrawPieChart(List<ChiTietHD> ct, List<SanPham> dsSP)
        {
            if (picPieChart.Width <= 10 || picPieChart.Height <= 10) return;

            var top = ct.GroupBy(c => c.MaSP)
                        .Select(g => new { MaSP = g.Key, Qty = g.Sum(x => x.SoLuong) })
                        .OrderByDescending(x => x.Qty)
                        .Take(5)
                        .ToList();

            var bmp = new Bitmap(picPieChart.Width, picPieChart.Height);
            var legend = new System.Text.StringBuilder();

            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                if (top.Count == 0)
                {
                    using (var f = new Font("Arial", 10, FontStyle.Italic))
                        g.DrawString("Không có dữ liệu", f, Brushes.Gray, 20, bmp.Height / 2 - 10);
                    SetPictureImage(picPieChart, bmp);
                    lblPieLegend.Text = "(Chưa có hóa đơn nào trong kỳ)";
                    return;
                }

                int totalQty = top.Sum(x => x.Qty);
                float startAngle = 0;
                int size = Math.Min(bmp.Width, bmp.Height) - 20;
                int x0 = (bmp.Width - size) / 2;
                int y0 = (bmp.Height - size) / 2;

                for (int i = 0; i < top.Count; i++)
                {
                    float sweep = totalQty == 0 ? 0 : (float)top[i].Qty / totalQty * 360f;
                    using (var brush = new SolidBrush(PieColors[i % PieColors.Length]))
                        g.FillPie(brush, x0, y0, size, size, startAngle, sweep);
                    using (var penEdge = new Pen(Color.White, 2))
                        g.DrawPie(penEdge, x0, y0, size, size, startAngle, sweep);
                    startAngle += sweep;

                    var sp = dsSP.FirstOrDefault(s => s.MaSP == top[i].MaSP);
                    string tenSP = sp != null ? sp.TenSP : top[i].MaSP;
                    int pct = totalQty == 0 ? 0 : (int)Math.Round(top[i].Qty * 100.0 / totalQty);
                    legend.AppendLine($"● {tenSP} — {top[i].Qty} ({pct}%)");
                }
            }
            SetPictureImage(picPieChart, bmp);
            lblPieLegend.Text = legend.ToString();
        }

        private void SetPictureImage(PictureBox pic, Bitmap bmp)
        {
            var old = pic.Image;
            pic.Image = bmp;
            old?.Dispose();
        }
    }
}
