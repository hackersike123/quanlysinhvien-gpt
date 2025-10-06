using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace QuanLyDiemSinhVien
{
    /// <summary>
    /// Lớp dịch vụ chứa logic nghiệp vụ (validate, CRUD, lọc, tìm kiếm...).
    /// Bổ sung giới hạn độ dài + regex kiểm tra định dạng chuỗi đầu vào.
    /// </summary>
    public class StudentService
    {
        private readonly IStudentRepository _repo;

        // Regex / giới hạn đơn giản (có thể điều chỉnh sau)
        private static readonly Regex MaSoRegex = new Regex("^[A-Za-z0-9]{1,12}$", RegexOptions.Compiled);          // 1-12 ký tự chữ hoặc số
        private const int HoTenMaxLength = 100;  // tối đa 100 ký tự
        private const int KhoaMaxLength  = 50;   // tối đa 50 ký tự

        public StudentService(IStudentRepository repo)
        {
            _repo = repo;
        }

        public SinhVien Add(string maSo, string hoTen, string khoa, string diemText)
        {
            double diem;
            ValidateBasic(maSo, hoTen, khoa, diemText, out diem);
            var sv = new SinhVien(NormalizeName(hoTen), maSo.Trim(), khoa.Trim(), diem);
            _repo.Add(sv);
            return sv;
        }

        public void Update(string maSo, string hoTen, string khoa, string diemText)
        {
            double diem;
            ValidateBasic(maSo, hoTen, khoa, diemText, out diem);
            var updated = new SinhVien(NormalizeName(hoTen), maSo.Trim(), khoa.Trim(), diem);
            _repo.Update(updated);
        }

        public bool Delete(string maSo) { return _repo.Remove(maSo.Trim()); }

        public IReadOnlyCollection<SinhVien> GetAll() { return _repo.GetAll(); }

        public IEnumerable<SinhVien> Filter(string khoa, double? min, double? max)
        {
            var data = _repo.GetAll().AsEnumerable();
            if (!string.IsNullOrWhiteSpace(khoa))
                data = data.Where(s => s.Khoa.Equals(khoa, StringComparison.OrdinalIgnoreCase));
            if (min.HasValue)
                data = data.Where(s => s.Diem >= min.Value);
            if (max.HasValue)
                data = data.Where(s => s.Diem <= max.Value);
            return data.ToList();
        }

        public IEnumerable<SinhVien> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return _repo.GetAll();
            keyword = keyword.Trim();
            return _repo.GetAll().Where(s => s.MaSo.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 || s.HoTen.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static void ValidateBasic(string maSo, string hoTen, string khoa, string diemText, out double diem)
        {
            if (string.IsNullOrWhiteSpace(maSo) || string.IsNullOrWhiteSpace(hoTen) || string.IsNullOrWhiteSpace(khoa) || string.IsNullOrWhiteSpace(diemText))
                throw new ArgumentException("Thiếu thông tin bắt buộc");

            // MaSo: chỉ chữ/số, tối đa 12 ký tự
            string maSoTrim = maSo.Trim();
            if (!MaSoRegex.IsMatch(maSoTrim))
                throw new ArgumentException("Mã số chỉ gồm chữ hoặc số (1-12 ký tự)");

            // Họ tên: độ dài, không chứa ký tự số / ký tự đặc biệt (cho phép khoảng trắng và chữ unicode)
            string hoTenTrim = hoTen.Trim();
            if (hoTenTrim.Length > HoTenMaxLength)
                throw new ArgumentException("Họ tên quá dài (tối đa 100 ký tự)");
            if (hoTenTrim.Any(ch => !(char.IsLetter(ch) || char.IsWhiteSpace(ch))))
                throw new ArgumentException("Họ tên chỉ được chứa chữ và khoảng trắng");

            // Khoa: độ dài + ký tự hợp lệ (chữ, số, khoảng trắng, & - _ ) giản lược
            string khoaTrim = khoa.Trim();
            if (khoaTrim.Length > KhoaMaxLength)
                throw new ArgumentException("Tên khoa quá dài (tối đa 50 ký tự)");
            if (khoaTrim.Any(ch => !(char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch) || ch=='-' || ch=='_' )))
                throw new ArgumentException("Khoa chứa ký tự không hợp lệ");

            // Điểm
            if (!double.TryParse(diemText.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out diem))
            {
                if (!double.TryParse(diemText.Trim(), out diem))
                    throw new ArgumentException("Điểm không hợp lệ");
            }
            if (diem < 0 || diem > 10) throw new ArgumentOutOfRangeException("diem", "Điểm phải 0-10");
        }

        private static string NormalizeName(string name)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
        }
    }
}
