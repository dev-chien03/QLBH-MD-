using System;
using System.Windows.Forms;
using System.Drawing;

namespace main
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            this.Text = "Dashboard - Thống Kê Kho Hàng";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1200, 600);
            this.BackColor = Color.FromArgb(240, 240, 240);
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Create a panel for statistics
            Panel pnlStats = new Panel() { Dock = DockStyle.Top, Height = 150, BackColor = Color.White, Padding = new Padding(20) };

            // Stat boxes
            var spBUS = new BUS.SanPhamBUS();
            var khBUS = new BUS.KhachHangBUS();

            var products = spBUS.LayDS();
            var customers = khBUS.LayDS();

            // Product Count
            Panel pnlProduct = CreateStatBox("Tổng Sản Phẩm", products.Count.ToString(), Color.FromArgb(52, 152, 219));
            pnlProduct.Location = new Point(20, 20);
            pnlProduct.Size = new Size(250, 100);

            // Customer Count
            Panel pnlCustomer = CreateStatBox("Tổng Khách Hàng", customers.Count.ToString(), Color.FromArgb(46, 204, 113));
            pnlCustomer.Location = new Point(290, 20);
            pnlCustomer.Size = new Size(250, 100);

            // Total Inventory Value
            decimal totalValue = 0;
            foreach (var product in products)
            {
                totalValue += product.GiaBan * product.SoLuongTon;
            }
            Panel pnlValue = CreateStatBox("Giá Trị Kho", string.Format("{0:C}", totalValue), Color.FromArgb(155, 89, 182));
            pnlValue.Location = new Point(560, 20);
            pnlValue.Size = new Size(250, 100);

            // Low Stock Alert
            int lowStock = 0;
            foreach (var product in products)
            {
                if (product.SoLuongTon < 5)
                    lowStock++;
            }
            Panel pnlLowStock = CreateStatBox("Sản Phẩm Sắp Hết", lowStock.ToString(), Color.FromArgb(231, 76, 60));
            pnlLowStock.Location = new Point(830, 20);
            pnlLowStock.Size = new Size(250, 100);

            pnlStats.Controls.AddRange(new Control[] { pnlProduct, pnlCustomer, pnlValue, pnlLowStock });

            // Add data grid for low stock products
            Label lblLowStockProducts = new Label() { Text = "Sản Phẩm Sắp Hết Hàng:", Dock = DockStyle.Top, Height = 30, Padding = new Padding(20, 10, 0, 0) };
            DataGridView dgvLowStock = new DataGridView() { Dock = DockStyle.Fill, AllowUserToAddRows = false };
            dgvLowStock.Columns.Add("MaSP", "Mã SP");
            dgvLowStock.Columns.Add("TenSP", "Tên SP");
            dgvLowStock.Columns.Add("SoLuongTon", "Số Lượng");
            dgvLowStock.Columns.Add("GiaBan", "Giá Bán");

            foreach (var product in products)
            {
                if (product.SoLuongTon < 5)
                {
                    dgvLowStock.Rows.Add(product.MaSP, product.TenSP, product.SoLuongTon, product.GiaBan);
                }
            }

            this.Controls.Add(dgvLowStock);
            this.Controls.Add(lblLowStockProducts);
            this.Controls.Add(pnlStats);
        }

        private Panel CreateStatBox(string title, string value, Color bgColor)
        {
            Panel pnl = new Panel() { BackColor = bgColor, BorderStyle = BorderStyle.FixedSingle };

            Label lblTitle = new Label()
            {
                Text = title,
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblValue = new Label()
            {
                Text = value,
                ForeColor = Color.White,
                Font = new Font("Arial", 24, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnl.Controls.Add(lblValue);
            pnl.Controls.Add(lblTitle);

            return pnl;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
    }
}
