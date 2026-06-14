using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommitAddDemolayoutQLSV
{
    public partial class UCQLLH : UserControl
    {
        databaseDataContext db = new databaseDataContext();
        string targetSearch = "";
        int pageNumber = 1;
        int pageSize = 2; 
        int totalPages = 1;

        public UCQLLH()
        {
            InitializeComponent();
        }

        private void UCQLLH_Load(object sender, EventArgs e)
        {
            if (txtMaIDLop != null) txtMaIDLop.Enabled = false;
            LoadData();
        }

        public void LoadData()
        {
            var query = db.tbl_lophocs.AsQueryable();

            if (!string.IsNullOrEmpty(targetSearch))
            {
                query = query.Where(lh => lh.malop.Contains(targetSearch) || lh.tenlop.Contains(targetSearch));
            }

            int totalRecords = query.Count();
            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            if (totalPages <= 0) totalPages = 1;

            if (pageNumber > totalPages) pageNumber = totalPages;

            lblPhanTrang.Text = $"Trang {pageNumber}/{totalPages} | {totalRecords} bản ghi";

            List<tbl_lophoc> dSLH = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            dgv_DSLH.DataSource = dSLH;
        }

        private void dgv_DSLH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv_DSLH.Rows[e.RowIndex];

                txtMaIDLop.Text = row.Cells["id"].Value?.ToString();
                txtMaLop.Text = row.Cells["malop"].Value?.ToString();
                txtTenLop.Text = row.Cells["tenlop"].Value?.ToString();
                txtGhiChu.Text = row.Cells["ghichu"].Value?.ToString();

                txtMaLop.Enabled = false;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaLop.Text) || string.IsNullOrWhiteSpace(txtTenLop.Text))
            {
                MessageBox.Show("Mã lớp và Tên lớp không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var checkTonTai = db.tbl_lophocs.SingleOrDefault(lh => lh.malop == txtMaLop.Text.Trim());
            if (checkTonTai != null)
            {
                MessageBox.Show("Mã lớp này đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tbl_lophoc lophoc = new tbl_lophoc();
            lophoc.malop = txtMaLop.Text.Trim();
            lophoc.tenlop = txtTenLop.Text.Trim();
            lophoc.ghichu = txtGhiChu.Text.Trim();

            try
            {
                db.tbl_lophocs.InsertOnSubmit(lophoc);
                db.SubmitChanges();
                MessageBox.Show("Thêm lớp học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnLamMoi_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaIDLop.Text))
            {
                MessageBox.Show("Vui lòng chọn một lớp để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idLop = int.Parse(txtMaIDLop.Text.Trim());
                tbl_lophoc lophoc = db.tbl_lophocs.SingleOrDefault(lh => lh.id == idLop);

                if (lophoc != null)
                {
                    lophoc.tenlop = txtTenLop.Text.Trim();
                    lophoc.ghichu = txtGhiChu.Text.Trim();

                    db.SubmitChanges();
                    MessageBox.Show("Cập nhật lớp học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaIDLop.Text))
            {
                MessageBox.Show("Vui lòng chọn một lớp từ danh sách để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maLop = txtMaLop.Text.Trim();
            var checkSinhVien = db.tbl_sinhviens.Any(sv => sv.malop == maLop);
            if (checkSinhVien)
            {
                MessageBox.Show("Lớp này đang có sinh viên theo học, không thể xóa! Vui lòng chuyển hoặc xóa sinh viên trước.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa lớp {maLop} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    int idLop = int.Parse(txtMaIDLop.Text.Trim());
                    tbl_lophoc lophoc = db.tbl_lophocs.SingleOrDefault(lh => lh.id == idLop);

                    if (lophoc != null)
                    {
                        db.tbl_lophocs.DeleteOnSubmit(lophoc);
                        db.SubmitChanges();
                        MessageBox.Show("Xóa lớp học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnLamMoi_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaIDLop.Text = "";
            txtMaLop.Text = "";
            txtTenLop.Text = "";
            txtGhiChu.Text = "";
            txtMaLop.Enabled = true;
            targetSearch = "";
            txtTimKiem.Text = "";
            pageNumber = 1;
            LoadData();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            targetSearch = txtTimKiem.Text.Trim();
            pageNumber = 1;
            LoadData();
        }

        private void btnTrangDau_Click(object sender, EventArgs e)
        {
            pageNumber = 1;
            LoadData();
        }

        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {
            if (pageNumber > 1) { pageNumber--; LoadData(); }
        }

        private void btnTrangSau_Click(object sender, EventArgs e)
        {

        }

        private void btnTrangCuoi_Click(object sender, EventArgs e)
        {
            pageNumber = totalPages;
            LoadData();
        }

        private void btnXemDanhSachSV_Click(object sender, EventArgs e)
        {
            string maLop = txtMaLop.Text.Trim();
            if (string.IsNullOrEmpty(maLop))
            {
                MessageBox.Show("Vui lòng chọn một lớp để xem danh sách sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var dsSinhVien = db.tbl_sinhviens.Where(sv => sv.malop == maLop).ToList();

            if (dsSinhVien.Count == 0)
            {
                MessageBox.Show($"Lớp {maLop} hiện tại chưa có sinh viên nào.", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string danhSach = $"DANH SÁCH SINH VIÊN LỚP {maLop}:\n\n";
                foreach (var sv in dsSinhVien)
                {
                    danhSach += $"- {sv.id} | {sv.hoten} | {sv.gioitinh}\n";
                }
                MessageBox.Show(danhSach, "Xem danh sách Sinh viên", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void pnlLopHoc_Paint(object sender, PaintEventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
    }
}