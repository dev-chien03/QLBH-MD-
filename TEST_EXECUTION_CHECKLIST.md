# Test Execution Checklist - frmSanPham & frmBanHang

## Environment Setup
- [ ] Visual Studio 2026 open
- [ ] Solution loaded: D:\Learning\dotnet\QLBH(MD)\
- [ ] MySQL Server running (check port 3306)
- [ ] Database DBQuanLyBanHang available
- [ ] All tables exist (SanPham, HoaDon, KhachHang, etc.)

---

## BUILD VERIFICATION
```
[ ] Build: Ctrl+Shift+B
	Expected: "Build successful"
	Result: ________________

[ ] Check for errors: View → Error List
	Expected: 0 errors, 0 warnings
	Result: ________________
```

---

## LAUNCH TEST
```
[ ] Start: Ctrl+F5 (or F5)
	Expected: Main form appears with 2 buttons
	Result: ________________

[ ] Main form displays:
	- [ ] "Quản Lý Sản Phẩm" button
	- [ ] "Bán Hàng" button

[ ] Click "Quản Lý Sản Phẩm"
	Expected: frmSanPham opens (no crashes)
	Result: ________________
```

---

## TEST 1: LOAD DATA (Quản Lý Sản Phẩm)
```
Test Case: frmSanPham_Load
Purpose: Verify database connection and data loading

[ ] Precondition: frmSanPham is open
[ ] Check DataGridView is populated
	- Products visible: _____ (Yes/No)
	- Columns visible: _____ (Yes/No)

[ ] Check ComboBox "Mã Loại" is populated
	- Categories loaded: _____ (Yes/No)
	- Can select category: _____ (Yes/No)

[ ] Check Data Formatting
	- Header color (dark): _____ (Yes/No)
	- Alternating row colors: _____ (Yes/No)
	- Font looks good: _____ (Yes/No)

Status: ✅ PASS / ❌ FAIL

Issues Found:
_________________________________
_________________________________
```

---

## TEST 2: SEARCH FUNCTIONALITY
```
Test Case: txtTimKiem_TextChanged
Purpose: Verify real-time product search

[ ] Precondition: frmSanPham is open with data

[ ] Clear search box first
	- Full product list shows: _____ (Yes/No)

[ ] Type partial product name (e.g., "Nước")
	- List filters in real-time: _____ (Yes/No)
	- Only matching products show: _____ (Yes/No)
	- Performance acceptable (< 500ms): _____ (Yes/No)

[ ] Delete search text
	- Full list returns: _____ (Yes/No)

[ ] Search for non-existent product
	- No results shown: _____ (Yes/No)
	- No error message: _____ (Yes/No)

Status: ✅ PASS / ❌ FAIL

Notes:
_________________________________
```

---

## TEST 3: SELECT ROW / POPULATE FORM
```
Test Case: dgvSanPham_CellClick
Purpose: Verify data population to edit form

[ ] Precondition: frmSanPham is open

[ ] Click a product row (any row)
	- Form fields populate: _____ (Yes/No)
	- Mã SP field shows value: _____ (Yes/No)
	- Tên SP field shows value: _____ (Yes/No)
	- Giá Bán shows value: _____ (Yes/No)
	- Số Lượng Tồn shows value: _____ (Yes/No)
	- Đơn Vị Tính shows value: _____ (Yes/No)
	- Category selected in combo: _____ (Yes/No)

[ ] Verify MaSP field is locked
	- Cannot type in Mã SP: _____ (Yes/No)
	- Cannot edit MaSP: _____ (Yes/No)

[ ] Click another product row
	- Fields update with new data: _____ (Yes/No)
	- MaSP still locked: _____ (Yes/No)

Status: ✅ PASS / ❌ FAIL

Issues Found:
_________________________________
```

---

