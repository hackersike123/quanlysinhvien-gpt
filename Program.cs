using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/* =============================================================
 * GHI CHÚ
 * Program.cs: Điểm vào ứng dụng WinForms. Không thay đổi logic.
 * Đã bổ sung chú thích tiếng Việt phục vụ commit mô tả.
 * ============================================================= */

namespace QuanLyDiemSinhVien
{
    internal static class Program
    {
        /// <summary>
        /// Điểm vào chính của ứng dụng.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
