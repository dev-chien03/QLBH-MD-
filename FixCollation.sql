-- Fix Collation Issues for MySQL 8.0
-- Lý do: bảng dùng utf8mb4_unicode_ci nhưng stored procedure được tạo với
-- collation_connection = utf8mb4_0900_ai_ci (mặc định MySQL 8).
-- Khi SP so sánh WHERE col = p_param -> 2 collation khác nhau -> lỗi 1267.
-- Cách fix: thêm COLLATE rõ ràng vào mệnh đề WHERE.

USE DBquanlybanhang;

DROP PROCEDURE IF EXISTS sp_SuaSanPham;
DROP PROCEDURE IF EXISTS sp_XoaSanPham;
DROP PROCEDURE IF EXISTS sp_SuaKhachHang;
DROP PROCEDURE IF EXISTS sp_LuuChiTietHD;
DROP PROCEDURE IF EXISTS sp_LuuChiTietPN;
DROP PROCEDURE IF EXISTS sp_TimKhachHangTheoSDT;

DELIMITER //
CREATE PROCEDURE sp_SuaSanPham(
  IN p_MaSP VARCHAR(10),
  IN p_TenSP VARCHAR(100),
  IN p_Gia DECIMAL(18,2),
  IN p_Ton INT,
  IN p_DVT VARCHAR(20),
  IN p_MaLoai VARCHAR(10)
)
BEGIN
  UPDATE SanPham
    SET TenSP = p_TenSP, GiaBan = p_Gia, SoLuongTon = p_Ton,
        DonViTinh = p_DVT, MaLoai = p_MaLoai
  WHERE MaSP COLLATE utf8mb4_unicode_ci = p_MaSP;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE sp_XoaSanPham(IN p_maSP VARCHAR(10))
BEGIN
  DELETE FROM SanPham WHERE MaSP COLLATE utf8mb4_unicode_ci = p_maSP;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE sp_SuaKhachHang(
  IN p_MaKH VARCHAR(10),
  IN p_TenKH VARCHAR(100),
  IN p_SDT VARCHAR(20),
  IN p_DiaChi VARCHAR(255)
)
BEGIN
  UPDATE KhachHang
    SET TenKH = p_TenKH, SoDienThoai = p_SDT, DiaChi = p_DiaChi
  WHERE MaKH COLLATE utf8mb4_unicode_ci = p_MaKH;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE sp_LuuChiTietHD(
  IN p_MaHD VARCHAR(10),
  IN p_MaSP VARCHAR(10),
  IN p_SoLuong INT,
  IN p_DonGia DECIMAL(18,2)
)
BEGIN
  INSERT INTO ChiTietHD VALUES (p_MaHD, p_MaSP, p_SoLuong, p_DonGia);
  UPDATE SanPham SET SoLuongTon = SoLuongTon - p_SoLuong
    WHERE MaSP COLLATE utf8mb4_unicode_ci = p_MaSP;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE sp_LuuChiTietPN(
  IN p_MaPN VARCHAR(10),
  IN p_MaSP VARCHAR(10),
  IN p_SoLuong INT,
  IN p_GiaNhap DECIMAL(18,2)
)
BEGIN
  INSERT INTO ChiTietPN VALUES (p_MaPN, p_MaSP, p_SoLuong, p_GiaNhap);
  UPDATE SanPham SET SoLuongTon = SoLuongTon + p_SoLuong
    WHERE MaSP COLLATE utf8mb4_unicode_ci = p_MaSP;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE sp_TimKhachHangTheoSDT(IN p_sdt VARCHAR(20))
BEGIN
  SELECT * FROM KhachHang
    WHERE SoDienThoai COLLATE utf8mb4_unicode_ci = p_sdt;
END //
DELIMITER ;

SELECT 'Collation fix applied successfully' AS status;
