# Test Guide - frmSanPham (Quản Lý Sản Phẩm)

## Hướng Dẫn Test Biểu Mẫu Sản Phẩm

### 1. Chạy Ứng Dụng
- Build project (Ctrl+Shift+B) ✅ Build successful
- Run (Ctrl+F5) để khởi động ứng dụng
- Bạn sẽ thấy Main form với 2 nút:
  - **Quản Lý Sản Phẩm** (nút test)
  - **Bán Hàng**

### 2. Truy Cập frmSanPham
- Bấm nút **"Quản Lý Sản Phẩm"** trên Main form
- Cửa sổ frmSanPham sẽ mở hiển thị:
  - Ô tìm kiếm (Search)
  - DataGridView chứa danh sách sản phẩm từ MySQL
  - Form nhập liệu: Mã SP, Tên SP, Giá Bán, Số Lượng Tồn, Đơn Vị Tính, Mã Loại
  - 3 Nút: Thêm, Sửa, Xóa

### 3. Test Các Tính Năng

#### 3.1 Tải Dữ Liệu (Load Data)
**Mục đích:** Xác nhận kết nối MySQL và hiển thị dữ liệu
- Khi form Load, danh sách sản phẩm từ DB sẽ tự động tải vào DataGridView
- Combo box Mã Loại sẽ hiển thị các danh mục sản phẩm
- ✅ **Thành công nếu:** DataGridView có dữ liệu, không có lỗi kết nối

#### 3.2 Tìm Kiếm (Search)
**Mục đích:** Lọc sản phẩm theo tên theo thời gian thực
- Gõ vào ô "Tìm kiếm" tên sản phẩm (vd: "Nước")
- Danh sách sẽ lọc ngay (real-time)
- ✅ **Thành công nếu:** Chỉ hiển thị sản phẩm chứa từ khóa

#### 3.3 Chọn Dòng Dữ Liệu (Select)
**Mục đích:** Đẩy dữ liệu lên form để sửa
- Bấm vào một dòng sản phẩm trong DataGridView
- Các ô nhập liệu sẽ tự động được điền với dữ liệu của sản phẩm đó
- Ô "Mã sản phẩm" sẽ bị khóa (Read-only) để tránh thay đổi khóa chính
- ✅ **Thành công nếu:** Dữ liệu hiển thị chính xác, Mã SP không thể chỉnh sửa

#### 3.4 Thêm Sản Phẩm (Insert)
**Mục đích:** Tạo sản phẩm mới
- Nhập dữ liệu vào các ô (bắt buộc: Mã SP, Tên SP)
- Chọn loại sản phẩm từ Combo box
- Nhập giá bán, số lượng, đơn vị tính
- Bấm nút **"Thêm"**
- ✅ **Thành công nếu:** 
  - Hiện thông báo "Thêm sản phẩm mới vào danh mục thành công!"
  - Sản phẩm mới xuất hiện trong danh sách
  - Form nhập liệu tự động làm mới (Clear)

#### 3.5 Sửa Sản Phẩm (Update)
**Mục đích:** Cập nhật thông tin sản phẩm
- Bấm vào sản phẩm muốn sửa
- Chỉnh sửa các thông tin (trừ Mã SP)
- Bấm nút **"Sửa"**
- ✅ **Thành công nếu:** 
  - Hiện thông báo "Cập nhật thông tin thay đổi của sản phẩm thành công!"
  - Dữ liệu trong DB được cập nhật
  - DataGridView làm mới

#### 3.6 Xóa Sản Phẩm (Delete)
**Mục đích:** Gỡ bỏ sản phẩm khỏi hệ thống
- Bấm vào sản phẩm muốn xóa
- Bấm nút **"Xóa"**
- Xác nhận hành động "Bạn có chắc chắn muốn xóa vĩnh viễn...?"
- Chọn **"Yes"**
- ✅ **Thành công nếu:** 
  - Hiện thông báo "Sản phẩm đã được gỡ bỏ hoàn toàn khỏi hệ thống dữ liệu."
  - Sản phẩm biến mất khỏi danh sách
- ⚠️ **Lỗi nếu:** Sản phẩm đã được sử dụng trong Hóa đơn/Phiếu nhập → Không thể xóa (ràng buộc FK)

#### 3.7 Kiểm Tra Nhập Liệu (Validation)
**Mục đích:** Xác nhận hệ thống không cho phép dữ liệu không hợp lệ
- Thử bấm "Thêm" hoặc "Sửa" mà không nhập Mã SP hoặc Tên SP
- ✅ **Thành công nếu:** Hiện thông báo "Vui lòng không để trống các ô dữ liệu bắt buộc..."

### 4. Test Tích Hợp với Bán Hàng
- Bấm **"Bán Hàng"** từ Main form
- Danh sách sản phẩm từ frmBanHang sẽ đúng với dữ liệu vừa thêm/sửa ở frmSanPham
- ✅ **Thành công nếu:** Dữ liệu đồng bộ giữa 2 form

### 5. Lỗi Phổ Biến & Khắc Phục

| Lỗi | Nguyên Nhân | Giải Pháp |
|-----|-----------|---------|
| DataGridView rỗng | Không kết nối MySQL | Kiểm tra connectionString trong dbConnection.cs |
| "Thêm mới thất bại" | Mã SP trùng hoặc Giá âm | Nhập Mã SP mới, Giá > 0 |
| Button không hoạt động | Event handler chưa connect | Đã fix trong Designer (btnThem_Click, btnSua_Click, v.v.) |
| Combo box không có dữ liệu | SP_LayDSLoaiSP lỗi | Kiểm tra stored procedure trong DB |

### 6. Liên Hệ & Báo Cáo
- Nếu gặp lỗi, check Output window (View → Output)
- Kiểm tra MySQL logs và Stored Procedures
- Đảm bảo DBQuanLyBanHang có các bảng: SanPham, LoaiSanPham, HoaDon, ChiTietHD

---

**Status:** ✅ Build successful - Ready to test
**Last Updated:** 2024
**Framework:** .NET 10 (GUI), .NET Framework 4.7.2 (BUS/DAL/DTO)