## TEST 4: ADD NEW PRODUCT (Thêm)
```
Test Case: btnThem_Click + KiemTraDuLieuDauVao
Purpose: Verify product creation with validation

[ ] Precondition: frmSanPham is open, clear search

[ ] Click "New" or clear form (or click empty area)
[ ] Verify MaSP is unlocked: _____ (Yes/No)

[ ] SUBTEST 4A: Add without required fields
	- Leave Mã SP empty
	- Click [Thêm]
	- Expected: Error message about required fields
	- Message shown: _____ (Yes/No)
	- Product not added: _____ (Yes/No)

[ ] SUBTEST 4B: Add with invalid price
	- Mã SP: "SP_TEST_001"
	- Tên SP: "Test Product"
	- Giá Bán: "-100" (negative)
	- Click [Thêm]
	- Expected: Validation error (negative price)
	- Error shown: _____ (Yes/No)

[ ] SUBTEST 4C: Add valid product
	- Mã SP: "SP_TEST_001"
	- Tên SP: "Test Product"
	- Giá Bán: "100000"
	- Số Lượng: "50"
	- Đơn Vị Tính: "cái"
	- Mã Loại: (select any)
	- Click [Thêm]
	- Expected: Success message
	- Message shown: _____ (Yes/No)
	- Product in list: _____ (Yes/No)
	- Form cleared: _____ (Yes/No)
	- MaSP unlocked: _____ (Yes/No)

[ ] Verify in database
	- SELECT * FROM SanPham WHERE MaSP='SP_TEST_001';
	- Product exists in DB: _____ (Yes/No)

Status: ✅ PASS / ❌ FAIL

Notes:
_________________________________
```

---

## TEST 5: EDIT PRODUCT (Sửa)
```
Test Case: btnSua_Click
Purpose: Verify product update

[ ] Precondition: frmSanPham is open

[ ] SUBTEST 5A: Edit without selecting
	- Clear search to get full list
	- Don't select any row
	- Click [Sửa]
	- Expected: Warning to select product
	- Warning shown: _____ (Yes/No)

[ ] SUBTEST 5B: Edit existing product
	- Click on product row (e.g., SP_TEST_001)
	- Change Tên SP: "Updated Test Product"
	- Change Giá Bán: "150000"
	- Click [Sửa]
	- Expected: Success message
	- Message shown: _____ (Yes/No)
	- Data in list updated: _____ (Yes/No)
	- Form cleared: _____ (Yes/No)

[ ] Verify in database
	- SELECT * FROM SanPham WHERE MaSP='SP_TEST_001';
	- Name updated: _____ (Yes/No)
	- Price updated: _____ (Yes/No)

Status: ✅ PASS / ❌ FAIL

Issues Found:
_________________________________
```

---

## TEST 6: DELETE PRODUCT (Xóa)
```
Test Case: btnXoa_Click
Purpose: Verify product deletion with protection

[ ] Precondition: frmSanPham is open

[ ] SUBTEST 6A: Delete without selection
	- Clear form (or don't select any row)
	- Click [Xóa]
	- Expected: Warning to select product
	- Warning shown: _____ (Yes/No)

[ ] SUBTEST 6B: Delete with confirmation
	- Click product row (SP_TEST_001)
	- Click [Xóa]
	- Expected: "Are you sure?" dialog
	- Dialog shown: _____ (Yes/No)
	- Click [No]
	- Product still exists: _____ (Yes/No)

[ ] SUBTEST 6C: Delete with confirmation accept
	- Click product row again
	- Click [Xóa]
	- Click [Yes] on confirmation
	- Expected: Success message
	- Message shown: _____ (Yes/No)
	- Product removed from list: _____ (Yes/No)
	- Form cleared: _____ (Yes/No)

[ ] Verify in database
	- SELECT * FROM SanPham WHERE MaSP='SP_TEST_001';
	- Product gone: _____ (Yes/No)

[ ] SUBTEST 6D: Delete protected product
	- Create invoice with a product (or use existing)
	- Try to delete that product
	- Expected: Error "Product exists in invoice"
	- Error shown: _____ (Yes/No)
	- Product NOT deleted: _____ (Yes/No)

Status: ✅ PASS / ❌ FAIL

Issues Found:
_________________________________
```

