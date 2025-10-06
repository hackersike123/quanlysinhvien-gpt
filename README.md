# Quản Lý Điểm Sinh Viên (WinForms / .NET Framework 4.7.2)

## 1. Giới thiệu
Ứng dụng WinForms demo quản lý danh sách sinh viên: thêm, sửa, xóa, tìm kiếm, lọc theo khoa và thống kê điểm trung bình.

## 2. Công nghệ
- .NET Framework 4.7.2
- WinForms
- Kiến trúc tách lớp đơn giản (UI / Service / Repository / Model)

## 3. Cấu trúc mã nguồn
```
Program.cs                // Điểm vào
Form1.*, frmMain          // Giao diện & sự kiện
SinhVien.cs               // Model
IStudentRepository.cs     // Interface lưu trữ
InMemoryStudentRepository // Lưu trữ trong RAM
StudentService.cs         // Nghiệp vụ: validate + CRUD + tìm kiếm + lọc
```

## 4. Tính năng chính
- Thêm / Sửa / Xóa sinh viên
- Validate đầu vào (Mã số, Họ tên, Khoa, Điểm 0–10)
- Tìm kiếm theo Mã số hoặc Họ tên (substring, không phân biệt hoa thường)
- Lọc theo Khoa (ComboBox)
- Thống kê: điểm trung bình chung & theo khoa
- Tự động thêm khoa mới vào danh sách nếu nhập khoa chưa có
- Nút Clear thoát chế độ sửa nhanh
- Tô màu dòng theo mức điểm (Yếu / TB / Khá / Giỏi)

## 5. Quy ước / Ràng buộc
| Trường | Ràng buộc |
|--------|-----------|
| Mã số  | 1–12 ký tự chữ hoặc số |
| Họ tên | <= 100 ký tự, chỉ chữ + khoảng trắng |
| Khoa   | <= 50 ký tự, chữ/số/khoảng trắng/-/_ |
| Điểm   | 0.00 – 10.00 |

## 6. Hướng dẫn chạy
1. Mở solution bằng Visual Studio 2022
2. Khôi phục (nếu cần) và Build (Ctrl+Shift+B)
3. Run (F5)

## 7. Kịch bản kiểm thử thủ công gợi ý
1. Thêm hợp lệ một sinh viên
2. Thêm trùng Mã số → phải báo lỗi
3. Nhập điểm âm / >10 / ký tự → báo lỗi
4. Chọn dòng → Sửa → Clear → kiểm tra trạng thái thêm mới
5. Xóa sinh viên đang chọn → biến mất khỏi lưới
6. Tìm kiếm một phần họ tên → lọc đúng
7. Thêm khoa mới (gõ ở ô Khoa) → sau thêm xuất hiện trong ComboBox lọc
8. Thống kê khi: (a) Có dữ liệu (b) Rỗng
9. Thêm nhiều sinh viên và kiểm tra màu nền theo điểm
10. Kiểm tra nhập tên chứa số → bị chặn

## 8. Định hướng cải tiến
- Binding `DataGridView` qua `BindingSource`
- Thêm lớp `IStudentService` để dễ unit test
- Lưu trữ sang file hoặc database (SQLite)
- Thêm xuất Excel / CSV
- Thêm unit test (NUnit / xUnit) cho `StudentService`

## 9. Phiên bản
- v1.0.0: Phiên bản ổn định đầu tiên (UI cải thiện: Clear, tự thêm khoa, chú thích mã)

## 10. Giấy phép
Tùy chọn (chưa khai báo). Có thể thêm MIT nếu cần.

---
Đóng góp / Issue: tạo Issue trên repository.
