# Quick Test Card - frmSanPham & frmBanHang

## 🚀 Start Testing Now

### Step 1: Run Application
```
Ctrl+F5 in Visual Studio
```

### Step 2: Test frmSanPham (Quản Lý Sản Phẩm)
```
Main Window → Click [Quản Lý Sản Phẩm] Button
```

### Step 3: Test Each Feature (< 2 minutes each)

#### ✅ TEST 1: Load Data
```
Expected: DataGridView shows products from MySQL
Result: _____ (Pass/Fail)
```

#### ✅ TEST 2: Search
```
Actions:
  1. Type "Nước" in search box
Expected: Only products with "Nước" in name
Result: _____ (Pass/Fail)
```

#### ✅ TEST 3: Select Row
```
Actions:
  1. Click any product row
Expected: Form fields populate, MaSP locked
Result: _____ (Pass/Fail)
```

#### ✅ TEST 4: Add New
```
Actions:
  1. Clear all fields (or click new empty area)
  2. Enter: MaSP="SP123", TenSP="Product X", GiaBan=50000
  3. Click [Thêm]
Expected: Success message + Product in list
Result: _____ (Pass/Fail)
Notes: _________________________
```

#### ✅ TEST 5: Edit
```
Actions:
  1. Select a product row
  2. Change name/price/quantity
  3. Click [Sửa]
Expected: Success message + List updated
Result: _____ (Pass/Fail)
Notes: _________________________
```

#### ✅ TEST 6: Delete
```
Actions:
  1. Select a product row
  2. Click [Xóa]
  3. Confirm "Yes"
Expected: Product removed from list
Result: _____ (Pass/Fail)
Notes: _________________________
```

#### ✅ TEST 7: Sales Form
```
Actions:
  1. Back to Main
  2. Click [Bán Hàng]
  3. Check product list matches frmSanPham
  4. Add products to cart
  5. Enter customer phone
  6. Click [Checkout]
Expected: Invoice saved, cart clears
Result: _____ (Pass/Fail)
Notes: _________________________
```

---

## 🐛 If Something Fails

### Check These First
```
❌ Products not showing?
   → Check MySQL connection string in DAL\Helper\dbConnection.cs
   → Verify DBQuanLyBanHang database exists
   → Run SELECT * FROM SanPham in MySQL

❌ Buttons don't work?
   → Already fixed! Event handlers are connected in Designer

❌ "Thêm mới thất bại"?
   → Make sure MaSP is unique
   → Make sure GiaBan >= 0

❌ Can't delete product?
   → Product exists in invoice (HoaDon)
   → This is correct behavior - protects data integrity
```

### Debug Steps
```
1. Open Output Window (View → Output)
2. Look for exception messages
3. Check MySQL Server is running
4. Verify connectionString matches your DB
5. Run Build → Check error list
```

---

## 📊 Test Coverage Checklist

| Feature | Manual | Auto | Notes |
|---------|--------|------|-------|
| Load products | ✅ | ❌ | Works via frmSanPham_Load |
| Search | ✅ | ❌ | Real-time search via TextChanged |
| Add product | ✅ | ❌ | With validation |
| Edit product | ✅ | ❌ | MaSP protected |
| Delete product | ✅ | ❌ | With confirmation |
| Customer lookup | ✅ | ❌ | Works in frmBanHang |
| Cart management | ✅ | ❌ | Works in frmBanHang |
| Checkout | ✅ | ❌ | Saves invoice + details |

---

## 💾 How to Save Test Results

### Option 1: Take Screenshots
```
Press PrintScreen during tests
Paste into OneNote/Excel
Add timestamps & notes
```

### Option 2: Export Data
```sql
-- Check if your changes were saved
SELECT * FROM SanPham ORDER BY MaSP DESC LIMIT 10;
SELECT * FROM HoaDon ORDER BY MaHD DESC LIMIT 10;
```

### Option 3: Create Test Report
```
Copy this template to TestResults.txt:

TEST DATE: _________
TESTER: _____________
BUILD: ✅ SUCCESS

TEST RESULTS:
- Load: _____ (Pass/Fail)
- Search: _____ (Pass/Fail)
- Add: _____ (Pass/Fail)
- Edit: _____ (Pass/Fail)
- Delete: _____ (Pass/Fail)
- Checkout: _____ (Pass/Fail)

ISSUES FOUND:
1. ___________________________
2. ___________________________

NOTES:
_________________________________
```

---

## 📞 Key Contacts in Code

### If you need to add features:

**Add product validation?**
→ Edit `SanPhamBUS.LuuSanPham()` in BUS\SanPhamBUS.cs

**Change database connection?**
→ Edit `strCon` in DAL\Helper\dbConnection.cs

**Modify UI layout?**
→ Edit frmSanPham.Designer.cs or use Form Designer

**Change button names?**
→ Edit `Text` property in Designer or .cs file

---

## 🎯 Success Criteria

### ✅ Minimal Success
- [ ] App runs without crashes
- [ ] Products load from MySQL
- [ ] Can add 1 new product
- [ ] Can edit that product
- [ ] Can delete it

### ✅ Full Success
- [ ] All 7 tests pass
- [ ] Search works
- [ ] Sales form shows updated products
- [ ] Can create invoice
- [ ] No validation errors

### ✅ Production Ready
- [ ] All features tested
- [ ] No MySQL errors
- [ ] Performance acceptable
- [ ] Data integrity verified
- [ ] Documentation complete

---

**⏱️ Estimated Testing Time: 15-30 minutes**

**📋 Total Effort: ~2 hours (if no issues)**

**Good luck! 🚀**
