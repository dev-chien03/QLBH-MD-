using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace GUI.Forms
{
    public partial class frmThongKe : Form
    {
        private HoaDonBUS hdBUS = new HoaDonBUS();

        public frmThongKe()
        {
            InitializeComponent();
        }

        private void frmThongKe_Load(object sender, EventArgs e)
        {
            BuildLayout();
            LoadThongKe();
        }

        private void BuildLayout()
        {
            SuspendLayout();
            Controls.Clear();

            Text = "Thống kê - Báo cáo";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1200, 800);
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
                Text = "THỐNG KÊ - BÁO CÁO",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.Black
            };
            headerPanel.Controls.Add(title);

            // Filter Panel
            Panel filterPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(12)
            };

            Label lblThongKe = new Label { Text = "Thống kê theo", Left = 10, Top = 15, Width = 80 };
            ComboBox cboThongKe = new ComboBox
            {
                Left = 100,
                Top = 12,
                Width = 100,
                Height = 24,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboThongKe.Items.AddRange(new[] { "Tháng", "Năm" });
            cboThongKe.SelectedIndex = 0;

            Label lblThang = new Label { Text = "Tháng", Left = 220, Top = 15, Width = 40 };
            ComboBox cboThang = new ComboBox
            {
                Left = 270,
                Top = 12,
                Width = 80,
                Height = 24,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            for (int i = 1; i <= 12; i++)
                cboThang.Items.Add(i);
            cboThang.SelectedIndex = DateTime.Now.Month - 1;

            Label lblNam = new Label { Text = "/ Năm", Left = 360, Top = 15, Width = 50 };
            TextBox txtNam = new TextBox { Left = 420, Top = 12, Width = 60, Height = 22, Text = DateTime.Now.Year.ToString() };

            Button btnThongKe = new Button { Text = "Thống kê", Left = 500, Top = 12, Width = 80, Height = 24, BackColor = Color.LightBlue, Font = new Font("Segoe UI", 10F) };

            filterPanel.Controls.AddRange(new Control[] { lblThongKe, cboThongKe, lblThang, cboThang, lblNam, txtNam, btnThongKe });

            // Main Content - 3 sections
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(12)
            };

            // Left - Bar Chart
            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 350,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(12)
            };

            Label lblChart1 = new Label { Text = "Doanh thu theo ngày", Left = 10, Top = 10, Width = 200, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            PictureBox picChart1 = new PictureBox { Left = 10, Top = 40, Width = 300, Height = 250, BackColor = Color.LightGray, BorderStyle = BorderStyle.FixedSingle };

            leftPanel.Controls.AddRange(new Control[] { lblChart1, picChart1 });

            // Center - Pie Chart
            Panel centerPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 350,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(12),
                Margin = new Padding(0, 0, 12, 0)
            };

            Label lblChart2 = new Label { Text = "Sản phẩm bán chạy", Left = 10, Top = 10, Width = 200, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            PictureBox picChart2 = new PictureBox { Left = 10, Top = 40, Width = 300, Height = 250, BackColor = Color.LightGray, BorderStyle = BorderStyle.FixedSingle };

            Label lblProduct1 = new Label { Text = "□ Nem ngựa 250g", Left = 10, Top = 300, Width = 150, Font = new Font("Segoe UI", 9F) };
            Label lblProduct2 = new Label { Text = "□ Nem đông lạnh 250g", Left = 10, Top = 320, Width = 150, Font = new Font("Segoe UI", 9F) };

            centerPanel.Controls.AddRange(new Control[] { lblChart2, picChart2, lblProduct1, lblProduct2 });

            // Right - Invoice List
            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(12)
            };

            Label lblInvoiceTitle = new Label { Text = "Danh sách hóa đơn", Left = 10, Top = 10, Width = 200, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };

            DataGridView dgvHoaDon = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Top = 35
            };
            dgvHoaDon.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 149, 237);
            dgvHoaDon.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHoaDon.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            dgvHoaDon.Columns.Add("MaHD", "Mã HD");
            dgvHoaDon.Columns.Add("NgayLap", "Ngày bán");
            dgvHoaDon.Columns.Add("MaKH", "Khách hàng");
            dgvHoaDon.Columns.Add("TongTien", "Tổng tiền");

            rightPanel.Controls.Add(dgvHoaDon);
            rightPanel.Controls.Add(lblInvoiceTitle);

            mainPanel.Controls.Add(rightPanel);
            mainPanel.Controls.Add(centerPanel);
            mainPanel.Controls.Add(leftPanel);

            Controls.Add(mainPanel);
            Controls.Add(filterPanel);
            Controls.Add(headerPanel);

            // Events
            btnThongKe.Click += (s, e) => LoadThongKe();

            ResumeLayout();
        }

        private void LoadThongKe()
        {
            try
            {
                List<HoaDon> danhSach = hdBUS.LayDS();

                // Tính doanh thu theo ngày
                var doanhThuNgay = danhSach
                    .GroupBy(x => x.NgayLap.Date)
                    .Select(g => new { Ngay = g.Key, TongTien = g.Sum(x => x.TongTien) })
                    .OrderBy(x => x.Ngay)
                    .ToList();

                // Load dữ liệu vào datagrid nếu có
                MessageBox.Show("Thống kê được cập nhật!\nTổng hóa đơn: " + danhSach.Count, "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo");
            }
        }
    }
}