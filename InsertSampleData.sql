-- Script thêm dữ liệu mẫu vào database DBquanlybanhang
-- Sản phẩm: Nem Ngựa - nhiều loại khác nhau

-- Xóa dữ liệu theo thứ tự constraint
DELETE FROM HoaDon;
DELETE FROM SanPham;
DELETE FROM LoaiSanPham;
DELETE FROM KhachHang;

-- 1. Thêm dữ liệu vào bảng Loại Sản Phẩm
INSERT INTO LoaiSanPham (MaLoai, TenLoai) VALUES 
('L001', 'Nem ngựa cay'),
('L002', 'Nem ngựa chua'),
('L003', 'Nem ngựa mặn'),
('L004', 'Nem ngựa cay xanh'),
('L005', 'Nem ngựa cay đỏ'),
('L006', 'Nem ngựa siêu cay'),
('L007', 'Nem ngựa tây');

-- 2. Thêm dữ liệu vào bảng Sản Phẩm - Tất cả là Nem Ngựa
INSERT INTO SanPham (MaSP, TenSP, GiaBan, SoLuongTon, DonViTinh, MaLoai) VALUES 
('SP001', 'Nem Ngựa Cay - Chuẩn', 95000, 150, 'gói', 'L001'),
('SP002', 'Nem Ngựa Cay - Vừa', 65000, 200, 'gói', 'L001'),
('SP003', 'Nem Ngựa Chua - Truyền Thống', 90000, 180, 'gói', 'L002'),
('SP004', 'Nem Ngựa Chua - Nhẹ', 70000, 160, 'gói', 'L002'),
('SP005', 'Nem Ngựa Mặn - Đặc Biệt', 100000, 120, 'gói', 'L003'),
('SP006', 'Nem Ngựa Mặn - Thường', 75000, 140, 'gói', 'L003'),
('SP007', 'Nem Ngựa Cay Xanh - Tươi', 105000, 100, 'gói', 'L004'),
('SP008', 'Nem Ngựa Cay Xanh - Đặc', 110000, 80, 'gói', 'L004'),
('SP009', 'Nem Ngựa Cay Đỏ - Nóng', 115000, 90, 'gói', 'L005'),
('SP010', 'Nem Ngựa Cay Đỏ - Cực Cay', 125000, 50, 'gói', 'L005'),
('SP011', 'Nem Ngựa Siêu Cay - Giới Hạn', 150000, 30, 'gói', 'L006'),
('SP012', 'Nem Ngựa Tây - Fusion', 120000, 60, 'gói', 'L007');

-- 3. Thêm dữ liệu vào bảng Khách Hàng
INSERT INTO KhachHang (MaKH, TenKH, SoDienThoai, DiaChi) VALUES 
('KH001', 'Nguyễn Văn A', '0912345678', '123 Đường Lê Lợi, Hà Nội'),
('KH002', 'Trần Thị B', '0987654321', '456 Đường Trần Hưng Đạo, TP.HCM'),
('KH003', 'Phạm Văn C', '0934567890', '789 Đường Nguyễn Huệ, Hà Nội'),
('KH004', 'Lê Thị D', '0956789012', '321 Đường Hùng Vương, Đà Nẵng'),
('KH005', 'Hoàng Văn E', '0945678901', '654 Đường Võ Văn Kiệt, TP.HCM'),
('KH006', 'Vũ Thị F', '0923456789', '789 Đường Bạch Đằng, Hải Phòng'),
('KH007', 'Đặng Văn G', '0967890123', '147 Đường Cách Mạng Tháng 8, Cần Thơ');

-- 4. Thêm dữ liệu vào bảng Hóa Đơn
INSERT INTO HoaDon (MaHD, MaKH, NgayLap, TongTien) VALUES 
('HD001', 'KH001', '2026-05-21', 650000),
('HD002', 'KH002', '2026-05-21', 920000),
('HD003', 'KH003', '2026-05-20', 750000),
('HD004', 'KH004', '2026-05-20', 1100000),
('HD005', 'KH005', '2026-05-19', 850000);

-- Kiểm tra dữ liệu
SELECT '=== Loại Sản Phẩm ===' AS Info;
SELECT * FROM LoaiSanPham;

SELECT '=== Sản Phẩm (Nem Ngựa) ===' AS Info;
SELECT * FROM SanPham;

SELECT '=== Khách Hàng ===' AS Info;
SELECT * FROM KhachHang;

SELECT '=== Hóa Đơn ===' AS Info;
SELECT * FROM HoaDon;
