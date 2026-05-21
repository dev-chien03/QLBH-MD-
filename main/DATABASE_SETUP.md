# Database Configuration Guide

## Connection String Setup

Your database connection is configured in the `DAL/Helper/dbConnection.cs` file.

### MySQL Connection String Format

```xml
<!-- In App.config -->
<connectionStrings>
  <add name="DefaultConnection" 
	   connectionString="server=localhost;userid=root;password=yourpassword;database=WarehouseDB" 
	   providerName="MySql.Data.MySqlClient" />
</connectionStrings>
```

### Parameters Explanation

| Parameter | Description | Example |
|-----------|-------------|---------|
| server | MySQL server hostname or IP | localhost, 127.0.0.1, or remote IP |
| userid | MySQL user account | root, admin, warehouse_user |
| password | MySQL user password | your_password_here |
| database | Database name | WarehouseDB, kho_hang |
| port | MySQL port (optional) | 3306 (default) |

## Database Creation Script

```sql
-- Create Database
CREATE DATABASE WarehouseDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE WarehouseDB;

-- Create LoaiSanPham Table
CREATE TABLE LoaiSanPham (
  MaLoai VARCHAR(10) PRIMARY KEY,
  TenLoai VARCHAR(100) NOT NULL
);

-- Create SanPham Table
CREATE TABLE SanPham (
  MaSP VARCHAR(10) PRIMARY KEY,
  TenSP VARCHAR(100) NOT NULL,
  GiaBan DECIMAL(10,2) NOT NULL,
  SoLuongTon INT DEFAULT 0,
  DonViTinh VARCHAR(20),
  MaLoai VARCHAR(10),
  FOREIGN KEY (MaLoai) REFERENCES LoaiSanPham(MaLoai)
);

-- Create KhachHang Table
CREATE TABLE KhachHang (
  MaKH VARCHAR(10) PRIMARY KEY,
  TenKH VARCHAR(100) NOT NULL,
  SoDienThoai VARCHAR(15) NOT NULL,
  DiaChi VARCHAR(200)
);

-- Create HoaDon Table
CREATE TABLE HoaDon (
  MaHD VARCHAR(10) PRIMARY KEY,
  NgayLap DATETIME DEFAULT NOW(),
  MaKH VARCHAR(10),
  TongTien DECIMAL(15,2),
  FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
);

-- Create ChiTietHD Table
CREATE TABLE ChiTietHD (
  MaHD VARCHAR(10),
  MaSP VARCHAR(10),
  SoLuong INT,
  DonGia DECIMAL(10,2),
  PRIMARY KEY (MaHD, MaSP),
  FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
  FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

-- Create PhieuNhap Table
CREATE TABLE PhieuNhap (
  MaPN VARCHAR(10) PRIMARY KEY,
  NgayNhap DATETIME DEFAULT NOW(),
  TongTienNhap DECIMAL(15,2)
);

-- Create ChiTietPN Table
CREATE TABLE ChiTietPN (
  MaPN VARCHAR(10),
  MaSP VARCHAR(10),
  SoLuong INT,
  GiaNhap DECIMAL(10,2),
  PRIMARY KEY (MaPN, MaSP),
  FOREIGN KEY (MaPN) REFERENCES PhieuNhap(MaPN),
  FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);
```

## Required Stored Procedures

