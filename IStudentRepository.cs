using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyDiemSinhVien
{
    /// <summary>
    /// Repository abstraction để lưu trữ SinhVien (có thể thay bằng DB/File sau này).
    /// Triển khai tối thiểu để dễ test / mở rộng.
    /// Loại bỏ nullable reference syntax để tương thích C# 7.3 (.NET Framework 4.7.2 mặc định).
    /// </summary>
    public interface IStudentRepository
    {
        IReadOnlyCollection<SinhVien> GetAll();
        SinhVien GetByCode(string maSo); // trả về null theo kiểu truyền thống nếu không tìm thấy (dùng pattern try)
        void Add(SinhVien sv);
        void Update(SinhVien sv);
        bool Remove(string maSo);
    }
}