---

## TEST 7: SALES FORM INTEGRATION
```
Test Case: frmBanHang integration
Purpose: Verify product list syncs between forms

[ ] Precondition: frmSanPham tested, product changes made

[ ] Close frmSanPham (or leave open)
[ ] Go back to Main form (click X or navigate)
[ ] Click [Bán Hàng]
	Expected: frmBanHang opens
	Result: _____ (Yes/No)

[ ] Check product list in frmBanHang
	- Products loaded: _____ (Yes/No)
	- Count matches frmSanPham: _____ (Yes/No)
	- New products visible: _____ (Yes/No)
	- Updated prices shown: _____ (Yes/No)

[ ] Test quick sale:
	- Select a product
	- Enter Quantity: 2
	- Click [Add to cart]
	- Expected: Product in cart, total updated
	- In cart: _____ (Yes/No)
	- Total calculated: _____ (Yes/No)

[ ] Test checkout:
	- Leave customer phone empty (or enter)
	- Click [Checkout]
	- Expected: Invoice saved (or message for customer)
	- Invoice saved: _____ (Yes/No)
	- Cart cleared: _____ (Yes/No)

[ ] Verify in database
	- SELECT * FROM HoaDon ORDER BY MaHD DESC LIMIT 1;
	- Invoice exists: _____ (Yes/No)
	- SELECT * FROM ChiTietHD WHERE MaHD='<latest>';
	- Line items exist: _____ (Yes/No)

Status: ✅ PASS / ❌ FAIL

Issues Found:
_________________________________
```

---

## OVERALL RESULTS

### Test Summary
```
Test 1 (Load Data):         ✅ PASS / ❌ FAIL
Test 2 (Search):            ✅ PASS / ❌ FAIL
Test 3 (Select Row):        ✅ PASS / ❌ FAIL
Test 4 (Add Product):       ✅ PASS / ❌ FAIL
Test 5 (Edit Product):      ✅ PASS / ❌ FAIL
Test 6 (Delete Product):    ✅ PASS / ❌ FAIL
Test 7 (Sales Integration): ✅ PASS / ❌ FAIL
```

### Overall Status
```
Total Tests: 7
Passed: ___
Failed: ___
Percentage: ___% (Target: 100%)
```

### Issues Severity
```
Critical (blocks usage):   [ ]
Major (significant flaw):  [ ]
Minor (cosmetic):          [ ]
```

---

## DEFECTS LOG

### Issue #1
```
Title: _________________________________
Severity: Critical / Major / Minor
Found in: Test #_
Description:
  _________________________________
  _________________________________

Steps to Reproduce:
  1. _________________________________
  2. _________________________________
  3. _________________________________

Expected: _________________________________
Actual: _________________________________

Workaround: _________________________________
```

### Issue #2
```
Title: _________________________________
Severity: Critical / Major / Minor
Found in: Test #_
Description:
  _________________________________

Workaround: _________________________________
```

---

## SIGN-OFF

**Tester Name:** _________________________________
**Test Date:** _________________________________
**Build Version:** ✅ SUCCESS
**Overall Result:** ✅ PASS / ❌ FAIL

**Approved by (QA Lead):** _________________________________
**Date:** _________________________________

**Ready for Production:** ✅ YES / ❌ NO (pending fixes)

---

## Notes & Recommendations

### What Worked Well
1. _________________________________
2. _________________________________
3. _________________________________

### Improvements Needed
1. _________________________________
2. _________________________________
3. _________________________________

### For Next Phase
1. _________________________________
2. _________________________________
3. _________________________________

---

**Test Execution Time:** _____ minutes
**Estimated Production Readiness:** _____ days
