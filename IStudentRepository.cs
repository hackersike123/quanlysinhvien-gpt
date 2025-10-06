using System;
using System.Collections.Generic;
using System.Linq;

/* =============================================================
 * GHI CHÚ REPOSITORY INTERFACE
 * IStudentRepository: định nghĩa các thao tác CRUD cơ bản trên thực thể SinhVien.
 * Cho phép thay thế triển khai (InMemory / Database / File ...).
 * ============================================================= */

namespace QuanLyDiemSinhVien
{
    /// <summary>
    /// Repository abstraction để lưu trữ SinhVien (có thể thay bằng DB/File sau này).
    /// Triển khai tối thiểu để dễ test / mở rộng.
    /// Loại bỏ nullable reference syntax để tương thích C# 7.3 (.NET Framework 4.7.2 mặc định).
    /// </summary>
    public interface IStudentRepository
    {
        IReadOnlyCollection<SinhVien> GetAll(); // Lấy toàn bộ sinh viên (readonly)
        SinhVien GetByCode(string maSo);        // Trả null nếu không tìm thấy
        void Add(SinhVien sv);                  // Thêm mới
        void Update(SinhVien sv);               // Cập nhật theo MaSo
        bool Remove(string maSo);               // Xóa, trả về true nếu thành công
    }
}