```sql
-- Get All Products
DELIMITER //
CREATE PROCEDURE sp_LayDSSanPham()
BEGIN
  SELECT * FROM SanPham;
END //
DELIMITER ;

-- Add Product
DELIMITER //
CREATE PROCEDURE sp_ThemSanPham(
  IN p_MaSP VARCHAR(10),
  IN p_TenSP VARCHAR(100),
  IN p_Gia DECIMAL(10,2),
  IN p_Ton INT,
  IN p_DVT VARCHAR(20),
  IN p_MaLoai VARCHAR(10)
)
BEGIN
  INSERT INTO SanPham VALUES (p_MaSP, p_TenSP, p_Gia, p_Ton, p_DVT, p_MaLoai);
END //
DELIMITER ;

-- Update Product
DELIMITER //
CREATE PROCEDURE sp_SuaSanPham(
  IN p_MaSP VARCHAR(10),
  IN p_TenSP VARCHAR(100),
  IN p_Gia DECIMAL(10,2),
  IN p_Ton INT,
  IN p_DVT VARCHAR(20),
  IN p_MaLoai VARCHAR(10)
)
BEGIN
  UPDATE SanPham SET TenSP=p_TenSP, GiaBan=p_Gia, SoLuongTon=p_Ton, 
					 DonViTinh=p_DVT, MaLoai=p_MaLoai WHERE MaSP=p_MaSP;
END //
DELIMITER ;

-- Delete Product
DELIMITER //
CREATE PROCEDURE sp_XoaSanPham(IN p_maSP VARCHAR(10))
BEGIN
  DELETE FROM SanPham WHERE MaSP=p_maSP;
END //
DELIMITER ;

-- Get All Customers
DELIMITER //
CREATE PROCEDURE sp_LayDSKhachHang()
BEGIN
  SELECT * FROM KhachHang;
END //
DELIMITER ;

-- Add Customer
DELIMITER //
CREATE PROCEDURE sp_ThemKhachHang(
  IN p_MaKH VARCHAR(10),
  IN p_TenKH VARCHAR(100),
  IN p_SDT VARCHAR(15),
  IN p_DiaChi VARCHAR(200)
)
BEGIN
  INSERT INTO KhachHang VALUES (p_MaKH, p_TenKH, p_SDT, p_DiaChi);
END //
DELIMITER ;

-- Update Customer
DELIMITER //
CREATE PROCEDURE sp_SuaKhachHang(
  IN p_MaKH VARCHAR(10),
  IN p_TenKH VARCHAR(100),
  IN p_SDT VARCHAR(15),
  IN p_DiaChi VARCHAR(200)
)
BEGIN
  UPDATE KhachHang SET TenKH=p_TenKH, SoDienThoai=p_SDT, DiaChi=p_DiaChi WHERE MaKH=p_MaKH;
END //
DELIMITER ;

-- Find Customer by Phone
DELIMITER //
CREATE PROCEDURE sp_TimKhachHangTheoSDT(IN p_SDT VARCHAR(15))
BEGIN
  SELECT * FROM KhachHang WHERE SoDienThoai=p_SDT LIMIT 1;
END //
DELIMITER ;

-- Get All Invoices
DELIMITER //
CREATE PROCEDURE sp_LayDSHoaDon()
BEGIN
  SELECT * FROM HoaDon;
END //
DELIMITER ;

-- Add Invoice
DELIMITER //
CREATE PROCEDURE sp_LuuHoaDon(
  IN p_MaHD VARCHAR(10),
  IN p_Ngay DATETIME,
  IN p_MaKH VARCHAR(10),
  IN p_Tong DECIMAL(15,2)
)
BEGIN
  INSERT INTO HoaDon VALUES (p_MaHD, p_Ngay, p_MaKH, p_Tong);
END //
DELIMITER ;

-- Add Invoice Detail
DELIMITER //
CREATE PROCEDURE sp_LuuChiTietHD(
  IN p_MaHD VARCHAR(10),
  IN p_MaSP VARCHAR(10),
  IN p_SoLuong INT,
  IN p_DonGia DECIMAL(10,2)
)
BEGIN
  INSERT INTO ChiTietHD VALUES (p_MaHD, p_MaSP, p_SoLuong, p_DonGia);
END //
DELIMITER ;

-- Get All Import Receipts
DELIMITER //
CREATE PROCEDURE sp_LayDSPhieuNhap()
BEGIN
  SELECT * FROM PhieuNhap;
END //
DELIMITER ;

-- Add Import Receipt
DELIMITER //
CREATE PROCEDURE sp_LuuPhieuNhap(
  IN p_MaPN VARCHAR(10),
  IN p_Ngay DATETIME,
  IN p_Tong DECIMAL(15,2)
)
BEGIN
  INSERT INTO PhieuNhap VALUES (p_MaPN, p_Ngay, p_Tong);
END //
DELIMITER ;

-- Add Import Receipt Detail
DELIMITER //
CREATE PROCEDURE sp_LuuChiTietPN(
  IN p_MaPN VARCHAR(10),
  IN p_MaSP VARCHAR(10),
  IN p_SoLuong INT,
  IN p_GiaNhap DECIMAL(10,2)
)
BEGIN
  INSERT INTO ChiTietPN VALUES (p_MaPN, p_MaSP, p_SoLuong, p_GiaNhap);
  UPDATE SanPham SET SoLuongTon = SoLuongTon + p_SoLuong WHERE MaSP=p_MaSP;
END //
DELIMITER ;
```

## Testing the Connection

After setting up the database and procedures:

1. **In Visual Studio**, open the Package Manager Console
2. **Ensure MySQL is running** (start MySQL server)
3. **Run the application** with F5
4. **Test each tab** to verify connection is working
5. **Check for error messages** if data doesn't load

## Troubleshooting Connection Issues

### Error: "Access denied for user 'root'@'localhost'"
- Verify username and password in connection string
- Ensure MySQL user account exists
- Grant necessary permissions:
  ```sql
  GRANT ALL PRIVILEGES ON WarehouseDB.* TO 'username'@'localhost' IDENTIFIED BY 'password';
  FLUSH PRIVILEGES;
  ```

### Error: "Unknown database 'WarehouseDB'"
- Verify database name matches in connection string
- Create the database using the SQL script above
- Check spelling (case-sensitive on Linux/Mac)

### Error: "Can't connect to MySQL server"
- Verify MySQL service is running
- Check correct server address and port
- Ensure MySQL port 3306 is not blocked by firewall

### Error: "Procedure not found"
- Verify stored procedure exists:
  ```sql
  SHOW PROCEDURE STATUS WHERE Db='WarehouseDB';
  ```
- Create missing procedures using the scripts above
- Check for typos in procedure names

## Performance Tips

1. Add indexes on frequently searched columns:
   ```sql
   ALTER TABLE SanPham ADD INDEX (MaLoai);
   ALTER TABLE KhachHang ADD INDEX (SoDienThoai);
   ALTER TABLE HoaDon ADD INDEX (MaKH);
   ```

2. Use pagination for large datasets
3. Cache frequently accessed data
4. Regular database maintenance and optimization

## Security Recommendations

1. Use strong passwords for database accounts
2. Limit database user permissions (don't use root in production)
3. Encrypt sensitive data
4. Use parameterized queries (already implemented)
5. Regular backups of database
6. Monitor database access and changes
