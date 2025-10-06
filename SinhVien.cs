using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* =============================================================
 * GHI CHÚ MODEL
 * Lớp SinhVien: biểu diễn thực thể Sinh viên trong hệ thống.
 * Thuộc tính: HoTen, MaSo, Khoa, Diem.
 * Có thêm các hàm tĩnh tìm kiếm theo Mã số và Khoa (giữ tương thích cũ).
 * ============================================================= */

namespace QuanLyDiemSinhVien
{
    // Lớp SinhVien đại diện cho một sinh viên. Public để các tầng khác (Service/Repository/UI) truy cập được.
    public class SinhVien
    {
        public string HoTen { get; set; }
        public string MaSo { get; set; }
        public string Khoa { get; set; }
        public double Diem { get; set; }

        public SinhVien(string hoTen, string maSo, string khoa, double diem)
        {
            HoTen = hoTen;
            MaSo = maSo;
            Khoa = khoa;
            Diem = diem;
        }
        // Tìm kiếm theo mã số (giữ lại hàm cũ để tương thích đơn giản)
        public static List<SinhVien> TimKiemTheoMaSo(List<SinhVien> danhSach, string maSo)
        {
            return danhSach.Where(sv => sv.MaSo.Equals(maSo, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        // Tìm kiếm theo khoa
        public static List<SinhVien> TimKiemTheoKhoa(List<SinhVien> danhSach, string khoa)
        {
            return danhSach.Where(sv => sv.Khoa.Equals(khoa, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
