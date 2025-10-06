using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace QuanLyDiemSinhVien
{
    public partial class frmMain : Form
    {
        // BỎ các field trùng với Designer: btnSua, btnXoa, btnThongKe, txtSearch, lblSearch
        private StudentService _studentService; 
        private string _currentSelectedMaSo = null;
        private bool _isEditMode = false;

        public frmMain()
        {
            InitializeComponent();
            if (IsDesignMode()) return;
            SafeInitRuntime();
        }

        private void SafeInitRuntime()
        {
            _studentService = new StudentService(new InMemoryStudentRepository());
            if (cboKhoa.Items.Count == 0)
                cboKhoa.Items.AddRange(new string[] { "CNTT", "Kế toán", "Tài chính" });
            dgvSinhVien.CellClick += dgvSinhVien_CellClick;
            dgvSinhVien.CellDoubleClick += (s, e) => LoadSelectedRowToForm();
            txtSearch.TextChanged += (s, e) => ApplyFilter();
        }

        private bool IsDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
                   (Site != null && Site.DesignMode) ||
                   System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower().Contains("devenv");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (IsDesignMode()) return;
            try
            {
                if (_isEditMode)
                {
                    MessageBox.Show("Đang ở chế độ sửa. Nhấn Clear để thêm mới.");
                    return;
                }
                EnsureService();
                var maSo = txtMaSo.Text.Trim();
                var hoTen = txtHoTen.Text.Trim();
                var khoa = !string.IsNullOrWhiteSpace(cboKhoa.Text) ? cboKhoa.Text.Trim() : txtKhoa.Text.Trim();
                var diemText = txtDiemTB.Text.Trim();
                _studentService.Add(maSo, hoTen, khoa, diemText);
                MessageBox.Show("Thêm sinh viên thành công!");
                ClearInputFields();
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (IsDesignMode()) return;
            if (string.IsNullOrEmpty(_currentSelectedMaSo)) { MessageBox.Show("Chưa chọn sinh viên để sửa"); return; }
            if (DialogResult.Yes != MessageBox.Show("Xác nhận cập nhật sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) return;
            try
            {
                EnsureService();
                var maSo = _currentSelectedMaSo;
                var hoTen = txtHoTen.Text.Trim();
                var khoa = !string.IsNullOrWhiteSpace(cboKhoa.Text) ? cboKhoa.Text.Trim() : txtKhoa.Text.Trim();
                var diemText = txtDiemTB.Text.Trim();
                _studentService.Update(maSo, hoTen, khoa, diemText);
                MessageBox.Show("Cập nhật thành công");
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (IsDesignMode()) return;
            if (string.IsNullOrEmpty(_currentSelectedMaSo)) { MessageBox.Show("Chưa chọn sinh viên để xóa"); return; }
            if (DialogResult.Yes != MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) return;
            EnsureService();
            if (_studentService.Delete(_currentSelectedMaSo))
            {
                MessageBox.Show("Đã xóa");
                ClearInputFields();
                _currentSelectedMaSo = null;
                RefreshGrid();
            }
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            if (IsDesignMode()) return;
            EnsureService();
            var all = _studentService.GetAll();
            if (all.Count == 0) { MessageBox.Show("Chưa có dữ liệu"); return; }
            double tbChung = all.Average(s => s.Diem);
            var byKhoa = all.GroupBy(s => s.Khoa).Select(g => string.Format("{0}: TB={1:0.00}, SL={2}", g.Key, g.Average(s => s.Diem), g.Count()));
            string msg = "Điểm TB chung: " + tbChung.ToString("0.00") + "\n" + string.Join("\n", byKhoa);
            MessageBox.Show(msg, "Thống kê");
        }

        private void RefreshGrid() { ApplyFilter(); }

        private void ApplyFilter()
        {
            if (IsDesignMode()) return;
            EnsureService();
            string keyword = txtSearch.Text;
            string selectedKhoa = cboKhoa.SelectedItem == null ? null : cboKhoa.SelectedItem.ToString();
            IEnumerable<SinhVien> data = string.IsNullOrWhiteSpace(keyword) ? _studentService.GetAll() : _studentService.Search(keyword);
            if (!string.IsNullOrWhiteSpace(selectedKhoa)) data = data.Where(s => s.Khoa.Equals(selectedKhoa, StringComparison.OrdinalIgnoreCase));
            RenderRows(data);
        }

        private void RenderRows(IEnumerable<SinhVien> data)
        {
            dgvSinhVien.Rows.Clear();
            foreach (var sv in data)
                dgvSinhVien.Rows.Add(sv.MaSo, sv.HoTen, sv.Khoa, sv.Diem, GetXepLoai(sv.Diem));
            ApplyRowStyles();
        }

        private void cboKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsDesignMode()) return;
            ApplyFilter();
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (IsDesignMode()) return;
            if (e.RowIndex < 0) return; LoadSelectedRowToForm();
        }

        private void LoadSelectedRowToForm()
        {
            if (IsDesignMode()) return;
            if (dgvSinhVien.CurrentRow == null || dgvSinhVien.CurrentRow.IsNewRow) return;
            var row = dgvSinhVien.CurrentRow;
            _currentSelectedMaSo = row.Cells[0].Value == null ? null : row.Cells[0].Value.ToString();
            if (string.IsNullOrEmpty(_currentSelectedMaSo)) return;
            txtMaSo.Text = _currentSelectedMaSo;
            txtHoTen.Text = row.Cells[1].Value == null ? "" : row.Cells[1].Value.ToString();
            txtKhoa.Text = row.Cells[2].Value == null ? "" : row.Cells[2].Value.ToString();
            txtDiemTB.Text = row.Cells[3].Value == null ? "" : row.Cells[3].Value.ToString();
            EnterEditMode();
        }

        private void EnterEditMode()
        {
            _isEditMode = true;
            txtMaSo.Enabled = false;
        }

        private string GetXepLoai(double diem)
        {
            if (diem >= 8) return "Giỏi";
            if (diem >= 6.5) return "Khá";
            if (diem >= 5) return "Trung bình";
            return "Yếu";
        }

        private void ApplyRowStyles()
        {
            foreach (DataGridViewRow row in dgvSinhVien.Rows)
            {
                if (row.IsNewRow) continue;
                double diem; if (!double.TryParse(row.Cells[3].Value == null ? null : row.Cells[3].Value.ToString(), out diem)) continue;
                if (diem < 5) { row.DefaultCellStyle.BackColor = Color.MistyRose; row.DefaultCellStyle.ForeColor = Color.DarkRed; }
                else if (diem >= 8) { row.DefaultCellStyle.BackColor = Color.Honeydew; row.DefaultCellStyle.ForeColor = Color.DarkGreen; }
                else { row.DefaultCellStyle.BackColor = Color.White; row.DefaultCellStyle.ForeColor = Color.Black; }
            }
        }

        private void ClearInputFields()
        {
            txtMaSo.Clear(); txtHoTen.Clear(); txtKhoa.Clear(); txtDiemTB.Clear(); cboKhoa.SelectedIndex = -1;
            _currentSelectedMaSo = null; _isEditMode = false; txtMaSo.Enabled = true; txtMaSo.Focus();
        }

        private void EnsureService()
        {
            if (_studentService == null)
                _studentService = new StudentService(new InMemoryStudentRepository());
        }
    }
}
