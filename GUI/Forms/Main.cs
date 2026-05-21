using BUS;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class Main : Form
    {
        private readonly SanPhamBUS spBUS = new SanPhamBUS();
        private readonly HoaDonBUS hdBUS = new HoaDonBUS();

        private Panel contentPanel;
        private Label lblRevenueValue;
        private Label lblInvoiceValue;

        public Main()
        {
            InitializeComponent();
            BuildHomeLayout();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshSummary();
        }

        private void btnQuanLySanPham_Click(object sender, EventArgs e)
        {
            OpenProductScreen();
        }

        private void btnBanHang_Click(object sender, EventArgs e)
        {
            OpenSalesScreen();
        }

        private void BuildHomeLayout()
        {
            SuspendLayout();
            Controls.Clear();

            BackColor = Color.White;
            ClientSize = new Size(980, 560);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PHẦN MỀM BÁN HÀNG NEM NGỰA";

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.FromArgb(223, 239, 212),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label title = new Label
            {
                Dock = DockStyle.Fill,
                Text = "PHẦN MỀM BÁN HÀNG NEM NGỰA",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14F, FontStyle.Regular),
                ForeColor = Color.Black
            };
            header.Controls.Add(title);

            Panel sideMenu = new Panel
            {
                Dock = DockStyle.Left,
                Width = 128,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8)
            };

            string[] menuItems = { "trang chủ", "Sản phẩm", "khách hàng", "Đơn hàng", "Kho hàng", "Thống kê" };
            for (int i = 0; i < menuItems.Length; i++)
            {
                Button button = new Button
                {
                    Width = 104,
                    Height = 33,
                    Left = 6,
                    Top = 8 + i * 41,
                    Text = menuItems[i],
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Standard,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Regular)
                };

                if (i == 1)
                    button.Click += OpenProductScreen;
                else if (i == 2)
                    button.Click += OpenCustomerScreen;
                else if (i == 3)
                    button.Click += OpenSalesScreen;
                else if (i == 4)
                    button.Click += OpenWarehouseScreen;
                else if (i == 5)
                    button.Click += ShowStatisticsMessage;

                sideMenu.Controls.Add(button);
            }

            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            lblRevenueValue = CreateStatText("......");
            lblInvoiceValue = CreateStatText("......");

            Panel revenueCard = CreateCard("Doanh thu hôm nay", lblRevenueValue, new Point(190, 92));
            Panel invoiceCard = CreateCard("Số hóa đơn", lblInvoiceValue, new Point(392, 92));

            contentPanel.Controls.Add(revenueCard);
            contentPanel.Controls.Add(invoiceCard);

            Controls.Add(contentPanel);
            Controls.Add(sideMenu);
            Controls.Add(header);

            ResumeLayout(true);
        }

        private Panel CreateCard(string caption, Label valueLabel, Point location)
        {
            Panel card = new Panel
            {
                Size = new Size(145, 88),
                Location = location,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label captionLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 56,
                Text = caption,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular)
            };

            valueLabel.Dock = DockStyle.Fill;
            valueLabel.TextAlign = ContentAlignment.TopCenter;
            valueLabel.Font = new Font("Segoe UI", 11F, FontStyle.Regular);

            card.Controls.Add(valueLabel);
            card.Controls.Add(captionLabel);
            return card;
        }

        private Label CreateStatText(string text)
        {
            return new Label
            {
                Text = text,
                ForeColor = Color.Black
            };
        }

        private void RefreshSummary()
        {
            try
            {
                var invoices = hdBUS.LayDS();
                decimal todayRevenue = invoices.Where(x => x.NgayLap.Date == DateTime.Today).Sum(x => x.TongTien);
                lblRevenueValue.Text = todayRevenue.ToString("N0");
                lblInvoiceValue.Text = invoices.Count.ToString();
            }
            catch
            {
                lblRevenueValue.Text = "......";
                lblInvoiceValue.Text = "......";
            }
        }

        private void OpenProductScreen(object sender = null, EventArgs e = null)
        {
            using (var form = new Forms.frmSanPham())
            {
                form.ShowDialog(this);
            }
        }

        private void OpenCustomerScreen(object sender = null, EventArgs e = null)
        {
            using (var form = new Forms.frmKhachHang())
            {
                form.ShowDialog(this);
            }
        }

        private void OpenSalesScreen(object sender = null, EventArgs e = null)
        {
            using (var form = new Forms.frmBanHang())
            {
                form.ShowDialog(this);
            }
        }

        private void OpenWarehouseScreen(object sender = null, EventArgs e = null)
        {
            using (var form = new Forms.frmKhoHang())
            {
                form.ShowDialog(this);
            }
        }

        private void ShowStatisticsMessage(object sender, EventArgs e)
        {
            using (var form = new Forms.frmThongKe())
            {
                form.ShowDialog(this);
            }
        }
    }
}
