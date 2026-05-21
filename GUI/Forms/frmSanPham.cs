using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class frmSanPham : Form
    {
        // Khởi tạo các đối tượng xử lý nghiệp vụ trung gian
        private SanPhamBUS spBUS = new SanPhamBUS();
        private LoaiSPBUS loaiBUS = new LoaiSPBUS();

        public frmSanPham()
        {
            InitializeComponent();
            DinhDangGiaoDienLuoi();
        }

        // Tối ưu hiển thị bảng dữ liệu giống phong cách thiết kế hiện đại
        private void DinhDangGiaoDienLuoi()
        {
            dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSanPham.EnableHeadersVisualStyles = false;
            dgvSanPham.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvSanPham.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSanPham.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvSanPham.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        }

        // Sự kiện Form Load: Tự động tải dữ liệu lên các thành phần khi mở cửa sổ
        private void frmSanPham_Load(object sender, EventArgs e)
        {
            try
            {
                TaiDanhMucLoaiSP();
                TaiDanhSachSanPham();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Form load error: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Lỗi khi mở form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TaiDanhMucLoaiSP()
        {
            cboLoaiSP.DataSource = loaiBUS.LayDS();
            cboLoaiSP.DisplayMember = "TenLoai";
            cboLoaiSP.ValueMember = "MaLoai";
        }

        private void TaiDanhSachSanPham()
        {
            try
            {
                List<SanPham> data = spBUS.LayDS();

                if (data == null || data.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("WARNING: No products loaded from database!");
                    dgvSanPham.DataSource = new List<SanPham>();
                    return;
                }

                dgvSanPham.DataSource = data;

                if (dgvSanPham.Columns["MaSP"] != null) dgvSanPham.Columns["MaSP"].HeaderText = "Mã Sản Phẩm";
                if (dgvSanPham.Columns["TenSP"] != null) dgvSanPham.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                if (dgvSanPham.Columns["GiaBan"] != null) dgvSanPham.Columns["GiaBan"].HeaderText = "Giá Bán (VNĐ)";
                if (dgvSanPham.Columns["SoLuongTon"] != null) dgvSanPham.Columns["SoLuongTon"].HeaderText = "Tồn Kho";
                if (dgvSanPham.Columns["DonViTinh"] != null) dgvSanPham.Columns["DonViTinh"].HeaderText = "ĐVT";
                if (dgvSanPham.Columns["MaLoai"] != null) dgvSanPham.Columns["MaLoai"].HeaderText = "Mã Loại Danh Mục";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading products: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Lỗi tải sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

   

        // 1. CHỨC NĂNG TÌM KIẾM: Lọc dữ liệu ngay khi người dùng đang gõ phím (Real-time Search)
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();
            List<SanPham> fullList = spBUS.LayDS();

            // Lọc danh sách chứa từ khóa tìm kiếm theo tên sản phẩm
            List<SanPham> filteredList = fullList.FindAll(sp => sp.TenSP.ToLower().Contains(tuKhoa));
            dgvSanPham.DataSource = filteredList;
        }

        // 2. SỰ KIỆN CLICK LƯỚI DỮ LIỆU: Đẩy dữ liệu ngược lên vùng nhập để chỉnh sửa
        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];

                txtMaSP.Text = row.Cells["MaSP"].Value.ToString();
                txtTenSP.Text = row.Cells["TenSP"].Value.ToString();
                txtGiaBan.Text = Convert.ToDecimal(row.Cells["GiaBan"].Value).ToString("G0");
                txtSoLuong.Text = row.Cells["SoLuongTon"].Value.ToString();
                txtDVT.Text = row.Cells["DonViTinh"].Value.ToString();
                cboLoaiSP.SelectedValue = row.Cells["MaLoai"].Value.ToString();

                // Khóa trường Mã sản phẩm không cho phép chỉnh sửa nhằm đảm bảo tính toàn vẹn dữ liệu
                txtMaSP.Enabled = false;
            }
        }

        // 3. CHỨC NĂNG THÊM MỚI
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!KiemTraDuLieuDauVao()) return;

            // Đóng gói thông tin thu thập được vào thực thể DTO
            SanPham sp = ThuThapThongTinSanPhan();

            if (spBUS.LuuSanPham(sp))
            {
                MessageBox.Show("Thêm sản phẩm mới vào danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachSanPham();
                LamMoiVungNhap();
            }
            else
            {
                MessageBox.Show("Thêm mới thất bại. Vui lòng kiểm tra lại mã sản phẩm hoặc giá bán hợp lệ!", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 4. CHỨC NĂNG CẬP NHẬT (SỬA)
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaSP.Enabled == true)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm cụ thể từ danh sách hiển thị bên dưới trước khi sửa!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!KiemTraDuLieuDauVao()) return;

            SanPham sp = ThuThapThongTinSanPhan();

            if (spBUS.SuaSanPham(sp))
            {
                MessageBox.Show("Cập nhật thông tin thay đổi của sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachSanPham();
                LamMoiVungNhap();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại. Vui lòng kiểm tra lại cấu trúc dữ liệu dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 5. CHỨC NĂNG XÓA BỎ
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text))
            {
                MessageBox.Show("Vui lòng bấm chọn sản phẩm muốn xóa khỏi hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult xacNhan = MessageBox.Show($"Bạn có chắc chắn muốn xóa vĩnh viễn mã sản phẩm [{txtMaSP.Text}] này không?", "Xác nhận hành động", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (xacNhan == DialogResult.Yes)
            {
                if (spBUS.XoaSanPham(txtMaSP.Text.Trim()))
                {
                    MessageBox.Show("Sản phẩm đã được gỡ bỏ hoàn toàn khỏi hệ thống dữ liệu.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachSanPham();
                    LamMoiVungNhap();
                }
                else
                {
                    MessageBox.Show("Không thể xóa sản phẩm này! Hàng hóa này đã tồn tại trong lịch sử Hóa đơn hoặc Phiếu nhập kho dữ liệu cũ.", "Cảnh báo bảo mật dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        // Các phương thức bổ trợ tối ưu hóa luồng hiển thị code
        private bool KiemTraDuLieuDauVao()
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text) || string.IsNullOrWhiteSpace(txtTenSP.Text))
            {
                MessageBox.Show("Vui lòng không để trống các ô dữ liệu bắt buộc (Mã sản phẩm và Tên sản phẩm)!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private SanPham ThuThapThongTinSanPhan()
        {
            return new SanPham
            {
                MaSP = txtMaSP.Text.Trim(),
                TenSP = txtTenSP.Text.Trim(),
                GiaBan = string.IsNullOrEmpty(txtGiaBan.Text) ? 0 : Convert.ToDecimal(txtGiaBan.Text),
                SoLuongTon = string.IsNullOrEmpty(txtSoLuong.Text) ? 0 : Convert.ToInt32(txtSoLuong.Text),
                DonViTinh = txtDVT.Text.Trim(),
                MaLoai = cboLoaiSP.SelectedValue?.ToString() ?? string.Empty
            };
        }

        private void LamMoiVungNhap()
        {
            txtMaSP.Text = "";
            txtTenSP.Text = "";
            txtGiaBan.Text = "0";
            txtSoLuong.Text = "0";
            txtDVT.Text = "";
            if (cboLoaiSP.Items.Count > 0) cboLoaiSP.SelectedIndex = 0;

            // Giải phóng trạng thái khóa để tiếp tục nhập mới sản phẩm khác
            txtMaSP.Enabled = true;
        }
    }
}
