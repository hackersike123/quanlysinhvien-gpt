using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyDiemSinhVien
{
    /// <summary>
    /// Triển khai in-memory đơn giản. Không thread-safe (đủ dùng WinForms đơn luồng).
    /// Bỏ nullable reference để tương thích C# 7.3
    /// </summary>
    public class InMemoryStudentRepository : IStudentRepository
    {
        private readonly List<SinhVien> _data = new List<SinhVien>();

        public IReadOnlyCollection<SinhVien> GetAll() { return _data.AsReadOnly(); }

        public SinhVien GetByCode(string maSo)
        {
            return _data.FirstOrDefault(s => s.MaSo.Equals(maSo, StringComparison.OrdinalIgnoreCase));
        }

        public void Add(SinhVien sv)
        {
            if (GetByCode(sv.MaSo) != null)
                throw new InvalidOperationException("Mã số đã tồn tại");
            _data.Add(sv);
        }

        public void Update(SinhVien sv)
        {
            var existing = GetByCode(sv.MaSo);
            if (existing == null)
                throw new InvalidOperationException("Không tìm thấy sinh viên để cập nhật");
            existing.HoTen = sv.HoTen;
            existing.Khoa = sv.Khoa;
            existing.Diem = sv.Diem;
        }

        public bool Remove(string maSo)
        {
            var sv = GetByCode(maSo);
            if (sv == null) return false;
            return _data.Remove(sv);
        }
    }
}
