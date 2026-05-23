-- Thêm dữ liệu khách hàng vào database
DELETE FROM KhachHang;

INSERT INTO KhachHang (MaKH, TenKH, SoDienThoai, DiaChi) VALUES
('KH001', 'Nguyễn Văn An', '0912345678', '123 Đường Lê Lợi, Hà Nội'),
('KH002', 'Trần Thị Bình', '0987654321', '456 Đường Trần Hưng Đạo, TP.HCM'),
('KH003', 'Phạm Văn Cường', '0934567890', '789 Đường Nguyễn Huệ, Hà Nội'),
('KH004', 'Lê Thị Diễm', '0956789012', '321 Đường Hùng Vương, Đà Nẵng'),
('KH005', 'Hoàng Văn Em', '0945678901', '654 Đường Võ Văn Kiệt, TP.HCM'),
('KH006', 'Vũ Thị Phương', '0923456789', '789 Đường Bạch Đằng, Hải Phòng'),
('KH007', 'Đặng Văn Giang', '0967890123', '147 Đường Cách Mạng Tháng 8, Cần Thơ'),
('KH008', 'Cao Thị Hương', '0901234567', '258 Đường Pasteur, Hà Nội'),
('KH009', 'Lý Văn Hoàng', '0978901234', '369 Đường Ngã Năm, TP.HCM'),
('KH010', 'Dương Thị Kiều', '0909876543', '741 Đường Tôn Đức Thắng, Đà Nẵng');

SELECT * FROM KhachHang;
