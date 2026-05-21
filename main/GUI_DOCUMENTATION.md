# Warehouse Management System - GUI Documentation

## Overview
Your warehouse management application now includes a complete GUI built with Windows Forms. The application is organized into a tabbed interface with full CRUD (Create, Read, Update, Delete) operations for all entities.

## Structure

### Main Form (`main.cs`)
The main form serves as the central hub of the application with 4 tabs:

1. **Quản Lý Sản Phẩm (Product Management)**
   - Add new products
   - View all products in a data grid
   - Update product information
   - Delete products
   - **Fields**: Mã SP (Product Code), Tên SP (Product Name), Giá Bán (Sale Price), Số Lượng (Quantity)

2. **Quản Lý Khách Hàng (Customer Management)**
   - Add new customers
   - View all customers
   - Update customer information
   - Delete customers
   - **Fields**: Mã KH (Customer Code), Tên KH (Customer Name), SĐT (Phone), Địa Chỉ (Address)

3. **Quản Lý Hóa Đơn (Invoice Management)**
   - Create new invoices
   - View all invoices
   - **Fields**: Mã HĐ (Invoice Code), Mã KH (Customer Code), Ngày Lập (Date), Tổng Tiền (Total Amount)

4. **Quản Lý Phiếu Nhập (Import Receipt Management)**
   - Create import receipts
   - View all import receipts
   - **Fields**: Mã PN (Receipt Code), Ngày Nhập (Date), Tổng Tiền Nhập (Total Cost)

### Dashboard Form (`Dashboard.cs`)
A statistics and overview form showing:
- Total products count
- Total customers count
- Total inventory value (Price × Quantity)
- Low stock alert count
- Detailed view of low-stock products (quantity < 5)

## Features

### User Interface Elements
Each tab includes:
- **Input Fields**: For entering/editing data
- **Data Grid View**: Shows all records in a table format
- **Action Buttons**:
  - **Thêm (Add)**: Create new records
  - **Sửa (Edit)**: Update existing records (for customers)
  - **Xóa (Delete)**: Remove records
  - **Tải Lại (Reload)**: Refresh the data grid

### Functionality
1. **Click-to-Select**: Click any row in the data grid to populate input fields
2. **Form Validation**: Required fields are checked before saving
3. **Auto-Refresh**: Data grids refresh automatically after operations
4. **Clear Fields**: Input fields clear after successful operations

## Integration with Business Logic

### Layer Architecture
```
GUI (Windows Forms) 
	↓
BUS (Business Layer)
	↓
DAL (Data Access Layer)
	↓
Database (SQL)
```

### Methods Used
- **SanPhamBUS**: LayDS(), LuuSanPham(), XoaSanPham()
- **KhachHangBUS**: LayDS(), ThemKhachHang(), SuaKhachHang()
- **HoaDonBUS**: LayDS(), LuuHoaDon(), XuatHoaDon()
- **PhieuNhapBUS**: LayDS(), LuuPhieuNhap(), NhapHangVaoKho()

## How to Use

### Adding a Product
1. Go to "Quản Lý Sản Phẩm" tab
2. Fill in: Mã SP, Tên SP, Giá Bán, Số Lượng
3. Click "Thêm" button
4. Refresh the grid with "Tải Lại"

### Updating a Product
1. Click on a product row to populate fields
2. Modify the information
3. Use the edit functionality or delete and re-add

### Viewing Customers
1. Go to "Quản Lý Khách Hàng" tab
2. All customers are displayed automatically
3. Click a customer to view details

### Managing Invoices
1. Go to "Quản Lý Hóa Đơn" tab
2. Click "Thêm" to create a new invoice
3. Invoices are linked to customers via Mã KH

### Importing Stock
1. Go to "Quản Lý Phiếu Nhập" tab
2. Create import receipts with product details
3. System updates inventory automatically

## Technical Details

### Database Stored Procedures Used
- `sp_LayDSSanPham` - Get all products
- `sp_ThemSanPham` - Add new product
- `sp_SuaSanPham` - Update product
- `sp_XoaSanPham` - Delete product
- `sp_LayDSKhachHang` - Get all customers
- `sp_LayDSHoaDon` - Get all invoices
- `sp_LayDSPhieuNhap` - Get all import receipts

### Error Handling
- Invalid negative prices are rejected
- Empty required fields show warning messages
- Delete operations ask for confirmation
- Failed operations show error messages

## Future Enhancements
1. Add search/filter functionality
2. Implement advanced reporting
3. Add data export to Excel
4. Implement user authentication
5. Add detailed transaction history
6. Multi-user support with role-based access
7. Print invoices and receipts
8. Real-time inventory alerts

## Notes
- All monetary values are in VND (Vietnamese Dong)
- All dates follow yyyy-MM-dd format
- Product codes are auto-generated in some cases
- Inventory is automatically updated during import/sales operations
