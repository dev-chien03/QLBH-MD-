# Quick Start Guide - Warehouse Management GUI

## Installation & Setup

### Prerequisites
- .NET Framework 4.7.2
- Visual Studio 2019 or later
- MySQL Database
- MySql.Data NuGet package

### Project Structure
```
Solution/
├── DTO/                    # Data Transfer Objects
│   └── Entities.cs        # Entity definitions
├── DAL/                   # Data Access Layer
│   ├── Implementations/   # Database operations
│   └── Interfaces/        # Contracts
├── BUS/                   # Business Logic Layer
│   ├── SanPhamBUS.cs      # Product management
│   ├── KhachHangBUS.cs    # Customer management
│   ├── HoaDonBUS.cs       # Invoice management
│   └── PhieuNhapBUS.cs    # Import receipt management
└── main/                  # GUI (Windows Forms)
	├── main.cs            # Main form with tabs
	├── Program.cs         # Entry point
	├── Dashboard.cs       # Statistics dashboard
	└── UIHelpers.cs       # UI utilities
```

## Running the Application

1. **Build the Solution**
   - Open the solution in Visual Studio
   - Press `Ctrl + Shift + B` to build
   - Ensure all projects build without errors

2. **Set Startup Project**
   - Right-click on the `main` project
   - Select "Set as Startup Project"

3. **Run the Application**
   - Press `F5` or click the "Run" button
   - The main form will open showing the warehouse management interface

## Main Features

### 1. Product Management Tab
```
How to Add a Product:
1. Go to "Quản Lý Sản Phẩm" tab
2. Enter Product Code (Mã SP)
3. Enter Product Name (Tên SP)
4. Enter Sale Price (Giá Bán)
5. Enter Quantity (Số Lượng)
6. Click "Thêm" (Add) button
7. Click "Tải Lại" (Reload) to refresh the list
```

### 2. Customer Management Tab
```
How to Add a Customer:
1. Go to "Quản Lý Khách Hàng" tab
2. Enter Customer Code (Mã KH)
3. Enter Customer Name (Tên KH)
4. Enter Phone Number (SĐT)
5. Enter Address (Địa Chỉ)
6. Click "Thêm" (Add) button
7. To update, select a customer and click "Sửa" (Edit)
```

### 3. Invoice Management Tab
```
How to Create an Invoice:
1. Go to "Quản Lý Hóa Đơn" tab
2. Enter Invoice Code (Mã HĐ)
3. Enter Customer Code (Mã KH)
4. Enter Date (Ngày Lập)
5. Enter Total Amount (Tổng Tiền)
6. Click "Thêm" (Add) button
```

### 4. Import Receipt Management Tab
```
How to Create an Import Receipt:
1. Go to "Quản Lý Phiếu Nhập" tab
2. Enter Receipt Code (Mã PN)
3. Enter Date (Ngày Nhập)
4. Enter Total Cost (Tổng Tiền)
5. Click "Thêm" (Add) button
```

## Database Connection

The application connects to MySQL using stored procedures. Ensure your database has these procedures:

```sql
-- Example stored procedure structure
DELIMITER //
CREATE PROCEDURE sp_LayDSSanPham()
BEGIN
  SELECT * FROM SanPham;
END //
DELIMITER ;
```

## Common Operations

### View All Records
- Go to any tab
- All records are loaded automatically on startup
- Click "Tải Lại" to refresh

### Select a Record
- Click on any row in the data grid
- The record details will populate in the input fields above

### Update a Record
1. Click on the record in the grid
2. Modify the fields
3. Click "Sửa" (Edit) if available, or delete and re-add

### Delete a Record
1. Click on the record in the grid
2. Click "Xóa" (Delete) button
3. Confirm the deletion

### Clear All Fields
- After successful operations, fields clear automatically
- Or manually clear to enter new data

## Troubleshooting

### Issue: "Database connection failed"
- Check MySQL server is running
- Verify connection string in `dbConnection.cs`
- Ensure stored procedures exist in database

### Issue: "Column not found"
- Verify database table structure matches DTO entities
- Ensure stored procedure SELECT statements match entity properties

### Issue: "Data not loading"
- Click "Tải Lại" (Reload) button
- Check database has data for that entity
- Verify stored procedure exists and works

### Issue: "Add/Edit/Delete not working"
- Check for validation errors in message boxes
- Ensure required fields are filled
- Verify business logic validation in BUS layer

## Tips & Best Practices

1. **Always fill required fields** before saving
2. **Click Reload** after operations to see updated data
3. **Use meaningful names** for product and customer codes
4. **Regular backups** of your database
5. **Test stored procedures** directly in MySQL Workbench first
6. **Monitor low stock** using the Dashboard form (if implemented)
7. **Keep phone numbers** in consistent format

## Advanced Features (Future)

- Search and filter functionality
- Export data to Excel
- Print reports and invoices
- User authentication
- Role-based access control
- Transaction history
- Real-time notifications

## Support Files

- `GUI_DOCUMENTATION.md` - Detailed documentation
- `UIHelpers.cs` - Reusable UI components
- `Dashboard.cs` - Statistics and overview

## Contact & Support

For issues or questions about the GUI:
1. Check the documentation files
2. Review the UIHelpers for available utilities
3. Check the BUS layer logic
4. Verify database stored procedures
